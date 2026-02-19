using System.Text.Json.Serialization;

namespace Effanville.FinancialStructures.Stocks.Download.Yahoo;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(YahooStockHistoryData))]
internal partial class YahooStockHistoryDataContext : JsonSerializerContext { }

public class YahooStockHistoryData
{
    public Chart chart { get; set; }
}