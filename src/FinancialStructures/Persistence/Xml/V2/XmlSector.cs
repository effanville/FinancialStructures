using System.Xml.Serialization;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

[XmlType(TypeName = "Sector")]
public class XmlSector : XmlValueList
{
    public XmlSector() { }

    public XmlSector(NameData names, XmlTimeList values)
        : base(names, values)
    {
    }
}