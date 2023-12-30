using System;
using System.Collections.Generic;
using System.Linq;

using Nager.Date;

namespace FinancialStructures.Stocks.HistoricalRepository
{
    /// <summary>
    /// Simulates a stock exchange.
    /// </summary>
    public class HistoricalExchange
    {
        public string ExchangeIdentifier { get; init; }
        public string Name { get; init; }
        public TimeZoneInfo TimeZone { get; init; }
        public CountryCode CountryDateCode { get; init; }
        public TimeOnly ExchangeOpen { get; init; }
        public TimeOnly ExchangeClose { get; init; }
        public List<HistoricalStock> Stocks { get; } = new ();

        public static HistoricalExchange FromString(string line)
        {
            string[] inputs = line.Split(',');
            if (inputs.Length != 6)
            {
                return null;
            }

            if (!Enum.TryParse<CountryCode>(inputs[3].Trim(), out var code))
            {
                code = CountryCode.GB;
            }

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(inputs[2].Trim());
            var openTime = TimeOnly.Parse(inputs[4].Trim());
            var exchangeClose = TimeOnly.Parse(inputs[5].Trim());
            HistoricalExchange exchange = new()
            {
                ExchangeIdentifier = inputs[0].Trim(),
                Name = inputs[1].Trim(),
                TimeZone = timeZone,
                CountryDateCode = code,
                ExchangeOpen = openTime,
                ExchangeClose = exchangeClose
            };

            return exchange;
        }

        public bool TryGetStock(string ric, string name, out HistoricalStock stock)
        {
            foreach (HistoricalStock maybeStock in Stocks)
            {
                var latestNameData = maybeStock.Name.LastOrDefault();
                if (latestNameData.Value == null)
                {
                    continue;
                }

                if (!string.Equals(ric, latestNameData.Value.Ric) || !string.Equals(name, latestNameData.Value.Name))
                {
                    continue;
                }

                stock = maybeStock;
                return true;
            }

            stock = null;
            return false;
        }

        public override string ToString() => $"[HistExch: {ExchangeIdentifier}-{Name}]";
    }
}