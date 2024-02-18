using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml
{
    public class XmlValueList
    {
        public NameData Names { get; set; }
        public TimeList Values { get; set; }
        
        public XmlValueList() { }

        public XmlValueList(NameData names, TimeList values)
        {
            Names = names;
            Values = values;
        }
    }
}