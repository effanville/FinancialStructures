using System.Text.Json.Serialization;

namespace Effanville.FinancialStructures.Download.Implementation
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(YahooStockData))]
    internal partial class YahooStockDataSourceGenerationContext : JsonSerializerContext
    {
    }
}