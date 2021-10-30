using System;
using System.Xml.Serialization;
using Common.Structure.Extensions;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Contains all information about a Stock trade.
    /// </summary>
    public class SecurityTrade : IComparable, IComparable<SecurityTrade>, IEquatable<SecurityTrade>
    {
        /// <summary>
        /// The type of this trade.
        /// </summary>
        [XmlAttribute]
        public TradeType TradeType
        {
            get;
            set;
        }

        /// <summary>
        /// The company name associated to this trade.
        /// </summary>
        [XmlAttribute]
        public string Company
        {
            get => Names.Company;
            set => Names.Company = value;
        }

        /// <summary>
        /// The secondary name of the security associated to this trade.
        /// </summary>
        [XmlAttribute]
        public string Name
        {
            get => Names.Name;
            set => Names.Name = value;
        }

        /// <summary>
        /// The names associated to this trade.
        /// </summary>
        [XmlIgnore]
        public TwoName Names
        {
            get;
            set;
        }

        /// <summary>
        /// The day this trade took place on.
        /// </summary>
        [XmlAttribute]
        public DateTime Day
        {
            get;
            set;
        }

        /// <summary>
        /// The total cost of this trade.
        /// </summary>
        public double TotalCost
        {
            get
            {
                if (TradeType == TradeType.Sell)
                {
                    return NumberShares * UnitPrice - TradeCosts;
                }

                return NumberShares * UnitPrice + TradeCosts;
            }
        }

        /// <summary>
        /// The number of shares this trade deals with.
        /// <para/>
        /// For Buy or sell this is a positive value. A dividend value is signed.
        /// </summary>
        [XmlAttribute]
        public double NumberShares
        {
            get;
            set;
        }

        /// <summary>
        /// The price of the underlying that this trade was enacted at.
        /// </summary>
        [XmlAttribute]
        public double UnitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// The cost of performing this trade. Encompasses all fixed costs and
        /// percentage costs.
        /// </summary>
        [XmlAttribute]
        public double TradeCosts
        {
            get;
            set;
        }

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
        {
            TradeType = type;
        }

        /// <summary>
        /// Create an instance filling in all data.
        /// </summary>
        public SecurityTrade(TradeType type, TwoName names, DateTime day, double numShares, double price, double costs)
        {
            TradeType = type;
            Names = names;
            Day = day;
            NumberShares = numShares;
            UnitPrice = price;
            TradeCosts = costs;
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
    }
}
