using System;
using System.Collections.Generic;

namespace FinancialStructures.Stocks.Persistence.Database.Models
{
    public class Exchange
    {
        public int Id { get; set; }
        public string ExchangeIdentifier { get; set; }
        public string Name { get; set; }
        public string TimeZone { get; set; }
        public string CountryCode { get; set; }
        public TimeOnly ExchangeOpen { get; set; }
        public TimeOnly ExchangeClose { get; set; }
        
        public List<Instrument> Instruments { get; set; }
        
        public override string ToString() => $"[Exchange:{Id}-{ExchangeIdentifier}-{Name}";
    }
}