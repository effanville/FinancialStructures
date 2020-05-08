using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Any name data associated to an account.
    /// </summary>
    public class NameData : TwoName
    {
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
            get { return fUrl; }
            set { fUrl = value; }
        }

        private string fCurrency;

        /// <summary>
        /// Any currency name.
        /// </summary>
        public virtual string Currency
        {
            get { return fCurrency; }
            set { fCurrency = value; }
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

        public virtual string SectorsFlat
        {
            get { return string.Join(",", fSectors); }
            set
            {
                HashSet<string> sectorList = new HashSet<string>();
                if (!string.IsNullOrEmpty(value))
                {
                    var sectorsSplit = value.Split(',');

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
