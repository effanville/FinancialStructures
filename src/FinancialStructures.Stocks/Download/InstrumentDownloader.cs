using System;
using System.IO.Abstractions;
using System.Text;

using Common.Structure.Reporting;
using Common.Structure.WebAccess;

namespace FinancialStructures.Stocks.Download
{
    public static class InstrumentDownloader
    {
        /// <summary>
        /// Get instruments from an index
        /// </summary>
        /// <param name="indexName">FTSE-100,FTSE-250,FTSE-350 etc</param>
        public static string[] GetIndexInstruments(string indexName)
        {
            string ftseWebsite = $"https://www.londonstockexchange.com/indices/{indexName}/constituents/table";
            IReportLogger logger = new LogReporter(null, true);

            var driver = WebDownloader.GetCachedInstance(forceNew: true);
            StringBuilder sb = new StringBuilder();
            int index = 1;
            while (index < 10)
            {
                string url = ftseWebsite + $"?page={index}";
                string text = null;
                int numberTries = 0;
                while (string.IsNullOrEmpty(text) && numberTries < 20)
                {
                    text = WebDownloader.GetElementText(driver, url, "ftse-index-table", 1000 + 1000 * numberTries,
                        logger);
                    numberTries++;
                }

                if (text != null)
                {
                    if (text == "What's this?\r\n12345")
                    {
                        break;
                    }

                    text = text.Substring(text.IndexOf("Change %", StringComparison.InvariantCulture) + "Change %".Length);
                    text = text.Replace("12345", "").Trim('\r').Trim('\n');
                    _ = sb.Append(text)
                        .AppendLine();
                }

                index++;
            }

            return sb.ToString().Split("\r\n");
        }
    }
}