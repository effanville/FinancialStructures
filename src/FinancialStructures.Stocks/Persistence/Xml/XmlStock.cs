using System.Collections.Generic;
using System.Xml.Serialization;

using FinancialStructures.NamingStructures;

namespace FinancialStructures.Stocks.Persistence.Xml
{
    [XmlType(TypeName = "Stock")]
    public class XmlStock
    {
        public NameData Name
        {
            get;
            set;
        }

        public List<XmlStockCandle> Valuations
        {
            get;
            set;
        }
    }
}