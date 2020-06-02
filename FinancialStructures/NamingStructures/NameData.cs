using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Any name data associated to an account.
    /// </summary>
    public class NameData : TwoName
    {
        public override bool Equals(object obj)
        {
            if (obj is NameData otherName)
            {
                if (string.IsNullOrEmpty(Currency) && string.IsNullOrEmpty(Url) && string.IsNullOrEmpty(SectorsFlat))
                {
                    return base.Equals(otherName);
                }

                if (string.IsNullOrEmpty(Url) && string.IsNullOrEmpty(SectorsFlat))
                {
                    if (Currency.Equals(otherName.Currency))
                    {
                        return base.Equals(otherName);
                    }

                    return false;
                }
                if (string.IsNullOrEmpty(Currency) && string.IsNullOrEmpty(SectorsFlat))
                {
                    if (Url.Equals(otherName.Url))
                    {
                        return base.Equals(otherName);
                    }

                    return false;
                }
                if (string.IsNullOrEmpty(Currency) && string.IsNullOrEmpty(Url))
                {
                    if (SectorsFlat.Equals(otherName.SectorsFlat))
                    {
                        return base.Equals(otherName);
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
            }

            return false;
        }

        public override int GetHashCode()
        {
            int currencyVal = Currency != null ? Currency.GetHashCode() : 0;
            int urlVal = Url != null ? Url.GetHashCode() : 0;
            int sectorVal = Sectors != null ? Sectors.GetHashCode() : 0;
            return currencyVal + urlVal + sectorVal + base.GetHashCode();
        }

        /// <summary>
        /// Takes a copy of the data.
        /// </summary>
        /// <returns></returns>
        public NameData Copy()
        {
            return new NameData(Company, Name, Currency, Url, Sectors);
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
        public NameData(string company, string name, string currency, string url, HashSet<string> sectors)
            : base(company, name)
        {
            Currency = currency;
            Url = url;
            Sectors = sectors;
        }

        /// <summary>
        /// Set data without sectors.
        /// </summary>
        public NameData(string company, string name, string currency, string url)
             : base(company, name)
        {
            Currency = currency;
            Url = url;
        }

        /// <summary>
        /// Set minimal naming
        /// </summary>
        public NameData(string company, string name)
             : base(company, name)
        {
        }

        private string fUrl;

        /// <summary>
        /// Website associated to account.
        /// </summary>
        public virtual string Url
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

        private string fCurrency;

        /// <summary>
        /// Any currency name.
        /// </summary>
        public virtual string Currency
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

        private HashSet<string> fSectors = new HashSet<string>();

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
        public virtual string SectorsFlat
        {
            get
            {
                return string.Join(",", fSectors);
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
    }
}
