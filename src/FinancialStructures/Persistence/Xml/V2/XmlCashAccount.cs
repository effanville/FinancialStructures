using System.Xml.Serialization;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

[XmlType(TypeName = "CashAccount")]
public class XmlCashAccount : XmlValueList
{
    public XmlCashAccount() { }

    public XmlCashAccount(NameData names, XmlTimeList values)
        : base(names, values)
    {
    }
}