using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Structure.Reporting;
using Common.Structure.WebAccess;

using HtmlAgilityPack;

namespace FinancialStructures.Stocks.Download
{
    public static class FundamentalDataDownloader
    {
        public static async Task<Dictionary<string, string>> GetExtraData(string instrumentUrl, IReportLogger logger = null)
        {
            string urlData = await WebDownloader.DownloadFromURLasync(instrumentUrl, logger);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(urlData);
            var quoteSummaryElement = htmlDocument.GetElementbyId("quote-summary");
            if (quoteSummaryElement == null)
            {
                return null;
            }

            var summaryColumn1 = quoteSummaryElement.ChildNodes.First();
            if (summaryColumn1 == null)
            {
                return null;
            }

            var summaryColumn2 = quoteSummaryElement.ChildNodes.Last();
            if (summaryColumn2 == null)
            {
                return null;
            }

            var tableValues = summaryColumn1.ChildNodes.First().ChildNodes.First().ChildNodes;
            if (tableValues == null)
            {
                return null;
            }
            var table2Values = summaryColumn2.ChildNodes.First().ChildNodes.First().ChildNodes;
            if (table2Values == null)
            {
                return null;
            }
            
            var dict = new Dictionary<string, string>();
            foreach (var child in tableValues.Union(table2Values))
            {
                string key = child.ChildNodes.First().InnerText;
                string value = child.ChildNodes.Last().InnerText;
                dict[key] = value;
            }

            return dict;
        }
    }
}