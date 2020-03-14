using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Outputs a copy of the security if it exists.
        /// </summary>
        public static bool TryGetSecurity(this Portfolio portfolio, string company, string name, out Security desired)
        {
            foreach (Security sec in portfolio.GetSecurities())
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    desired = sec.Copy();
                    return true;
                }
            }
            desired = null;
            return false;
        }

        /// <summary>
        /// Outputs a copy of the BankAccount if it exists.
        /// </summary>
        public static bool TryGetBankAccount(this Portfolio portfolio, string company, string name, out CashAccount desired)
        {
            foreach (CashAccount sec in portfolio.GetBankAccounts())
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    desired = sec.Copy();
                    return true;
                }
            }

            desired = null;
            return false;
        }

        /// <summary>
        /// Outputs a copy of the BankAccount if it exists.
        /// </summary>
        public static bool TryGetCurrency(this Portfolio portfolio, string name, out Currency desired)
        {
            foreach (Currency sec in portfolio.GetCurrencies())
            {
                if (sec.GetName() == name)
                {
                    desired = sec.Copy();
                    return true;
                }
            }

            desired = null;
            return false;
        }
    }
}
