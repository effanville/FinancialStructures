using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.DataStructures;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    /// <summary>
    /// Contains extension methods for calculating statistics.
    /// </summary>
    public static partial class Statistics
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalMDD(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(total, name);
            DateTime laterTime = portfolio.LatestDate(total, name);
            return portfolio.TotalMDD(total, earlierTime, laterTime, name);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalMDD(this IPortfolio portfolio, Totals accountType, DateTime earlierTime, DateTime laterTime, TwoName name = null)
        {
            switch (accountType)
            {
                case Totals.All:
                case Totals.Security:
                {
                    return TotalMDDOf(portfolio.FundsThreadSafe, earlierTime, laterTime);
                }
                case Totals.SecurityCompany:
                {
                    return TotalMDDOf(portfolio.CompanyAccounts(Account.Security, name.Company), earlierTime, laterTime);
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    return TotalMDDOf(portfolio.SectorAccounts(Account.Security, name), earlierTime, laterTime);
                }
                case Totals.BankAccount:
                {
                    return TotalMDDOf(portfolio.BankAccountsThreadSafe, earlierTime, laterTime);
                }
                case Totals.BankAccountCompany:
                {
                    return TotalMDDOf(portfolio.CompanyAccounts(Account.BankAccount, name.Company), earlierTime, laterTime);
                }
                case Totals.BankAccountSector:
                {
                    return TotalMDDOf(portfolio.SectorAccounts(Account.BankAccount, name), earlierTime, laterTime);
                }
                case Totals.Benchmark:
                {
                    return TotalMDDOf(portfolio.BankAccountsThreadSafe, earlierTime, laterTime);
                }
                case Totals.Company:
                {
                    return TotalMDDOf(portfolio.CompanyAccounts(Account.All, name.Company), earlierTime, laterTime);
                }
                case Totals.Currency:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                default:
                {
                    return 0.0;
                }
            }
        }

        private static double TotalMDDOf(IReadOnlyList<IValueList> securities, DateTime earlierTime, DateTime laterTime)
        {
            List<DateTime> values = new List<DateTime>();
            foreach (IValueList security in securities)
            {
                List<DailyValuation> vals = security.ListOfValues().Where(value => value.Day >= earlierTime && value.Day <= laterTime && !value.Value.Equals(0.0)).ToList();
                foreach (DailyValuation val in vals)
                {
                    if (!values.Any(value => value.Equals(val.Day)))
                    {
                        values.Add(val.Day);
                    }
                }
            }

            values.Sort();
            List<DailyValuation> valuations = new List<DailyValuation>();
            foreach (DateTime date in values)
            {
                double value = 0.0;
                foreach (IValueList sec in securities)
                {
                    value += sec.Value(date).Value;
                }
                valuations.Add(new DailyValuation(date, value));
            }

            return MDD(valuations);
        }

        /// <summary>
        /// Calculates the IRR for the account with specified account and name.
        /// </summary>
        public static double MDD(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            DateTime firstDate = portfolio.FirstDate(accountType, names);
            DateTime lastDate = portfolio.LatestDate(accountType, names);
            return portfolio.MDD(accountType, names, firstDate, lastDate);
        }

        /// <summary>
        /// Calculates the MDD for the account with specified account and name between the times specified.
        /// </summary>
        public static double MDD(this IPortfolio portfolio, Account accountType, TwoName names, DateTime earlierTime, DateTime laterTime)
        {
            switch (accountType)
            {
                case Account.Security:
                case Account.BankAccount:
                case Account.Benchmark:
                case Account.Currency:
                {
                    if (!portfolio.TryGetAccount(accountType, names, out IValueList desired))
                    {
                        return double.NaN;
                    }

                    List<DailyValuation> values = desired.ListOfValues().Where(value => value.Day >= earlierTime && value.Day <= laterTime && !value.Value.Equals(0.0)).ToList();
                    return MDD(values);
                }
                case Account.All:
                default:
                {
                    return 0.0;
                }
            }
        }

        /// <summary>
        /// Calculate the MDD of a list of values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double MDD(List<DailyValuation> values)
        {
            if (values.Any())
            {
                double maximumDrawDown = 0.0;
                double peakValue = double.MinValue;
                for (int i = 0; i < values.Count; i++)
                {
                    double value = values[i].Value;
                    if (value > peakValue)
                    {
                        peakValue = value;
                    }

                    double drawDown = 100.0 * (peakValue - value) / peakValue;
                    if (drawDown > maximumDrawDown)
                    {
                        maximumDrawDown = drawDown;
                    }
                }

                return maximumDrawDown;
            }

            return double.NaN;
        }
    }
}
