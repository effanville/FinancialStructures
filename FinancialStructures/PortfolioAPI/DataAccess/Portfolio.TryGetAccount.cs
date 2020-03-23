using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Outputs a copy of the security if it exists.
        /// </summary>
        public static bool TryGetSecurity(this IPortfolio portfolio, TwoName names, out ISecurity desired)
        {
            foreach (ISecurity sec in portfolio.Funds)
            {
                if (names.IsEqualTo(sec.Names))
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
        public static bool TryGetBankAccount(this IPortfolio portfolio, TwoName names, out ICashAccount desired)
        {
            foreach (ICashAccount sec in portfolio.BankAccounts)
            {
                if (names.IsEqualTo(sec.Names))
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
        public static bool TryGetSector(this IPortfolio portfolio, string name, out ISector Desired)
        {
            foreach (ISector sector in portfolio.BenchMarks)
            {
                if (sector.Name == name)
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
        public static bool TryGetCurrency(this IPortfolio portfolio, TwoName names, out ICurrency desired)
        {
            foreach (ICurrency currency in portfolio.Currencies)
            {
                if (names.IsEqualTo(currency.Names))
                {
                    desired = currency.Copy();
                    return true;
                }
            }

            desired = null;
            return false;
        }
    }
}
