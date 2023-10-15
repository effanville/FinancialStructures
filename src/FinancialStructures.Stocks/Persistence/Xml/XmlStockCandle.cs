using System;
using System.Xml.Serialization;

namespace FinancialStructures.Stocks.Persistence.Xml
{
    [XmlType(TypeName = "StockDay")]
    public class XmlStockCandle
    {
        [XmlAttribute(AttributeName = "T")]
        public DateTime Start
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "D")]
        public TimeSpan Duration
        {
            get;
            set;
        } = TimeSpan.FromHours(8.5);

        [XmlAttribute(AttributeName = "O")]
        public decimal Open
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "H")]
        public decimal High
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "L")]
        public decimal Low
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "C")]
        public decimal Close
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "V")]
        public decimal Volume
        {
            get;
            set;
        }
    }
}