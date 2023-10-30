using System.Collections.Generic;
using System.Xml.Serialization;

using Common.Structure.DataStructures;

using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Persistence.Xml
{
    [XmlType(TypeName="Security")]
    public class XmlSecurity
    {
        public NameData Names { get; set; }
        public TimeList Shares { get; set; } = new TimeList();
        public TimeList UnitPrice { get; set; } = new TimeList();
        public TimeList Investments { get; set; } = new TimeList();
        public List<SecurityTrade> SecurityTrades { get; set; } = new List<SecurityTrade>();
        
        public XmlSecurity() { }

        public XmlSecurity(NameData names, TimeList unitPrice, TimeList shares, TimeList investments, List<SecurityTrade> securityTrades)
        {
            Names = names;
            UnitPrice = unitPrice;
            Shares = shares;
            Investments = investments;
            SecurityTrades = securityTrades;
        }
    }
}