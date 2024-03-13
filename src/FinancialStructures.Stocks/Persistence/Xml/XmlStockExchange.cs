using System.Collections.Generic;
using System.Xml.Serialization;

namespace Effanville.FinancialStructures.Stocks.Persistence.Xml
{
    /// <summary>
    /// Provides the class for serializing the stock exchange into Xml.
    /// </summary>
    [XmlType(TypeName = "StockExchange")]
    public class XmlStockExchange
    {
        public string Name
        {
            get;
            set;
        }

        public string TimeZone
        {
            get;
            set;
        } = "GMT Standard Time";

        public string CountryCode
        {
            get;
            set;
        } = "GB";

        public List<XmlStock> Stocks
        {
            get;
            set;
        }
    }
}