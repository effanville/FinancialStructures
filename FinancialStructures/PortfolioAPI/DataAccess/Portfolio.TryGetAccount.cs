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
            foreach (Security sec in portfolio.Funds)
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
            foreach (CashAccount sec in portfolio.BankAccounts)
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
        /// Returns a sector from the database with specified name.
        /// </summary>
        public static bool TryGetSector(this Portfolio portfolio, string name, out Sector Desired)
        {
            foreach (Sector sector in portfolio.BenchMarks)
            {
                if (sector.GetName() == name)
                {
                    Desired = sector.Copy();
                    return true;
                }
            }

            Desired = null;
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
