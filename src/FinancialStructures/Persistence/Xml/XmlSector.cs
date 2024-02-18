using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml
{
    [XmlType(TypeName="Sector")]
    public class XmlSector : XmlValueList
    {
        public XmlSector() { }

        public XmlSector(NameData names, TimeList values)
            : base(names, values)
        {
        }
    }
}