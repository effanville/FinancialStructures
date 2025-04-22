using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

public class XmlSecurity : IXmlSerializable
{
    public NameData Names { get; set; }
    public TimeList Shares { get; set; }
    public TimeList UnitPrice { get; set; }
    public TimeList Investments { get; set; }
    public List<SecurityTrade> SecurityTrades { get; set; }

    public XmlSecurity()
    {
        Names = new NameData();
        Shares = new TimeList();
        Investments = new TimeList();
        UnitPrice = new TimeList();
        SecurityTrades = new List<SecurityTrade>();
    }

    public XmlSecurity(NameData names, TimeList unitPrice, TimeList shares, TimeList investments, List<SecurityTrade> securityTrades)
    {
        Names = names;
        UnitPrice = unitPrice;
        Shares = shares;
        Investments = investments;
        SecurityTrades = securityTrades;
    }

    /// <inheritdoc/>
    public XmlSchema GetSchema() => null;
    private const string XmlBaseName = "Security";
    private const string XmlTradeBaseName = "SecurityTrades";

    /// <inheritdoc/>
    public virtual void ReadXml(XmlReader reader)
    {
        bool isEmpty = reader.IsEmptyElement;

        bool isInnerEmpty = false;
        reader.ReadStartElement();

        XmlSerializer serializer = new XmlSerializer(typeof(NameData), new XmlRootAttribute(nameof(Names)));
        Names = (NameData)serializer.Deserialize(reader);

        Shares.ReadXml(reader);
        UnitPrice.ReadXml(reader);
        Investments.ReadXml(reader);
        // if the timelist is part of a larger class, then the data is stored in an
        // extra node.
        bool partOfClass = reader.AttributeCount > 0 || reader.LocalName == XmlTradeBaseName;

        if (reader.LocalName == XmlTradeBaseName)
        {
            if (partOfClass)
            {
                isInnerEmpty = reader.IsEmptyElement;
                reader.ReadStartElement(XmlTradeBaseName);
            }
            if (!isEmpty)
            {
                while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
                {
                    SecurityTrade valuation = new SecurityTrade();
                    valuation.ReadXml(reader);
                    SecurityTrades.Add(valuation);
                    _ = reader.MoveToContent();
                }
                if (reader.NodeType != XmlNodeType.None)
                {
                    if (partOfClass && !isInnerEmpty)
                    {
                        reader.ReadEndElement();
                    }
                    reader.ReadEndElement();
                }
            }
        }
        else
        {
            throw new System.Exception("Missing Trade data");
        }
    }

    /// <inheritdoc/>
    public virtual void WriteXml(XmlWriter writer)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(NameData), new XmlRootAttribute(nameof(Names)));
        serializer.Serialize(writer, Names);

        writer.WriteStartElement("Shares");
        Shares.WriteXml(writer);
        writer.WriteEndElement();
        writer.WriteStartElement("UnitPrice");
        UnitPrice.WriteXml(writer);
        writer.WriteEndElement();

        writer.WriteStartElement("Investments");
        Investments.WriteXml(writer);
        writer.WriteEndElement();
        writer.WriteStartElement(XmlTradeBaseName);
        foreach (SecurityTrade value in SecurityTrades)
        {
            value.WriteXml(writer);
        }
        writer.WriteEndElement();
    }
}
