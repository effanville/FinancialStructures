using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        public static double CompanyRecentChange(this IPortfolio portfolio, string company)
        {
            double total = 0;

            List<ISecurity> securities = portfolio.CompanySecurities(company);
            if (securities.Count == 0)
            {
                return double.NaN;
            }

            foreach (ISecurity desired in securities)
            {
                if (desired.Any())
                {
                    total += portfolio.RecentChange(AccountType.Security, desired.Names);
                }
            }

            return total;
        }
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio, AccountType elementType = AccountType.Security)
        {
            double total = 0;
            foreach (ISecurity desired in portfolio.Funds)
            {
                if (desired.Any())
                {
                    total += portfolio.RecentChange(elementType, desired.Names);
                }
            }

            return total;
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio, AccountType elementType, TwoName names)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    if (portfolio.TryGetSecurity(names, out ISecurity desired))
                    {
                        if (desired.Any())
                        {
                            ICurrency currency = portfolio.Currency(AccountType.Security, desired);
                            DailyValuation needed = desired.LatestValue(currency);
                            if (needed.Value > 0)
                            {
                                return needed.Value - desired.LastEarlierValuation(needed.Day, currency).Value;
                            }

                            return 0.0;
                        }
                    }

                    break;
                }
                case (AccountType.BankAccount):
                {
                    if (portfolio.TryGetAccount(elementType, names, out ISingleValueDataList desired))
                    {
                        if (desired.Any())
                        {
                            var cashAcc = (ICashAccount)desired;
                            ICurrency currency = portfolio.Currency(elementType, cashAcc);
                            DailyValuation needed = cashAcc.LatestValue(currency);
                            if (needed.Value > 0)
                            {
                                return needed.Value - cashAcc.LastEarlierValuation(needed.Day, currency).Value;
                            }

                            return 0.0;
                        }
                    }
                    break;
                }
                default:
                {
                    if (portfolio.TryGetAccount(elementType, names, out ISingleValueDataList desired))
                    {
                        if (desired.Any())
                        {
                            DailyValuation needed = desired.LatestValue();
                            if (needed.Value > 0)
                            {
                                return needed.Value - desired.LastEarlierValuation(needed.Day).Value;
                            }

                            return 0.0;
                        }
                    }
                    break;
                }
            }

            return double.NaN;
        }
    }
}
