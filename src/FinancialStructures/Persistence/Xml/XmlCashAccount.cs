using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;

using FinancialStructures.NamingStructures;

namespace FinancialStructures.Persistence.Xml
{
    [XmlType(TypeName="CashAccount")]
    public class XmlCashAccount : XmlValueList
    {
        public XmlCashAccount() { }

        public XmlCashAccount(NameData names, TimeList values)
            : base(names, values)
        {
        }
    }
}