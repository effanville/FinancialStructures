using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio, Totals elementType = Totals.Security, string company = null)
        {
            switch (elementType)
            {
                case Totals.All:
                {
                    return portfolio.RecentChange(Totals.Security) + portfolio.RecentChange(Totals.BankAccount);
                }
                case (Totals.Security):
                {
                    double total = 0;
                    foreach (ISecurity desired in portfolio.Funds)
                    {
                        if (desired.Any())
                        {
                            total += portfolio.RecentChange(AccountToTotalsConverter.ConvertTotalToAccount(elementType), desired.Names);
                        }
                    }

                    return total;
                }
                case (Totals.SecurityCompany):
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
                            total += portfolio.RecentChange(AccountToTotalsConverter.ConvertTotalToAccount(elementType), desired.Names);
                        }
                    }

                    return total;
                }
                case Totals.BankAccount:
                {
                    double total = 0.0;
                    foreach (ICashAccount cashAccount in portfolio.BankAccounts)
                    {
                        total += portfolio.RecentChange(AccountToTotalsConverter.ConvertTotalToAccount(elementType), cashAccount.Names);
                    }

                    return total;
                }
                default:
                {
                    return 0.0;
                }
            }
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio, Account elementType, TwoName names)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    if (portfolio.TryGetSecurity(names, out ISecurity desired))
                    {
                        if (desired.Any())
                        {
                            ICurrency currency = portfolio.Currency(Account.Security, desired);
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
                case (Account.BankAccount):
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
