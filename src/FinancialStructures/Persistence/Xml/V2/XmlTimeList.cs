using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

public class XmlTimeList : IXmlSerializable
{
    private List<XmlValuation> _values;
    public XmlTimeList() => _values = new List<XmlValuation>();

    public XmlTimeList(TimeList timeList)
        => _values = timeList.Values().Select(x => new XmlValuation(x)).ToList();

    /// <inheritdoc/>
    public XmlSchema GetSchema() => null;

    private const string XmlBaseName = "Values";

    /// <inheritdoc/>
    public virtual void ReadXml(XmlReader reader)
    {
        bool isInnerEmpty = false;
        bool isEmpty = reader.IsEmptyElement;

        reader.ReadStartElement();

        // if the timelist is part of a larger class, then the data is stored in an
        // extra node.
        bool partOfClass = reader.AttributeCount > 0 || reader.LocalName == XmlBaseName;
        if (partOfClass)
        {
            isInnerEmpty = reader.IsEmptyElement;
            reader.ReadStartElement(XmlBaseName);
        }

        if (!isEmpty)
        {
            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                XmlValuation valuation = new XmlValuation();
                valuation.ReadXml(reader);
                _values.Add(valuation);
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

    /// <inheritdoc/>
    public virtual void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement(XmlBaseName);

        foreach (XmlValuation value in _values)
        {
            value.WriteXml(writer);
        }
        writer.WriteEndElement();
    }

    public TimeList ToTimeList() => new TimeList(_values.Select(x => x.ToDV()).ToList());
}