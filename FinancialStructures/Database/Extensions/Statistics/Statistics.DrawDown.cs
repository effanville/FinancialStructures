﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.DataStructures;
using Common.Structure.MathLibrary.Finance;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalDrawdown(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(total, name);
            DateTime laterTime = portfolio.LatestDate(total, name);
            return portfolio.TotalDrawdown(total, earlierTime, laterTime, name);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalDrawdown(this IPortfolio portfolio, Totals accountType, DateTime earlierTime, DateTime laterTime, TwoName name = null)
        {
            switch (accountType)
            {
                case Totals.All:
                case Totals.Security:
                {
                    return TotalDrawdownOf(portfolio.FundsThreadSafe, earlierTime, laterTime);
                }
                case Totals.SecurityCompany:
                {
                    return TotalDrawdownOf(portfolio.CompanyAccounts(Account.Security, name.Company), earlierTime, laterTime);
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    return TotalDrawdownOf(portfolio.SectorAccounts(Account.Security, name), earlierTime, laterTime);
                }
                case Totals.BankAccount:
                {
                    return TotalDrawdownOf(portfolio.BankAccountsThreadSafe, earlierTime, laterTime);
                }
                case Totals.BankAccountCompany:
                {
                    return TotalDrawdownOf(portfolio.CompanyAccounts(Account.BankAccount, name.Company), earlierTime, laterTime);
                }
                case Totals.BankAccountSector:
                {
                    return TotalDrawdownOf(portfolio.SectorAccounts(Account.BankAccount, name), earlierTime, laterTime);
                }
                case Totals.Benchmark:
                {
                    return TotalDrawdownOf(portfolio.BankAccountsThreadSafe, earlierTime, laterTime);
                }
                case Totals.Company:
                {
                    return TotalDrawdownOf(portfolio.CompanyAccounts(Account.All, name.Company), earlierTime, laterTime);
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

        private static double TotalDrawdownOf(IReadOnlyList<IValueList> securities, DateTime earlierTime, DateTime laterTime)
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
                decimal value = 0.0m;
                foreach (IValueList sec in securities)
                {
                    value += sec.Value(date).Value;
                }

                valuations.Add(new DailyValuation(date, value));
            }

            return (double)FinanceFunctions.Drawdown(valuations);
        }

        /// <summary>
        /// Calculates the IRR for the account with specified account and name.
        /// </summary>
        public static double Drawdown(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            DateTime firstDate = portfolio.FirstDate(accountType, names);
            DateTime lastDate = portfolio.LatestDate(accountType, names);
            return portfolio.Drawdown(accountType, names, firstDate, lastDate);
        }

        /// <summary>
        /// Calculates the MDD for the account with specified account and name between the times specified.
        /// </summary>
        public static double Drawdown(this IPortfolio portfolio, Account accountType, TwoName names, DateTime earlierTime, DateTime laterTime)
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
                    return (double)FinanceFunctions.Drawdown(values);
                }
                case Account.All:
                default:
                {
                    return 0.0;
                }
            }
        }
    }
}
