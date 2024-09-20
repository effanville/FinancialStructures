using System;
using System.Linq;
using System.Text;

using Effanville.Common.Structure.Reporting;
using Effanville.Common.Structure.WebAccess;

namespace Effanville.FinancialStructures.Stocks.Download
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

            string[] instruments = sb.ToString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            for (index = 0; index < instruments.Length; index++)
            {
                string item = instruments[index];
                instruments[index] = item.Trim();
            }

            instruments = instruments.DistinctBy(x => x.Substring(8)).ToArray();
            return instruments;
        }
    }
}