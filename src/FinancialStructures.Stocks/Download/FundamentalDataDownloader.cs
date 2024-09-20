using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;
using Effanville.Common.Structure.WebAccess;

using HtmlAgilityPack;

namespace Effanville.FinancialStructures.Stocks.Download
{
    public static class FundamentalDataDownloader
    {
        private static HtmlNode GetDescendentFromTag(HtmlNode node, string tagName, string tagValue)
        {
            if (node == null)
            {
                return null;
            }

            if (node.HasAttributes)
            {
                HtmlAttribute matching = node.Attributes.FirstOrDefault(a => a.Name == tagName && a.Value == tagValue);
                if (matching != null)
                {
                    return node;
                }
            }

            if (!node.HasChildNodes)
            {
                return null;
            }

            foreach (HtmlNode childNode in node.ChildNodes)
            {
                HtmlNode descendentMatches = GetDescendentFromTag(childNode, tagName, tagValue);
                if (descendentMatches != null)
                {
                    return descendentMatches;
                }
            }

            return null;
        }

        public static async Task<Dictionary<string, string>> GetExtraData(string instrumentUrl, IReportLogger logger = null)
        {
            string urlData = await WebDownloader.DownloadFromURLasync(instrumentUrl, addCookie: false, logger);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(urlData);
            HtmlNode quoteSummaryElement = GetDescendentFromTag(htmlDocument.DocumentNode, "data-testid", "quote-statistics");
            if (quoteSummaryElement == null)
            {
                return null;
            }

            HtmlNode summaryColumn1 = quoteSummaryElement.ChildNodes.First();
            if (summaryColumn1 == null)
            {
                return null;
            }

            if (!summaryColumn1.HasChildNodes)
            {
                return null;
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (HtmlNode childNode in summaryColumn1.ChildNodes)
            {
                if (childNode.ChildNodes.Count == 4)
                {
                    string key = childNode.ChildNodes[0].InnerText.Trim();
                    string value = childNode.ChildNodes[2].InnerText;
                    dict[key] = value;
                }
            }

            return dict;
        }
    }
}