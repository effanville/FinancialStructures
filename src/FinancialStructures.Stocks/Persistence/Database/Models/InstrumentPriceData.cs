using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialStructures.Stocks.Persistence.Database.Models
{
    public class InstrumentPriceData
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public int DataSourceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

        [ForeignKey("DataSourceId")] public DataSource DataSource { get; set; }
        public Instrument Instrument { get; set; }
        
        public override string ToString() => $"[InstrumentData:{Id}-{InstrumentId}-{StartTime}-{EndTime}-{Close}";
    }
}