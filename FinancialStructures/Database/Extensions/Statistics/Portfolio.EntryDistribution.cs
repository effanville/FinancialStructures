using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Returns a dictionary with the distribution over time of values for the
        /// account type.
        /// </summary>
        public static Dictionary<DateTime, int> EntryDistribution(this IPortfolio portfolio, Totals elementType = Totals.Security, TwoName company = null)
        {
            switch (elementType)
            {
                case Totals.All:
                {
                    return portfolio.EntryDistribution(Totals.Security);
                }
                case Totals.Security:
                {
                    return EntryDistributionOf(portfolio.FundsThreadSafe);
                }
                case Totals.SecurityCompany:
                {
                    return EntryDistributionOf(portfolio.CompanyAccounts(Account.Security, company.Company));
                }
                case Totals.BankAccount:
                {
                    return EntryDistributionOf(portfolio.BankAccountsThreadSafe);
                }
                case Totals.Benchmark:
                {
                    return EntryDistributionOf(portfolio.BenchMarksThreadSafe);
                }
                default:
                {
                    return new Dictionary<DateTime, int>();
                }
            }
        }

        private static Dictionary<DateTime, int> EntryDistributionOf(IReadOnlyList<ISecurity> accounts)
        {
            Dictionary<DateTime, int> totals = new Dictionary<DateTime, int>();
            foreach (ISecurity desired in accounts)
            {
                if (desired.Any())
                {
                    foreach (DailyValuation value in desired.Shares.Values())
                    {
                        if (totals.TryGetValue(value.Day, out _))
                        {
                            totals[value.Day] += 1;
                        }
                        else
                        {
                            totals.Add(value.Day, 1);
                        }
                    }

                    foreach (DailyValuation priceValue in desired.UnitPrice.Values())
                    {
                        if (totals.TryGetValue(priceValue.Day, out _))
                        {
                            totals[priceValue.Day] += 1;
                        }
                        else
                        {
                            totals.Add(priceValue.Day, 1);
                        }
                    }

                    foreach (DailyValuation investmentValue in desired.Investments.Values())
                    {
                        if (totals.TryGetValue(investmentValue.Day, out _))
                        {
                            totals[investmentValue.Day] += 1;
                        }
                        else
                        {
                            totals.Add(investmentValue.Day, 1);
                        }
                    }
                }
            }

            return totals;
        }

        private static Dictionary<DateTime, int> EntryDistributionOf(IReadOnlyList<IValueList> accounts)
        {
            Dictionary<DateTime, int> totals = new Dictionary<DateTime, int>();
            foreach (IValueList cashAccount in accounts)
            {
                foreach (DailyValuation value in cashAccount.ListOfValues())
                {
                    if (totals.TryGetValue(value.Day, out _))
                    {
                        totals[value.Day] += 1;
                    }
                    else
                    {
                        totals.Add(value.Day, 1);
                    }
                }
            }

            return totals;
        }
    }
}
