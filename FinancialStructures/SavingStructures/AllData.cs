using System.Collections.Generic;
using FinancialStructures.Database.Implementation;
using FinancialStructures.FinanceStructures.Implementation;

namespace FinancialStructures.SavingClasses
{
    /// <summary>
    /// Saves into a file only. Used to ensure compatibility with legacy saved files.
    /// </summary>
    public class AllData
    {
        /// <summary>
        /// The portfolio data.
        /// </summary>
        public Portfolio MyFunds { get; set; } = new Portfolio();

        /// <summary>
        /// The Sector data.
        /// </summary>
        public List<Sector> myBenchMarks { get; set; } = new List<Sector>();

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public AllData()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AllData(Portfolio portfo, List<Sector> fSectors)
        {
            MyFunds.SetFrom(portfo);
            myBenchMarks = fSectors;
        }
    }
}
