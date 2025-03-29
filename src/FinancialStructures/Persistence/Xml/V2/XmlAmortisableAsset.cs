using System.Xml.Serialization;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

[XmlType(TypeName = "AmortisableAsset")]
public class XmlAmortisableAsset : XmlValueList
{
    public XmlTimeList Debt { get; set; }

    public XmlTimeList Payments { get; set; }

    public XmlAmortisableAsset()
    {
        Debt = new XmlTimeList();
        Payments = new XmlTimeList();
    }

    public XmlAmortisableAsset(NameData names, XmlTimeList values, XmlTimeList debt, XmlTimeList payments)
        : base(names, values)
    {
        Debt = debt;
        Payments = payments;
    }
}