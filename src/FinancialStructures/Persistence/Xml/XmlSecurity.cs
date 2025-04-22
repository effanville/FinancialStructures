using System.Collections.Generic;
using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml
{
    [XmlType(TypeName = "Security")]
    public class XmlSecurity
    {
        public NameData Names { get; set; }
        public TimeList Shares { get; set; }
        public TimeList UnitPrice { get; set; }
        public TimeList Investments { get; set; }
        public List<V1.XmlTrade> SecurityTrades { get; set; }

        public XmlSecurity()
        {
            Names = new NameData();
            Shares = new TimeList();
            Investments = new TimeList();
            UnitPrice = new TimeList();
            SecurityTrades = new List<V1.XmlTrade>();
        }

        public XmlSecurity(NameData names, TimeList unitPrice, TimeList shares, TimeList investments, List<V1.XmlTrade> securityTrades)
        {
            Names = names;
            UnitPrice = unitPrice;
            Shares = shares;
            Investments = investments;
            SecurityTrades = securityTrades;
        }
    }
}