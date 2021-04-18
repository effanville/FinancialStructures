using System;
using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Any name data associated to an account.
    /// </summary>
    public class NameData : TwoName, IEquatable<NameData>
    {
        private string fUrl;
        private string fCurrency;
        private HashSet<string> fSectors = new HashSet<string>();

        /// <summary>
        /// Website associated to account.
        /// </summary>
        public string Url
        {
            get
            {
                return fUrl;
            }
            set
            {
                fUrl = value;
            }
        }

        /// <summary>
        /// Any currency name.
        /// </summary>
        public string Currency
        {
            get
            {
                return fCurrency;
            }
            set
            {
                fCurrency = value;
            }
        }

        /// <summary>
        /// Sectors associated to account.
        /// </summary>
        public HashSet<string> Sectors
        {
            get
            {
                return fSectors;
            }
            set
            {
                fSectors = value;
            }
        }

        /// <summary>
        /// Input of sector values from a string, with comma as separator.
        /// </summary>
        public string SectorsFlat
        {
            get
            {
                return fSectors != null ? string.Join(",", fSectors) : null;
            }
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
        }

        /// <summary>
        /// Set all name type values.
        /// </summary>
        public NameData(string company, string name, string currency = null, string url = null, HashSet<string> sectors = null, string notes = null)
            : base(company, name)
        {
            Currency = currency;
            Url = url;
            Sectors = sectors;
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
            if (string.IsNullOrEmpty(Currency) && string.IsNullOrEmpty(Url) && string.IsNullOrEmpty(SectorsFlat))
            {
                if (string.IsNullOrEmpty(otherName.Currency) && string.IsNullOrEmpty(otherName.Url) && string.IsNullOrEmpty(otherName.SectorsFlat))
                {

                    return base.Equals(otherName);
                }

                return false;
            }

            if (string.IsNullOrEmpty(Url) && string.IsNullOrEmpty(SectorsFlat))
            {
                if (string.IsNullOrEmpty(otherName.Url) && string.IsNullOrEmpty(otherName.SectorsFlat))
                {
                    if (Currency.Equals(otherName.Currency))
                    {
                        return base.Equals(otherName);
                    }
                }
                return false;
            }
            if (string.IsNullOrEmpty(Currency) && string.IsNullOrEmpty(SectorsFlat))
            {
                if (string.IsNullOrEmpty(otherName.Currency) && string.IsNullOrEmpty(otherName.SectorsFlat))
                {
                    if (Url.Equals(otherName.Url))
                    {
                        return base.Equals(otherName);
                    }
                }
                return false;
            }
            if (string.IsNullOrEmpty(Currency) && string.IsNullOrEmpty(Url))
            {
                if (string.IsNullOrEmpty(otherName.Currency) && string.IsNullOrEmpty(otherName.Url))
                {
                    if (SectorsFlat.Equals(otherName.SectorsFlat))
                    {
                        return base.Equals(otherName);
                    }
                }
                return false;
            }

            if (string.IsNullOrEmpty(Currency))
            {
                if (string.IsNullOrEmpty(otherName.Currency))
                {
                    if (Url.Equals(otherName.Url) && SectorsFlat.Equals(otherName.SectorsFlat))
                    {
                        return base.Equals(otherName);
                    }
                }

                return false;
            }
            if (string.IsNullOrEmpty(Url))
            {
                if (string.IsNullOrEmpty(otherName.Url))
                {
                    if (Currency.Equals(otherName.Currency) && SectorsFlat.Equals(otherName.SectorsFlat))
                    {
                        return base.Equals(otherName);
                    }
                }

                return false;
            }
            if (string.IsNullOrEmpty(SectorsFlat))
            {
                if (string.IsNullOrEmpty(otherName.SectorsFlat))
                {
                    if (Currency.Equals(otherName.Currency) && Url.Equals(otherName.Url))
                    {
                        return base.Equals(otherName);
                    }
                }

                return false;
            }
            if (Currency.Equals(otherName.Currency) && Url.Equals(otherName.Url) && SectorsFlat.Equals(otherName.SectorsFlat))
            {
                return base.Equals(otherName);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Currency?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Url?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Sectors?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Notes?.GetHashCode() ?? 17
            return 23 * hashCode + base.GetHashCode();
        }
    }
}
