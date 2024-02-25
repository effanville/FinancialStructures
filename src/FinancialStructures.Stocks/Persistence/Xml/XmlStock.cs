using System.Collections.Generic;
using System.Xml.Serialization;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Stocks.Persistence.Xml
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