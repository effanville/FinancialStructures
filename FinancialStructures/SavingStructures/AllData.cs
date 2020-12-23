using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.SavingClasses
{
    /// <summary>
    /// Saves into a file only. Used to ensure compatibility with legacy saved files.
    /// </summary>
    public class AllData
    {
        public Portfolio MyFunds { get; set; } = new Portfolio();

        public List<Sector> myBenchMarks { get; set; } = new List<Sector>();

        public AllData()
        {
        }

        public AllData(IPortfolio portfo, List<Sector> fSectors)
        {
            MyFunds.CopyData(portfo);
            myBenchMarks = fSectors;
        }
    }
}
