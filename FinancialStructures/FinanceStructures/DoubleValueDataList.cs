using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    public class DoubleValueDataList : SingleValueDataList
    {
        private TimeList fSecondValues = new TimeList();

        public DoubleValueDataList()
            : base()
        {
        }

        public DoubleValueDataList(string company, string name)
           : base(new NameData(company, name))
        {
        }
    }
}
