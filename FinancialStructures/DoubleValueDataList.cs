using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures
{
    public class DoubleValueDataList : SingleValueDataList
    {
        private TimeList fSecondValues = new TimeList();

        public DoubleValueDataList()
            : base()
        {
        }

        public DoubleValueDataList(string company, string name)
           : base(company, name)
        {
        }
    }
}
