using System;
using System.Globalization;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.DataStructures
{
    /// <summary>
    /// Contains all information about a Stock trade.
    /// </summary>
    public class SecurityTrade :
        IComparable,
        IComparable<SecurityTrade>,
        IEquatable<SecurityTrade>,
        IXmlSerializable
    {
        /// <summary>
        /// The type of this trade.
        /// </summary>
        public TradeType TradeType { get; set; }

        /// <summary>
        /// The company name associated to this trade.
        /// </summary>
        public string Company
        {
            get => Names.Company;
            set => Names.Company = value;
        }

        /// <summary>
        /// The secondary name of the security associated to this trade.
        /// </summary>
        public string Name
        {
            get => Names.Name;
            set => Names.Name = value;
        }

        /// <summary>
        /// The names associated to this trade.
        /// </summary>
        public TwoName Names { get; set; }

        /// <summary>
        /// The day this trade took place on.
        /// </summary>
        public DateTime Day { get; set; }

        /// <summary>
        /// The total cost of this trade.
        /// </summary>
        public decimal TotalCost
        {
            get
            {
                decimal sign = TradeType.Sign();
                bool isInvestmentAltering = TradeType.IsInvestmentTradeType();
                return isInvestmentAltering ? NumberShares * UnitPrice + sign * TradeCosts : 0.0m;
            }
        }

        /// <summary>
        /// The number of shares this trade deals with.
        /// <para/>
        /// For Buy or sell this is a positive value. A dividend value is signed.
        /// </summary>
        public decimal NumberShares { get; set; }

        /// <summary>
        /// The price of the underlying that this trade was enacted at.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// The cost of performing this trade. Encompasses all fixed costs and
        /// percentage costs.
        /// </summary>
        public decimal TradeCosts { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public SecurityTrade()
        {
            Names = new TwoName();
        }

        /// <summary>
        /// Construct an instance with only a <see cref="TradeType"/>
        /// </summary>
        public SecurityTrade(TradeType type)
            : this()
        {
            TradeType = type;
        }

        /// <summary>
        /// Create an instance filling in all data.
        /// </summary>
        public SecurityTrade(TradeType type, TwoName names, DateTime day, decimal numShares, decimal price, decimal costs)
        {
            TradeType = type;
            Names = names ?? new TwoName();
            Day = day;
            NumberShares = numShares;
            UnitPrice = price;
            TradeCosts = costs;
        }

        internal SecurityTrade(DateTime day)
        {
            Day = day;
        }

        /// <summary>
        /// Provides a copy of this <see cref="SecurityTrade"/>.
        /// </summary>
        public SecurityTrade Copy()
        {
            return new SecurityTrade(TradeType, Names, Day, NumberShares, UnitPrice, TradeCosts);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Day.ToUkDateString() + "-" + TradeType.ToString() + "-" + Names.Company + "-" + Names.Name + "-" + TotalCost + "-" + NumberShares + "-" + UnitPrice + "-" + TradeCosts;
        }

        /// <inheritdoc/>
        public int CompareTo(SecurityTrade other)
        {
            return DateTime.Compare(Day, other.Day);
        }

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            if (obj is SecurityTrade val)
            {
                return CompareTo(val);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is SecurityTrade other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(SecurityTrade other)
        {
            return TradeType.Equals(other.TradeType)
                && Names.Equals(other.Names)
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
            hashCode = 23 * hashCode + Names.GetHashCode();
            hashCode = 23 * hashCode + Day.GetHashCode();
            hashCode = 23 * hashCode + NumberShares.GetHashCode();
            hashCode = 23 * hashCode + UnitPrice.GetHashCode();
            hashCode = 23 * hashCode + TradeCosts.GetHashCode();
            return hashCode;
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
                _ = DateTimeOffset.TryParse(dayString, out DateTimeOffset dateTimeOffset);
                Day = DateTime.SpecifyKind(dateTimeOffset.DateTime, DateTimeKind.Utc);

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
            writer.WriteAttributeString(XmlDayElement, Day.ToString("s"));
            writer.WriteAttributeString(XmlNumSharesAttribute, NumberShares.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(XmlUnitPriceAttribute, UnitPrice.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(XmlTradeCostsAttribute, TradeCosts.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }
    }
}
