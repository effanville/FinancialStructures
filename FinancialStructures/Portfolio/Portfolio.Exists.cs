using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database
{

    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool CompanyExists(AccountType elementType, string company)
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
        public bool Exists(AccountType elementType, TwoName name)
        {
            foreach (TwoName sec in NameData(elementType))
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
