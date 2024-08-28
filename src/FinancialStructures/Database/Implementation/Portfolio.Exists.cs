using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool CompanyExists(Account elementType, string company)
        {
            foreach (string comp in Companies(elementType))
            {
                if (comp.Equals(company))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Exists(Account elementType, TwoName name)
        {
            if (elementType == Account.Security)
            {
                return _funds.Exists(name);
            }

            foreach (TwoName sec in NameDataForAccount(elementType))
            {
                if (sec.IsEqualTo(name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
