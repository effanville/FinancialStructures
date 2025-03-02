using System.Text.Json.Serialization;
using Effanville.Common.Structure.NamingStructures;

namespace Effanville.FinancialStructures.Download.Implementation
{
    public class YahooStockData
    {
        public Spark spark { get; set; }
    }

    public class Spark
    {
        public Result[] result { get; set; }
        public object error { get; set; }
    }

    public class Result
    {
        public string symbol { get; set; }
        public Response[] response { get; set; }
    }

    public class Response
    {
        public Meta meta { get; set; }
        public int[] timestamp { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("regularMarketPrice")]
        public double RegularMarketPrice { get; set; }
    }
}