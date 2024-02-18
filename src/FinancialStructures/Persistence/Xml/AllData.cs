using System.Collections.Generic;

using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation;

namespace Effanville.FinancialStructures.Persistence.Xml
{
    /// <summary>
    /// Saves into a file only. Used to ensure compatibility with legacy saved files.
    /// </summary>
    public class AllData
    {
        /// <summary>
        /// The portfolio data.
        /// </summary>
        public XmlPortfolio MyFunds { get; set; } = new XmlPortfolio();

        /// <summary>
        /// The Sector data.
        /// </summary>
        public List<XmlSector> myBenchMarks { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public AllData()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AllData(Portfolio portfolio, List<Sector> fSectors)
        {
            MyFunds.SetFrom(portfolio);
            if (fSectors != null)
            {
                foreach (var benchmark in fSectors)
                {
                    myBenchMarks.Add(new XmlSector() { Names = benchmark.Names, Values = benchmark.Values });
                }
            }
        }
    }
}
