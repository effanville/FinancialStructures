using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Effanville.FinancialStructures.NamingStructures
{
    /// <summary>
    /// Any name data associated to an instrument.
    /// </summary>
    public class NameData : TwoName, IEquatable<NameData>
    {
        /// <summary>
        /// The ticker associated to this instrument
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string Ticker { get; set; }

        /// <summary>
        /// The ric to identify the instrument.
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string Ric { get; set; }

        /// <summary>
        /// LSE based code detailing the instrument
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string Sedol { get; set; }

        /// <summary>
        /// The isin for this instrument.
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string Isin { get; set; }

        /// <summary>
        /// The exchange this instrument is listed on.
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string Exchange { get; set; }

        /// <summary>
        /// Website associated to account.
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string Url { get; set; }

        /// <summary>
        /// Any currency name.
        /// </summary>
        [XmlElement(IsNullable = false)]
        public string Currency { get; set; }

        /// <summary>
        /// Sectors associated to account.
        /// </summary>
        public HashSet<string> Sectors { get; set; }

        /// <summary>
        /// Input of sector values from a string, with comma as separator.
        /// </summary>
        [XmlIgnore]
        public string SectorsFlat
        {
            get => FlattenSectors(Sectors);
            set
            {
                HashSet<string> sectorList = new HashSet<string>();
                if (!string.IsNullOrEmpty(value))
                {
                    string[] sectorsSplit = value.Split(',');

                    for (int i = 0; i < sectorsSplit.Length; i++)
                    {
                        sectorsSplit[i] = sectorsSplit[i].Trim(' ');
                    }

                    sectorList.UnionWith(sectorsSplit);
                }

                Sectors = sectorList;
            }
        }

        /// <summary>
        /// Converts a list of sectors into a string.
        /// </summary>
        public static string FlattenSectors(HashSet<string> sectorsSet) => sectorsSet != null ? string.Join(",", sectorsSet) : null;

        /// <summary>
        /// Any extra notes to add to the NameData.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public NameData()
            : base()
        {
            Sectors = new HashSet<string>();
        }

        /// <summary>
        /// Set all name type values.
        /// </summary>
        public NameData(string company, string name, string currency = null, string url = null, HashSet<string> sectors = null, string notes = null)
            : base(company, name)
        {
            Currency = currency;
            Url = url;
            Sectors = sectors ?? new HashSet<string>();
            Notes = notes;
        }

        /// <summary>
        /// Takes a copy of the data.
        /// </summary>
        public NameData Copy() => new NameData(Company, Name, Currency, Url, Sectors, Notes);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is NameData otherName)
            {
                return Equals(otherName);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(NameData otherName)
        {
            if (otherName == null)
            {
                return false;
            }
            bool currenciesEqual = Currency?.Equals(otherName.Currency) ?? otherName.Currency == null;
            bool urlEqual = Url?.Equals(otherName.Url) ?? otherName.Url == null;
            bool sectorsEqual = SectorsFlat?.Equals(otherName.SectorsFlat) ?? otherName.SectorsFlat == null;
            bool sedolEqual = Sedol?.Equals(otherName.Sedol) ?? otherName.Sedol == null;
            bool isinEqual = Isin?.Equals(otherName.Isin) ?? otherName.Isin == null;
            bool ricEqual = Ric?.Equals(otherName.Ric) ?? otherName.Ric == null;
            bool tickerEqual = Ticker?.Equals(otherName.Ticker) ?? otherName.Ticker == null;
            bool exchangeEqual = Exchange?.Equals(otherName.Exchange) ?? otherName.Exchange == null;
            bool notesEqual = Notes?.Equals(otherName.Notes) ?? otherName.Notes == null;
            return currenciesEqual
                && urlEqual
                && sectorsEqual
                && sedolEqual
                && isinEqual
                && ricEqual
                && tickerEqual
                && exchangeEqual
                && notesEqual
                && base.Equals(otherName);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Currency?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Url?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + SectorsFlat?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Ric?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Sedol?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Isin?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Ticker?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Exchange?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Notes?.GetHashCode() ?? 17;
            return 23 * hashCode + base.GetHashCode();
        }
    }
}
