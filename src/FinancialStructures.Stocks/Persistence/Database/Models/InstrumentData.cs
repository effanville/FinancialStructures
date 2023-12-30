using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialStructures.Stocks.Persistence.Database.Models
{
    public class InstrumentData
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public DateTime ValidFrom { get; set; }
        public string Index { get; set; }
        public double PeRatio { get; set; }
        public double EPS { get; set; }
        public double Beta5YearMonth { get; set; }
        public double AverageVolume { get; set; }
        public double ForwardDividend { get; set; }
        public double ForwardYield { get; set; }
        public double MarketCap { get; set; }

        public Instrument Instrument { get; set; }

        public InstrumentData()
        { }

        public InstrumentData(InstrumentData instrumentData)
        {
            InstrumentId = instrumentData.InstrumentId;
            Index = instrumentData.Index;
            PeRatio = instrumentData.PeRatio;
            EPS = instrumentData.EPS;
            Beta5YearMonth = instrumentData.Beta5YearMonth;
            AverageVolume = instrumentData.AverageVolume;
            ForwardDividend = instrumentData.ForwardDividend;
            ForwardYield = instrumentData.ForwardYield;
            MarketCap = instrumentData.MarketCap;
        }
        
        public override string ToString() => $"[InstrumentData:{Id}-{InstrumentId}-{ValidFrom}";
    }
}