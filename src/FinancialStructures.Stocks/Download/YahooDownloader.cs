using System;
using System.Globalization;
using System.Threading.Tasks;
using Common.Structure.Reporting;

using FinancialStructures.Download.Implementation;
using FinancialStructures.Stocks.Implementation;

namespace FinancialStructures.Stocks.Download
{
    /// <summary>
    /// An implementation of an <see cref="IStockDownloader"/> for Yahoo websites.
    /// </summary>
    internal sealed class YahooDownloader : IStockDownloader
    {
        private static readonly char DefaultCommaSeparator = ',';

        /// <inheritdoc/>
        public string BaseUrl => "https://uk.finance.yahoo.com/";

        internal YahooDownloader()
        {
        }

        private static async Task<bool> TryGetPriceInternal(string url, string financialCode, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string webData = await DownloadHelper.GetWebData(url, reportLogger);
            if (string.IsNullOrEmpty(webData))
            {
                reportLogger?.Error("Downloading", $"Could not download data from {url}");
                return false;
            }

            decimal? value = GetValue(webData, financialCode);
            if (!value.HasValue)
            {
                return false;
            }

            retrieveValueAction(value.Value);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPriceData(
            string financialCode,
            Action<StockDay> retrieveValueAction,
            IReportLogger reportLogger = null)
        {
            string url = BuildQueryUrl(BaseUrl, financialCode);
            string stockWebsite = await DownloadHelper.GetWebData(url, reportLogger);
            if (string.IsNullOrEmpty(stockWebsite))
            {
                reportLogger?.Error("Downloading", $"Could not download data from {url}");
                return false;
            }

            decimal? close = GetValue(stockWebsite, financialCode);
            decimal? open = FindAndGetSingleValue(stockWebsite, "data-test=\"OPEN-value\"", true);
            Tuple<decimal, decimal> range = FindAndGetDoubleValues(stockWebsite, "data-test=\"DAYS_RANGE-value\"");
            decimal? volume = FindAndGetSingleValue(stockWebsite, $"data-test=\"TD_VOLUME-value\"><fin-streamer data-symbol=\"{financialCode}\" data-field=\"regularMarketVolume\" data-trend=\"none\" data-pricehint=\"2\" data-dfield=\"longFmt\"", true);

            DateTime date = DateTime.Now.TimeOfDay > new DateTime(2010, 1, 1, 16, 30, 0).TimeOfDay ? DateTime.Today : DateTime.Today.AddDays(-1);
            retrieveValueAction(new StockDay(date, open.Value, range.Item2, range.Item1, close.Value, volume ?? 0.0m));
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetFullPriceHistory(
            string financialCode,
            DateTime firstDate,
            DateTime lastDate,
            TimeSpan recordInterval,
            Action<IStock> getHistory,
            IReportLogger reportLogger = null)
        {
            Uri downloadUrl = new Uri($"https://query1.finance.yahoo.com/v7/finance/download/{financialCode}?period1={DateToYahooInt(firstDate)}&period2={DateToYahooInt(lastDate)}&interval=1d&&events=history&includeAdjustedClose=true&filter=history&frequency=1d");
            string stockWebsite = await DownloadHelper.GetWebData(downloadUrl.ToString(), reportLogger);
            Stock stock = new Stock();

            // stockWebsite here is a csv file
            string newLineSeparator = stockWebsite.Contains("\r\n") ? "\r\n" : "\n";
            string[] lines = stockWebsite.Split(newLineSeparator);

            if (lines.Length <= 1)
            {
                getHistory(stock);
                return true;
            }

            int numberEntries = 0;
            for (int lineIndex = 1; lineIndex < lines.Length; lineIndex++)
            {
                string[] entries = lines[lineIndex].Split(DefaultCommaSeparator);
                string dateString = entries[0];
                try
                {
                    DateTime date = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
                    var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    decimal open = decimal.Parse(entries[1]);
                    decimal high = decimal.Parse(entries[2]);
                    decimal low = decimal.Parse(entries[3]);
                    decimal close = decimal.Parse(entries[4]);
                    decimal volume = decimal.Parse(entries[6]);
                    stock.AddValue(utcDate, open, high, low, close, volume);
                    numberEntries++;
                }
                catch (Exception ex)
                {
                    reportLogger?.Error("Downloading", $"Could not convert stock {stock.Name} data- {lines[lineIndex]}. Error {ex.Message}");
                }
            }

            reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, "Downloading", $"Added {lines.Length - 1} to stock {stock.Name}");

            stock.Sort();
            getHistory(stock);
            return true;
        }

        private static string BuildQueryUrl(string url, string identifier)
        {
            return $"{url}/quote/{identifier}";
        }

        private static int DateToYahooInt(DateTime date)
        {
            return int.Parse((date - new DateTime(1970, 1, 1)).TotalSeconds.ToString());
        }

        /// <summary>
        /// Enables retrieval of the financial code specifier for the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFinancialCode(string url)
        {
            string urlSearchString = "/quote/";
            int startIndex = url.IndexOf(urlSearchString);
            int endIndex = url.IndexOfAny(new[] { '/', '?' }, startIndex + urlSearchString.Length);
            if (endIndex == -1)
            {
                endIndex = url.Length;
            }
            string code = url.Substring(startIndex + urlSearchString.Length, endIndex - startIndex - urlSearchString.Length);
            code = code.Replace("%5E", "^").Replace("%3D", "=").ToUpper();

            // seems to be a bug in the website where it uses the wrong code.
            if (code.Equals("USDGBP=X"))
            {
                code = "GBP=X";
            }

            return code;
        }

        private static decimal? FindAndGetSingleValue(string searchString, string findString, bool includeComma, int containedWithin = 50)
        {
            int index = searchString.IndexOf(findString);
            int lengthToSearch = Math.Min(containedWithin, searchString.Length - index - findString.Length);
            return DownloadHelper.ParseDataIntoNumber(searchString, index, findString.Length, lengthToSearch, includeComma);
        }

        private static Tuple<decimal, decimal> FindAndGetDoubleValues(string searchString, string findString, int containedWithin = 50)
        {
            int index = searchString.IndexOf(findString);
            int lengthToSearch = Math.Min(containedWithin, searchString.Length - index - findString.Length);
            decimal? firstValue = DownloadHelper.ParseDataIntoNumber(searchString, index, findString.Length, lengthToSearch, true);

            if (!firstValue.HasValue)
            {
                return new Tuple<decimal, decimal>(decimal.MinValue, decimal.MinValue);
            }

            string value = searchString.Substring(index + findString.Length, lengthToSearch);
            int separator = value.IndexOf("-");
            decimal? value2 = DownloadHelper.ParseDataIntoNumber(value, separator, 0, lengthToSearch, true);
            if (!value2.HasValue)
            {
                return new Tuple<decimal, decimal>(decimal.MinValue, decimal.MinValue);
            }

            return new Tuple<decimal, decimal>(firstValue.Value, value2.Value);
        }

        private static decimal? GetValue(string webData, string financialCode)
        {
            int number = 2;
            int poundsIndex;
            string searchString;
            do
            {
                searchString = $"data-symbol=\"{financialCode}\" data-test=\"qsp-price\" data-field=\"regularMarketPrice\" data-trend=\"none\" data-pricehint=\"{number}\"";
                poundsIndex = webData.IndexOf(searchString);
                number++;
            }
            while (poundsIndex == -1 && number < 100);

            decimal? value = DownloadHelper.ParseDataIntoNumber(webData, poundsIndex, searchString.Length, 20, true);
            if (value.HasValue)
            {
                if (webData.Contains("Currency in GBp"))
                {
                    return value.Value / 100.0m;
                }

                return value.Value;
            }

            return null;
        }
    }
}
