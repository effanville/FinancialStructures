using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;
using System.Collections.Generic;

namespace SavingClasses
{
    /// <summary>
    /// Saves into a file only. For legacy reasons really.
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
