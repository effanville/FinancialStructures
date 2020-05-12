using StructureCommon.Reporting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StructureCommon.WebAccess
{
    /// <summary>
    /// Provides methods for downloading html from a website.
    /// Hopefully works with websites that require cookies.
    /// </summary>
    public static class WebDownloader
    {
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Determines whether the string is well formed as a url.
        /// </summary>
        public static bool IsValidWebAddress(string address)
        {
            if (!Uri.TryCreate(address, UriKind.Absolute, out Uri uri) || null == uri)
            {
                return false;
            }

            return Uri.IsWellFormedUriString(address, UriKind.Absolute);
        }

        /// <summary>
        /// Downloads from url synchronously.
        /// </summary>
        public static string DownloadFromURL(string url, IReportLogger reportLogger = null)
        {
            return DownloadFromURLasync(url, reportLogger).Result;
        }

        /// <summary>
        /// downloads the data from url asynchronously.
        /// </summary>
        public static async Task<string> DownloadFromURLasync(string url, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            string newUrl = Uri.EscapeUriString(url);
            string output = string.Empty;
            if (WebDownloader.IsValidWebAddress(newUrl))
            {
                try
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage
                    {
                        RequestUri = new Uri(newUrl)
                    };
                    requestMessage.Headers.Add("Cookie", "A1S=d=AQABBBqjZl4CELBdwrYAVcmgp1PaEGx2xoQFEgABAQGPgF5eX_bPb2UB_iMAAAcIGqNmXmx2xoQ&S=AQAAAii_Jnul1r-aKGkxwIrap9c&j=GDPR");
                    requestMessage.Headers.Add("Cookie", "A1=d=AQABBBqjZl4CELBdwrYAVcmgp1PaEGx2xoQFEgABAQGPgF5eX_bPb2UB_iMAAAcIGqNmXmx2xoQ&S=AQAAAii_Jnul1r-aKGkxwIrap9c");
                    requestMessage.Headers.Add("Cookie", "A3=d=AQABBBqjZl4CELBdwrYAVcmgp1PaEGx2xoQFEgABAQGPgF5eX_bPb2UB_iMAAAcIGqNmXmx2xoQ&S=AQAAAii_Jnul1r-aKGkxwIrap9c");
                    requestMessage.Headers.Add("Cookie", "B=89hjmdhf6d8oq&b=3&s=bv");
                    requestMessage.Headers.Add("Cookie", "cmp=v=28&t=1586111544&j=1&o=106");
                    requestMessage.Headers.Add("Cookie", "EuConsent=BOw-KL2OxZWozAOABCENC7uAAAAtl6__f_97_8_v2ddvduz_Ov_j_c__3XW8fPZvcELzhK9Meu_2xzd4u9wNRM5wckx87eJrEso5czISsG-RMod_zt__3ziX9oxPowEc9rz3nbEw6vs2v-ZzBCGJ_Iw");
                    requestMessage.Headers.Add("Cookie", "GUC=AQABAQFegI9fXkIe8wSV");
                    requestMessage.Headers.Add("Cookie", "PRF=t%3D2800.HK%252BVUKE.L%252BIGLS.L%252BSAAA.L%252BVWRL.L");
                    HttpResponseMessage response = await client.SendAsync(requestMessage).ConfigureAwait(false);
                    _ = response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (result != null)
                    {
                        output = result;
                    }
                }
                catch (Exception ex)
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Downloading, $"Failed to download from url {url}. Reason : {ex.Message}");
                    return output;
                }
            }

            return output;
        }
    }
}
