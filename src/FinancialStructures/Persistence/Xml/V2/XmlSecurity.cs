using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

public class XmlSecurity : IXmlSerializable
{
    public NameData Names { get; set; }
    public XmlTimeList UnitPrice { get; set; }
    public List<XmlTrade> SecurityTrades { get; set; }

    public XmlSecurity()
    {
        UnitPrice = new XmlTimeList();
        SecurityTrades = new List<XmlTrade>();
    }

    public XmlSecurity(NameData names, XmlTimeList unitPrice, List<XmlTrade> securityTrades)
    {
        Names = names;
        UnitPrice = unitPrice;
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

        reader.ReadStartElement();

        XmlSerializer serializer = new XmlSerializer(typeof(NameData));
        Names = (NameData)serializer.Deserialize(reader);

        UnitPrice.ReadXml(reader);

        if (reader.LocalName == XmlTradeBaseName)
        {
            reader.ReadStartElement(XmlTradeBaseName);
        }

        if (!isEmpty)
        {
            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                XmlTrade valuation = new XmlTrade();
                valuation.ReadXml(reader);
                SecurityTrades.Add(valuation);
                _ = reader.MoveToContent();
            }
            if (reader.NodeType != XmlNodeType.None)
            {
                reader.ReadEndElement();
            }
        }

        reader.ReadEndElement();
    }

    /// <inheritdoc/>
    public virtual void WriteXml(XmlWriter writer)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(NameData));
        serializer.Serialize(writer, Names);

        writer.WriteStartElement("UnitPrice");
        UnitPrice.WriteXml(writer);
        writer.WriteEndElement();
        writer.WriteStartElement(XmlTradeBaseName);
        foreach (XmlTrade value in SecurityTrades)
        {
            value.WriteXml(writer);
        }
        writer.WriteEndElement();
    }
}