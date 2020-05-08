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
        /// Outputs a copy of the account if it exists.
        /// </summary>
        public static bool TryGetAccount(this IPortfolio portfolio, AccountType accountType, TwoName names, out ISingleValueDataList desired)
        {
            bool success = false;
            desired = null;
            switch (accountType)
            {
                case (AccountType.BankAccount):
                    {
                        foreach (ICashAccount sec in portfolio.BankAccounts)
                        {
                            if (names.IsEqualTo(sec.Names))
                            {
                                desired = sec.Copy();
                                success = true;
                            }
                        }
                        break;
                    }
                case (AccountType.Currency):
                    {
                        foreach (ICurrency currency in portfolio.Currencies)
                        {
                            if (names.IsEqualTo(currency.Names))
                            {
                                desired = currency.Copy();
                                success = true;
                            }
                        }
                        break;
                    }
                case (AccountType.Sector):
                    {
                        foreach (ISector sector in portfolio.BenchMarks)
                        {
                            if (sector.Name == names.Name)
                            {
                                desired = sector.Copy();
                                success = true;
                            }
                        }
                        break;
                    }
            }

            return success;
        }
    }
}
