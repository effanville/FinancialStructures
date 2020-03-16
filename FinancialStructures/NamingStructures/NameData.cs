using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Any name data associated to an account.
    /// </summary>
    public class NameData : TwoName
    {
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
        public NameData(string company, string name, string currency, string url, List<string> sectors)
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
        public string Url
        {
            get { return fUrl; }
            set { fUrl = value; }
        }

        private string fCurrency;

        /// <summary>
        /// Any currency name.
        /// </summary>
        public string Currency
        {
            get { return fCurrency; }
            set { fCurrency = value; }
        }

        private List<string> fSectors = new List<string>();

        /// <summary>
        /// Sectors associated to account.
        /// </summary>
        public List<string> Sectors
        {
            get { return fSectors; }
            set { fSectors = value; }
        }

        public string SectorsFlat
        {
            get { return string.Join(",", fSectors); }
            set
            {
                List<string> sectorList = new List<string>();
                if (!string.IsNullOrEmpty(value))
                {
                    var sectorsSplit = value.Split(',');

                    sectorList.AddRange(sectorsSplit);
                    for (int i = 0; i < sectorList.Count; i++)
                    {
                        sectorList[i] = sectorList[i].Trim(' ');
                    }
                }

                fSectors = sectorList;
            }
        }
    }
}
