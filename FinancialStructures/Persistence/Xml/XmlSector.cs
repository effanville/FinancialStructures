using System.Xml.Serialization;

using Common.Structure.DataStructures;

using FinancialStructures.NamingStructures;

namespace FinancialStructures.Persistence.Xml
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