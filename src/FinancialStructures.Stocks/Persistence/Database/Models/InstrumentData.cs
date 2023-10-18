using System;

namespace FinancialStructures.Stocks.Persistence.Models
{
    public class InstrumentData
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public DateTime SnapshotTime { get; set; }
        public string Index { get; set; }
        public double PERatio { get; set; }
        public double EPS { get; set; }
        public double Beta_5YMonth { get; set; }
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
            PERatio = instrumentData.PERatio;
            EPS = instrumentData.EPS;
            Beta_5YMonth = instrumentData.Beta_5YMonth;
            AverageVolume = instrumentData.AverageVolume;
            ForwardDividend = instrumentData.ForwardDividend;
            ForwardYield = instrumentData.ForwardYield;
            MarketCap = instrumentData.MarketCap;
        }
    }
}