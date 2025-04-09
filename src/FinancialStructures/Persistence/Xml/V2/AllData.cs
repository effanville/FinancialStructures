using System.Xml.Serialization;
using Effanville.FinancialStructures.Database.Implementation;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

/// <summary>
/// Saves into a file only. Used to ensure compatibility with legacy saved files.
/// </summary>
public class AllData
{
    [XmlElement(ElementName = "Version")]
    public string Version { get; set; } = "2.0.0.0";

    /// <summary>
    /// The portfolio data.
    /// </summary>
    public XmlPortfolio MyFunds { get; set; } = new XmlPortfolio();

    /// <summary>
    /// Empty constructor.
    /// </summary>
    public AllData() { }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public AllData(Portfolio portfolio)
    {
        MyFunds.SetFrom(portfolio);
    }
}
