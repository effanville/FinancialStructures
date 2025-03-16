using System.Collections.Generic;
using System.Xml.Serialization;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation;

namespace Effanville.FinancialStructures.Persistence.Xml
{
    /// <summary>
    /// Saves into a file only. Used to ensure compatibility with legacy saved files.
    /// </summary>
    public class AllData
    {
        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }

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
            Version = "1.0.0.0";
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
