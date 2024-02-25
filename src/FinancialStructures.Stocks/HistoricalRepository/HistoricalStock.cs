using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.NamingStructures;
using FinancialStructures.Stocks.Implementation;

namespace FinancialStructures.Stocks.HistoricalRepository
{
    /// <summary>
    /// Simulates a Stock, and records all data with a timestamp to view evolutions.
    /// </summary>
    public sealed class HistoricalStock
    {
        private List<StockDay> _valuations = new();
        public int StockId { get; set; }
        public SortedList<DateTime, NameData> Name { get; set; } = new();

        public SortedList<DateTime, StockFundamentalData> Fundamentals { get; } = new();

        public List<StockDay> Valuations
        {
            get => _valuations;
            set
            {
                _valuations = value;
                _valuations.Sort();
            }
        }

        public static HistoricalStock FromString(DateTime timeStamp, string line, char separator)
        {
            NameData name = separator switch
            {
                ' ' => GetSpaceSepData(line),
                '\t' => GetTsvName(line),
                ',' => GetData(line),
                _ => null
            };
            if (name == null)
            {
                return null;
            }

            var stock = new HistoricalStock();

            stock.Name.Add(timeStamp, name);
            return stock;
        }
        
        public bool UpdateName(DateTime timeStamp, string line, char separator)
        {            
            NameData name = separator switch
            {
                ' ' => GetSpaceSepData(line),
                '\t' => GetTsvName(line),
                ',' => GetData(line),
                _ => null
            };
            if (name == null)
            {
                return false;
            }

            if (name.Equals(Name.Last().Value))
            {
                return false;
            }

            Name.Add(timeStamp, name);
            return true;
        }

        private static NameData GetData(string csvString)
        {
            string[] inputs = csvString.Split(',');
            if (inputs.Length != 7)
            {
                return null;
            }

            var name = new NameData
            {
                Ric = inputs[0],
                Company = inputs[1],
                Name = inputs[2],
                Currency = inputs[3],
                Url = inputs[5],
                SectorsFlat = inputs[6],
                Exchange = "LSE",
            };
            return name;
        }

        private static NameData GetTsvName(string tsvString)
        {
            string[] inputs = tsvString.Split("\t");
            if (inputs.Length != 7)
            {
                return null;
            }

            string ric = $"{inputs[0].Trim('.')}.L";
            string nameString = inputs[0].Replace(".L", "");
            var name = new NameData
            {
                Ric = ric,
                Company = inputs[1],
                Name = nameString,
                Currency = inputs[2],
                Exchange = "LSE",
                Url = $"https://uk.finance.yahoo.com/quote/{ric}",
                SectorsFlat = ""
            };

            return name;
        }        
        private static NameData GetSpaceSepData(string spaceSepString)
        {
            string[] inputs = spaceSepString.Split(" ");
            if (inputs.Length < 7)
            {
                return null;
            }

            string ric = $"{inputs[0].Trim('.').Replace('.', '-')}.L";
            string[] nameArray = inputs.Skip(1).SkipLast(5).ToArray();
            string nameString = string.Join(" ", nameArray);
            var name = new NameData
            {
                Ric = ric,
                Company = inputs[0],
                Name = nameString,
                Currency = inputs[^5],
                Exchange = "LSE",
                Url = $"https://uk.finance.yahoo.com/quote/{ric}",
                SectorsFlat = ""
            };

            return name;
        }

        public DateTime EarliestValidity() => Name.Keys.FirstOrDefault();

        public NameData ValidName(DateTime snapshotTime)
        {
            KeyValuePair<DateTime, NameData> lastPrecedingSnapshot = new (DateTime.MinValue, null);
            foreach (var name in Name)
            {
                if (name.Key < snapshotTime)
                {
                    lastPrecedingSnapshot = name;
                }
            }

            return lastPrecedingSnapshot.Value;
        }
        
        public StockFundamentalData ValidFundamentals(DateTime snapshotTime)
        {
            KeyValuePair<DateTime, StockFundamentalData> lastPrecedingSnapshot = new (DateTime.MinValue, null);
            foreach (var kvp in Fundamentals)
            {
                if (kvp.Key < snapshotTime)
                {
                    lastPrecedingSnapshot = kvp;
                }
            }

            return lastPrecedingSnapshot.Value;
        }
        
        public override string ToString() => $"[HistStock: {Name.Last().Value.Ric}, Values: {Valuations.Count}]";
    }
}