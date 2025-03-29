using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Persistence.Xml.V2;

public class XmlTrade :
    IComparable,
    IComparable<XmlTrade>,
    IEquatable<XmlTrade>,
    IXmlSerializable
{
    public TradeType TradeType { get; set; }

    public string Company { get; set; }

    public string Name { get; set; }

    public DateTime Day { get; set; }

    public decimal NumberShares { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TradeCosts { get; set; }

    public XmlTrade() { }

    public XmlTrade(SecurityTrade trade)
    {
        TradeType = trade.TradeType;
        Company = trade.Company;
        Name = trade.Name;
        Day = trade.Day;
        NumberShares = trade.NumberShares;
        UnitPrice = trade.UnitPrice;
        TradeCosts = trade.TradeCosts;
    }

    private const string XmlBaseElement = "SecurityTrade";

    private const string XmlTradeTypeAttribute = "TradeType";
    private const string XmlCompanyAttribute = "Company";
    private const string XmlNameAttribute = "Name";
    private const string XmlDayElement = "Day";
    private const string XmlNumSharesAttribute = "NumberShares";
    private const string XmlUnitPriceAttribute = "UnitPrice";
    private const string XmlTradeCostsAttribute = "TradeCosts";

    public XmlSchema GetSchema() => null;
    public void ReadXml(XmlReader reader)
    {
        // new shorter xml format
        _ = reader.MoveToContent();

        if (reader.Name == XmlBaseElement)
        {
            string tradeTypeString = reader.GetAttribute(XmlTradeTypeAttribute);
            _ = Enum.TryParse(typeof(TradeType), tradeTypeString, true, out object tradeType);
            TradeType = tradeType == null ? TradeType.Unknown : (TradeType)tradeType;

            Company = reader.GetAttribute(XmlCompanyAttribute);
            Name = reader.GetAttribute(XmlNameAttribute);
            string dayString = reader.GetAttribute(XmlDayElement);
            _ = DateTime.TryParse(dayString, out DateTime date);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            Day = date;

            string valueString = reader.GetAttribute(XmlNumSharesAttribute);
            _ = decimal.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value);
            NumberShares = value;

            valueString = reader.GetAttribute(XmlUnitPriceAttribute);
            _ = decimal.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
            UnitPrice = value;

            valueString = reader.GetAttribute(XmlTradeCostsAttribute);
            _ = decimal.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
            TradeCosts = value;

            _ = reader.MoveToElement();
            reader.ReadStartElement();
        }
    }

    //<SecurityTrade TradeType="Buy" Company="Vanguard" Name="US Equity Acc" Day="2021-11-19T08:00:00" NumberShares="0.19250" UnitPrice="779.36" TradeCosts="0.0" />
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement(XmlBaseElement);
        writer.WriteAttributeString(XmlTradeTypeAttribute, TradeType.ToString());
        writer.WriteAttributeString(XmlCompanyAttribute, Company);
        writer.WriteAttributeString(XmlNameAttribute, Name);
        writer.WriteAttributeString(XmlDayElement, Day.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"));
        writer.WriteAttributeString(XmlNumSharesAttribute, NumberShares.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString(XmlUnitPriceAttribute, UnitPrice.ToString(CultureInfo.InvariantCulture));
        writer.WriteAttributeString(XmlTradeCostsAttribute, TradeCosts.ToString(CultureInfo.InvariantCulture));
        writer.WriteEndElement();
    }

    public SecurityTrade ToTrade()
        => new SecurityTrade(
            TradeType,
            new TwoName(Company, Name),
            Day,
            NumberShares,
            UnitPrice,
            TradeCosts);

    /// <inheritdoc/>
    public int CompareTo(XmlTrade other)
    {
        int dateComparison = DateTime.Compare(Day, other.Day);
        if (dateComparison == 0)
        {
            return TradeType.CompareTo(other.TradeType);
        }

        return dateComparison;
    }

    /// <summary>
    /// Method of comparison. Compares dates.
    /// </summary>
    public virtual int CompareTo(object obj)
    {
        if (obj is XmlTrade val)
        {
            return CompareTo(val);
        }

        return 0;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is XmlTrade other)
        {
            return Equals(other);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(XmlTrade other)
    {
        return TradeType.Equals(other.TradeType)
            && string.Equals(Company, other.Company)
            && string.Equals(Name, other.Name)
            && Day.Equals(other.Day)
            && NumberShares.Equals(other.NumberShares)
            && UnitPrice.Equals(other.UnitPrice)
            && TradeCosts.Equals(other.TradeCosts);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 17;
        hashCode = 23 * hashCode + TradeType.GetHashCode();
        hashCode = 23 * hashCode + Company.GetHashCode();
        hashCode = 23 * hashCode + Name.GetHashCode();
        hashCode = 23 * hashCode + Day.GetHashCode();
        hashCode = 23 * hashCode + NumberShares.GetHashCode();
        hashCode = 23 * hashCode + UnitPrice.GetHashCode();
        hashCode = 23 * hashCode + TradeCosts.GetHashCode();
        return hashCode;
    }
}