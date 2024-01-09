using System;
using System.Collections.Generic;

namespace FinancialStructures.Stocks.Persistence.Database.Models
{
    public class Instrument
    {
        public int Id { get; set; }
        public int CoreInstrumentId { get; set; }
        public string Ric { get; set; }
        public string Sedol { get; set; }
        public string Isin { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public int ExchangeId { get; set; }
        public string Url { get; set; }
        public string Sectors { get; set; }
        public DateTime ValidFrom { get; set; }
        public Exchange Exchange { get; set; }
        
        public List<InstrumentPriceData> Prices { get; set; }
        public List<InstrumentData> FundamentalData { get; set; }
        
        public override string ToString() => $"[Instrument:{Id}-{CoreInstrumentId}-{Ric}-{Company}-{Name}";
    }
}