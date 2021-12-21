using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Any name data associated to an account.
    /// </summary>
    public class NameData : TwoName, IEquatable<NameData>
    {
        /// <summary>
        /// Website associated to account.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Any currency name.
        /// </summary>
        public string Currency
        {
            get;
            set;
        }

        /// <summary>
        /// Sectors associated to account.
        /// </summary>
        public HashSet<string> Sectors
        {
            get;
            set;
        }

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
        public static string FlattenSectors(HashSet<string> sectorsSet)
        {
            return sectorsSet != null ? string.Join(",", sectorsSet) : null;
        }

        /// <summary>
        /// Any extra notes to add to the NameData.
        /// </summary>
        public string Notes
        {
            get;
            set;
        }

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
        public NameData Copy()
        {
            return new NameData(Company, Name, Currency, Url, Sectors, Notes);
        }

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
            bool currenciesEqual = Currency?.Equals(otherName.Currency) ?? otherName.Currency == null;
            bool urlEqual = Url?.Equals(otherName.Url) ?? otherName.Url == null;
            bool sectorsEqual = SectorsFlat?.Equals(otherName.SectorsFlat) ?? otherName.SectorsFlat == null;
            bool notesEqual = Notes?.Equals(otherName.Notes) ?? otherName.Notes == null;
            return currenciesEqual && urlEqual && sectorsEqual && notesEqual && base.Equals(otherName);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Currency?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Url?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + SectorsFlat?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Notes?.GetHashCode() ?? 17;
            return 23 * hashCode + base.GetHashCode();
        }
    }
}
