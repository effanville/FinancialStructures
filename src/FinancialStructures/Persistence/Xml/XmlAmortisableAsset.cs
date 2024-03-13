using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml
{
    [XmlType(TypeName = "AmortisableAsset")]
    public class XmlAmortisableAsset : XmlValueList
    {
        public TimeList Debt { get; set; } = new TimeList();

        public TimeList Payments { get; set; } = new TimeList();

        public XmlAmortisableAsset() { }

        public XmlAmortisableAsset(NameData names, TimeList values, TimeList debt, TimeList payments)
            : base(names, values)
        {
            Debt = debt;
            Payments = payments;
        }
    }
}