using System.Collections.Generic;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// NameData with check whether data has been changed.
    /// </summary>
    public class NameData_ChangeLogged : NameData
    {
        /// <summary>
        /// Whether some value has changed.
        /// </summary>
        public bool NewValue
        {
            get;
            set;
        }

        public override string Company
        {
            set { base.Company = value; NewValue = true; }
        }

        public override string Name
        {
            set { base.Name = value; NewValue = true; }
        }

        public override string Currency
        {
            set { base.Currency = value; NewValue = true; }
        }

        public override string Url
        {
            set { base.Url = value; NewValue = true; }
        }

        public override string SectorsFlat
        {
            set { base.SectorsFlat = value; NewValue = true; }
        }

        /// <summary>
        /// default empty constructor.
        /// </summary>
        public NameData_ChangeLogged()
            : base()
        {
            NewValue = true;
        }

        /// <summary>
        /// Set all name type values.
        /// </summary>
        public NameData_ChangeLogged(string company, string name, string currency, string url, List<string> sectors, bool newValue = true)
            : base(company, name, currency, url, sectors)
        {
            NewValue = newValue;
        }

        /// <summary>
        /// Set data without sectors.
        /// </summary>
        public NameData_ChangeLogged(string company, string name, string currency, string url, bool newValue = true)
             : base(company, name, currency, url)
        {
            NewValue = newValue;
        }

        /// <summary>
        /// Set minimal naming
        /// </summary>
        public NameData_ChangeLogged(string company, string name, bool newValue = true)
             : base(company, name)
        {
            NewValue = newValue;
        }
    }
}
