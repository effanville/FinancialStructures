using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using System;
using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

public class XmlValuation : IXmlSerializable
{
    public DateTime Day { get; set; }

    public decimal Value { get; set; }

    public XmlValuation() { }
    public XmlValuation(DailyValuation dv)
    {
        Day = dv.Day;
        Value = dv.Value;
    }

    /// <inheritdoc/>
    public XmlSchema GetSchema() => null;

    private const string XmlBaseElement = "DV";

    private const string XmlDayElement = "D";
    private const string XmlValueElement = "V";

    /// <inheritdoc/>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement(XmlBaseElement);
        writer.WriteAttributeString(XmlDayElement, Day.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"));
        writer.WriteAttributeString(XmlValueElement, Value.ToString(CultureInfo.InvariantCulture));
        writer.WriteEndElement();
    }

    /// <inheritdoc/>
    public void ReadXml(XmlReader reader)
    {
        // new shorter xml format
        _ = reader.MoveToContent();

        if (reader.Name == XmlBaseElement)
        {
            string dayString = reader.GetAttribute(XmlDayElement);
            string valueString = reader.GetAttribute(XmlValueElement);

            _ = DateTime.TryParse(dayString, out DateTime date);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            _ = decimal.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value);

            Day = date;
            Value = value;
            _ = reader.MoveToElement();
            reader.ReadStartElement();
        }
    }

    public DailyValuation ToDV() => new DailyValuation(Day, Value);
}