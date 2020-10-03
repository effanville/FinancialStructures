﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinancialStructures.NamingStructures;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;
using StructureCommon.WebAccess;

namespace FinancialStructures.StockStructures
{
    public enum DownloadType
    {
        All,
        Latest
    }

    public class ExchangeStocks
    {
        public ExchangeStocks()
        {
        }

        public double GetValue(NameData name, DateTime date, DataStream datatype = DataStream.Close)
        {
            foreach (Stock stock in Stocks)
            {
                if (stock.Name.IsEqualTo(name))
                {
                    return stock.Value(date, datatype);
                }
            }

            return 0.0;
        }

        public DateTime EarliestDate()
        {
            DateTime earliest = Stocks[0].EarliestTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockEarliest = Stocks[stockIndex].EarliestTime();
                if (stockEarliest < earliest)
                {
                    earliest = stockEarliest;
                }
            }

            return earliest;
        }

        public DateTime LatestEarliestDate()
        {
            DateTime earliest = Stocks[0].EarliestTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockEarliest = Stocks[stockIndex].EarliestTime();
                if (stockEarliest > earliest)
                {
                    earliest = stockEarliest;
                }
            }

            return earliest;
        }

        public DateTime LastDate()
        {
            DateTime last = Stocks[0].LastTime();

            for (int stockIndex = 1; stockIndex < Stocks.Count; stockIndex++)
            {
                DateTime stockLast = Stocks[stockIndex].LastTime();
                if (stockLast < last)
                {
                    last = stockLast;
                }
            }

            return last;
        }

        public List<Stock> Stocks
        {
            get;
            set;
        } = new List<Stock>();

        public bool CheckValidity()
        {
            if (Stocks.Count() == 0)
            {
                return false;
            }

            return true;
        }

        public void LoadExchangeStocks(string filePath, IReportLogger reportLogger = null)
        {
            if (File.Exists(filePath))
            {
                ExchangeStocks database = XmlFileAccess.ReadFromXmlFile<ExchangeStocks>(filePath, out string error);
                if (database != null)
                {
                    Stocks = database.Stocks;
                }
                else
                {
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.Loading, $"No Database Loaded from {filePath}. Error {error}.");
                }
                return;
            }

            _ = reportLogger?.LogUseful(ReportType.Report, ReportLocation.Loading, "Loaded Empty New Database.");
            Stocks = new List<Stock>();
        }

        public void SaveExchangeStocks(string filePath, IReportLogger reportLogger = null)
        {
            XmlFileAccess.WriteToXmlFile<ExchangeStocks>(filePath, this, out string error);
            if (error != null)
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.Saving, error);
            }
        }

        public void Download(DownloadType downloadType, DateTime startDate, DateTime endDate, IReportLogger reportLogger = null)
        {
            foreach (Stock stock in Stocks)
            {
                Uri downloadUrl;
                if (downloadType == DownloadType.All)
                {
                    downloadUrl = new Uri(stock.Name.Url + $"/history?period1={DateToYahooInt(startDate)}&period2={DateToYahooInt(endDate)}&interval=1d&filter=history&frequency=1d");
                }
                else
                {
                    downloadUrl = new Uri(stock.Name.Url);
                }
                string stockWebsite = WebDownloader.DownloadFromURLasync(downloadUrl.ToString(), reportLogger).Result;
                ProcessAndAddData(downloadType, stock, stockWebsite, reportLogger);
            }
        }

        private int DateToYahooInt(DateTime date)
        {
            return int.Parse((date - new DateTime(1970, 1, 1)).TotalSeconds.ToString());
            throw new NotImplementedException();
        }

        private DateTime YahooIntToDate(int yahooInt)
        {
            return new DateTime(1970, 1, 1).AddSeconds(yahooInt);
        }



        private void ProcessAndAddData(DownloadType download, Stock stock, string websiteHtml, IReportLogger reportLogger = null)
        {
            if (download == DownloadType.All)
            {
                string findString = "\"HistoricalPriceStore\":{\"prices\":";
                int historyStartIndex = websiteHtml.IndexOf(findString);

                string dataLeft = websiteHtml.Substring(historyStartIndex + findString.Length);
                // data is of form {"date":1582907959,"open":150.4199981689453,"high":152.3000030517578,"low":146.60000610351562,"close":148.74000549316406,"volume":120763559,"adjclose":148.74000549316406}. 
                // Iterate through these until stop.
                int numberEntriesAdded = 0;
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
                        int yahooInt = int.Parse(FindAndGetSingleValue(dayValues, "date").ToString());
                        DateTime date = YahooIntToDate(yahooInt);
                        double open = FindAndGetSingleValue(dayValues, "open", false);
                        double high = FindAndGetSingleValue(dayValues, "high", false);
                        double low = FindAndGetSingleValue(dayValues, "low", false);
                        double close = FindAndGetSingleValue(dayValues, "close", false);
                        double volume = FindAndGetSingleValue(dayValues, "volume", false);
                        stock.AddValue(date, open, high, low, close, volume);
                        dataLeft = dataLeft.Substring(dayEndIndex);
                        numberEntriesAdded++;
                    }
                    else
                    {
                        break;
                    }
                }

                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Report, ReportLocation.Downloading, $"Added {numberEntriesAdded} to stock {stock.Name}");
            }

            if (download == DownloadType.Latest)
            {
                double close = FindAndGetSingleValue(websiteHtml, "<span class=\"Trsdu(0.3s) Fw(b) Fz(36px) Mb(-4px) D(ib)\" data-reactid=\"34\">");
                double open = FindAndGetSingleValue(websiteHtml, "data-test=\"OPEN-value\" data-reactid=\"46\"><span class=\"Trsdu(0.3s) \" data-reactid=\"47\">");
                Tuple<double, double> range = FindAndGetDoubleValues(websiteHtml, "data-test=\"DAYS_RANGE-value\" data-reactid=\"61\">");
                double volume = FindAndGetSingleValue(websiteHtml, "data-test=\"TD_VOLUME-value\" data-reactid=\"69\"><span class=\"Trsdu(0.3s) \" data-reactid=\"70\">");

                DateTime date = DateTime.Now.TimeOfDay > new DateTime(2010, 1, 1, 16, 30, 0).TimeOfDay ? DateTime.Today : DateTime.Today.AddDays(-1);
                stock.AddValue(date, open, range.Item2, range.Item1, close, volume);
            }

            stock.Sort();
        }

        private double FindAndGetSingleValue(string searchString, string findString, bool includeCommas = true, int containedWithin = 50)
        {
            bool continuer(char c)
            {
                if (char.IsDigit(c) || c == '.' || (includeCommas && c == ','))
                {
                    return true;
                }

                return false;
            };

            int index = searchString.IndexOf(findString);
            int lengthToSearch = Math.Min(containedWithin, searchString.Length - index - findString.Length);
            string value = searchString.Substring(index + findString.Length, lengthToSearch);

            char[] digits = value.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

            string str = new string(digits);
            if (string.IsNullOrEmpty(str))
            {
                return double.NaN;
            }
            return double.Parse(str);
        }

        private Tuple<double, double> FindAndGetDoubleValues(string searchString, string findString, bool includeCommas = true, int containedWithin = 50)
        {
            bool continuer(char c)
            {
                if (char.IsDigit(c) || c == '.' || (includeCommas && c == ','))
                {
                    return true;
                }

                return false;
            };

            int index = searchString.IndexOf(findString);
            int lengthToSearch = Math.Min(containedWithin, searchString.Length - index - findString.Length);
            string value = searchString.Substring(index + findString.Length, lengthToSearch);

            char[] digits = value.SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();

            string str = new string(digits);
            if (string.IsNullOrEmpty(str))
            {
                return new Tuple<double, double>(double.NaN, double.NaN);
            }
            double value1 = double.Parse(str);
            int separator = value.IndexOf("-");
            char[] secondDigits = value.Substring(separator).SkipWhile(c => !char.IsDigit(c)).TakeWhile(continuer).ToArray();
            string str2 = new string(secondDigits);
            double value2 = double.Parse(str2);
            return new Tuple<double, double>(value1, value2);
        }

        public void Configure(string stockFilePath)
        {
            string[] fileContents = new string[] { };
            try
            {
                fileContents = File.ReadAllLines(stockFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read from file located at {stockFilePath}: {ex.Message}");
            }

            if (fileContents.Length == 0)
            {
                Console.WriteLine("Nothing in file selected, but expected stock company, name, url data.");
            }

            foreach (string line in fileContents)
            {
                string[] inputs = line.Split(',');
                AddStock(inputs);
            }
        }

        private void AddStock(string[] parameters)
        {
            if (parameters.Length != 3)
            {
                Console.WriteLine("Insufficient Data in line to add Stock");
                return;
            }

            Stock stock = new Stock(parameters[0], parameters[1], parameters[2]);
            Stocks.Add(stock);
        }
    }
}