using System;
using System.Threading.Tasks;
using Common.Structure.Reporting;
using FinancialStructures.NamingStructures;
using FinancialStructures.StockStructures;
using FinancialStructures.StockStructures.Implementation;

namespace FinancialStructures.Download.Implementation
{
    /// <summary>
    /// An implementation of an <see cref="IPriceDownloader"/> for Yahoo websites.
    /// </summary>
    internal sealed class YahooDownloader : IPriceDownloader
    {
        /// <inheritdoc/>
        public string BaseUrl => "https://uk.finance.yahoo.com/";

        internal YahooDownloader()
        {
        }

        /// <inheritdoc/>
        public Task<bool> TryGetIdentifier(TwoName name, Action<string> getIdentifierAction, IReportLogger reportLogger = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPriceFromUrl(string url, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string financialCode = GetFinancialCode(url);
            return await TryGetPriceInternal(url, financialCode, retrieveValueAction, reportLogger);
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetLatestPrice(string financialCode, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string url = BuildQueryUrl(BaseUrl, financialCode);
            return await TryGetPriceInternal(url, financialCode, retrieveValueAction, reportLogger);
        }

        private static async Task<bool> TryGetPriceInternal(string url, string financialCode, Action<decimal> retrieveValueAction, IReportLogger reportLogger = null)
        {
            string webData = await DownloadHelper.GetWebData(url, reportLogger);
            if (string.IsNullOrEmpty(webData))
            {
                _ = reportLogger?.LogUsefulError(ReportLocation.Downloading, $"Could not download data from {url}");
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
                _ = reportLogger?.LogUsefulError(ReportLocation.Downloading, $"Could not download data from {url}");
                return false;
            }

            decimal? close = GetValue(stockWebsite, financialCode);
            decimal? open = FindAndGetSingleValue(stockWebsite, "data-test=\"OPEN-value\"", true);
            Tuple<decimal, decimal> range = FindAndGetDoubleValues(stockWebsite, "data-test=\"DAYS_RANGE-value\"");
            decimal? volume = FindAndGetSingleValue(stockWebsite, $"data-test=\"TD_VOLUME-value\"><fin-streamer data-symbol=\"{financialCode}\" data-field=\"regularMarketVolume\" data-trend=\"none\" data-pricehint=\"2\" data-dfield=\"longFmt\"", true);

            DateTime date = DateTime.Now.TimeOfDay > new DateTime(2010, 1, 1, 16, 30, 0).TimeOfDay ? DateTime.Today : DateTime.Today.AddDays(-1);
            retrieveValueAction(new StockDay(date, open.Value, range.Item2, range.Item1, close.Value, volume.HasValue ? volume.Value : 0.0m));
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
            Uri downloadUrl = new Uri($"{BuildQueryUrl(BaseUrl, financialCode)}/history?period1={DateToYahooInt(firstDate)}&period2={DateToYahooInt(lastDate)}&interval=1d&filter=history&frequency=1d");
            string stockWebsite = await DownloadHelper.GetWebData(downloadUrl.ToString(), reportLogger);
            Stock stock = new Stock();
            string findString = "\"HistoricalPriceStore\":{\"prices\":";
            int historyStartIndex = stockWebsite.IndexOf(findString);

            string dataLeft = stockWebsite.Substring(historyStartIndex + findString.Length);
            // data is of form {"date":1582907959,"open":150.4199981689453,"high":152.3000030517578,"low":146.60000610351562,"close":148.74000549316406,"volume":120763559,"adjclose":148.74000549316406}.
            // Iterate through these until stop.
            int numberEntriesAdded = 0;
            var characters = dataLeft.Substring(0, 2);
            if (characters.Equals("[]"))
            {
                getHistory(stock);
                return true;
            }

            try
            {

                while (dataLeft.Length > 0)
                {
                    int dayFirstIndex = dataLeft.IndexOf('{');
                    int dayEndIndex = dataLeft.IndexOf('}', dayFirstIndex);
                    string dayValues = dataLeft.Substring(dayFirstIndex, dayEndIndex - dayFirstIndex);
                    if (dayValues.Contains("DIVIDEND"))
                    {
                        dataLeft = dataLeft.Substring(dayEndIndex);
                        continue;
                    }
                    else if (dayValues.Contains("date"))
                    {
                        int yahooInt = int.Parse(FindAndGetSingleValue(dayValues, "date", false).ToString());
                        DateTime date = YahooIntToDate(yahooInt);
                        var localDate = date.ToLocalTime();
                        decimal? open = FindAndGetSingleValue(dayValues, "open", false);
                        decimal? high = FindAndGetSingleValue(dayValues, "high", false);
                        decimal? low = FindAndGetSingleValue(dayValues, "low", false);
                        decimal? close = FindAndGetSingleValue(dayValues, "close", false);
                        decimal? volume = FindAndGetSingleValue(dayValues, "volume", false);
                        stock.AddValue(localDate, ParseNullable(open), ParseNullable(high), ParseNullable(low), ParseNullable(close), ParseNullable(volume));
                        dataLeft = dataLeft.Substring(dayEndIndex);
                        numberEntriesAdded++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _ = reportLogger?.LogUsefulError(ReportLocation.Downloading, $"Error when downloading: {ex.Message}");
                return false;
            }

            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Information, ReportLocation.Downloading, $"Added {numberEntriesAdded} to stock {stock.Name}");

            stock.Sort();
            getHistory(stock);
            return true;
        }

        private static decimal ParseNullable(decimal? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }

            return decimal.MinValue;
        }

        private static string BuildQueryUrl(string url, string identifier)
        {
            return $"{url}/quote/{identifier}";
        }

        private static int DateToYahooInt(DateTime date)
        {
            return int.Parse((date - new DateTime(1970, 1, 1)).TotalSeconds.ToString());
        }

        private static DateTime YahooIntToDate(int yahooInt)
        {
            return new DateTime(1970, 1, 1).AddSeconds(yahooInt);
        }

        /// <summary>
        /// Enables retrieval of the financial code specifier for the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFinancialCode(string url)
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

        private decimal? FindAndGetSingleValue(string searchString, string findString, bool includeComma, int containedWithin = 50)
        {
            int index = searchString.IndexOf(findString);
            int lengthToSearch = Math.Min(containedWithin, searchString.Length - index - findString.Length);
            return DownloadHelper.ParseDataIntoNumber(searchString, index, findString.Length, lengthToSearch, includeComma);
        }

        private Tuple<decimal, decimal> FindAndGetDoubleValues(string searchString, string findString, int containedWithin = 50)
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
