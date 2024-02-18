using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;

using FinancialStructures.NamingStructures;

namespace FinancialStructures.Persistence.Xml
{
    [XmlType(TypeName="Currency")]
    public class XmlCurrency : XmlValueList
    {
        public XmlCurrency() { }

        public XmlCurrency(NameData names, TimeList values)
            : base(names, values)
        {
        }
    }
}