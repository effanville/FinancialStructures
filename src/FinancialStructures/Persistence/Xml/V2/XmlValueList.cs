using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

public class XmlValueList
{
    public NameData Names { get; set; }
    public XmlTimeList Values { get; set; }

    public XmlValueList() { }

    public XmlValueList(NameData names, XmlTimeList values)
    {
        Names = names;
        Values = values;
    }
}