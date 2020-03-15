using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using System.Collections.Generic;

namespace SavingClasses
{
    public class AllData
    {
        public Portfolio MyFunds { get; set; } = new Portfolio();

        public List<Sector> myBenchMarks { get; set; } = new List<Sector>();

        public AllData()
        {
        }

        public AllData(Portfolio portfo, List<Sector> fSectors)
        {
            MyFunds = portfo;
            myBenchMarks = fSectors;
        }
    }
}
