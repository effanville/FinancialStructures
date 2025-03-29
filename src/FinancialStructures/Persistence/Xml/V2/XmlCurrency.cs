using System.Xml.Serialization;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

[XmlType(TypeName = "Currency")]
public class XmlCurrency : XmlValueList
{
    public XmlCurrency() { }

    public XmlCurrency(NameData names, XmlTimeList values)
        : base(names, values)
    {
    }
}