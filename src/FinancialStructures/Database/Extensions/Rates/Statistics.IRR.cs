using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.MathLibrary.Finance;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Rates
{
    /// <summary>
    /// Contains extension methods for calculating rates.
    /// </summary>
    public static class Rates
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalIRR(this IPortfolio portfolio, Totals total, string identifier = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(total, identifier);
            DateTime laterTime = portfolio.LatestDate(total, identifier);
            return portfolio.TotalIRR(total, earlierTime, laterTime, identifier);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalIRR(this IPortfolio portfolio, Totals accountType, DateTime earlierTime, DateTime laterTime, string identifier = null, int numIterations = 10)
        {
            IReadOnlyList<IValueList> accounts = portfolio.Accounts(accountType, identifier);
            DateTime earliestTime = portfolio.FirstValueDate(accountType, identifier);
            if (earlierTime < earliestTime)
            {
                earlierTime = earliestTime;
            }

            DateTime latestTime = portfolio.LatestDate(accountType, identifier);
            if (laterTime > latestTime)
            {
                laterTime = latestTime;
            }

            switch (accountType)
            {
                case Totals.All:
                case Totals.Security:
                case Totals.SecurityCompany:
                case Totals.Sector:
                case Totals.SecuritySector:
                case Totals.Pension:
                case Totals.PensionCompany:
                case Totals.PensionSector:
                case Totals.Company:
                case Totals.SecurityCurrency:
                {
                    return TotalIRR(accounts, portfolio, earlierTime, laterTime, numIterations);
                }
                case Totals.BankAccount:
                case Totals.BankAccountCompany:
                case Totals.BankAccountSector:
                case Totals.BankAccountCurrency:
                case Totals.Asset:
                case Totals.AssetCompany:
                case Totals.AssetSector:
                case Totals.AssetCurrency:
                case Totals.Benchmark:
                case Totals.Currency:
                case Totals.CurrencySector:
                {
                    return TotalCAR(accounts, portfolio, earlierTime, laterTime);
                }
                default:
                {
                    return 0.0;
                }
            }
        }

        private static double TotalCAR(IReadOnlyList<IValueList> valueLists, IPortfolio portfolio, DateTime earlierTime, DateTime laterTime)
        {
            if (!valueLists.Any())
            {
                return 0.0;
            }

            decimal earlierValue = 0;
            decimal laterValue = 0;

            foreach (IValueList valueList in valueLists)
            {
                if (valueList is ISecurity security && security.Any())
                {
                    ICurrency currency = portfolio.Currency(security.Names.Currency);
                    earlierValue += security.Value(earlierTime, currency, 0.0m);
                    laterValue += security.Value(laterTime, currency, 0.0m);
                }
                else if (valueList is IExchangeableValueList exchangableValueList && exchangableValueList.Any())
                {
                    ICurrency currency = portfolio.Currency(exchangableValueList.Names.Currency);
                    earlierValue += exchangableValueList.Value(earlierTime, currency, 0.0m);
                    laterValue += exchangableValueList.Value(laterTime, currency, 0.0m);
                }
                else
                {
                    earlierValue += valueList.Value(earlierTime, 0.0m);
                    laterValue += valueList.Value(laterTime, 0.0m);
                }
            }

            return FinanceFunctions.CAR(new DailyValuation(earlierTime, earlierValue), new DailyValuation(laterTime, laterValue));
        }

        private static double TotalIRR(IReadOnlyList<IValueList> valueLists, IPortfolio portfolio, DateTime earlierTime, DateTime laterTime, int numIterations)
        {
            if (!valueLists.Any())
            {
                return 0.0;
            }

            decimal earlierValue = 0;
            decimal laterValue = 0;
            List<DailyValuation> investments = new List<DailyValuation>();

            foreach (IValueList valueList in valueLists)
            {
                if (valueList is ISecurity security && security.Any())
                {
                    ICurrency currency = portfolio.Currency(security.Names.Currency);
                    earlierValue += security.Value(earlierTime, currency, 0.0m);
                    laterValue += security.Value(laterTime, currency, 0.0m);
                    investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                }
                else if (valueList is IExchangeableValueList exchangableValueList && exchangableValueList.Any())
                {
                    ICurrency currency = portfolio.Currency(exchangableValueList.Names.Currency);
                    earlierValue += exchangableValueList.Value(earlierTime, currency, 0.0m);
                    laterValue += exchangableValueList.Value(laterTime, currency, 0.0m);
                }
                else
                {
                    earlierValue += valueList.Value(earlierTime, 0.0m);
                    laterValue += valueList.Value(laterTime, 0.0m);
                }
            }

            return FinanceFunctions.IRR(new DailyValuation(earlierTime, earlierValue), investments, new DailyValuation(laterTime, laterValue), numIterations);
        }

        /// <summary>
        /// Calculates the IRR for the account with specified account and name.
        /// </summary>
        public static double IRR(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            return portfolio.CalculateValue(accountType,
                names, 
                vl =>
                {
                    ICurrency currency = portfolio.Currency(vl);
                    return vl.IRR(currency);
                },
                defaultValue: double.NaN);
        }

        /// <summary>
        /// Calculates the IRR for the account with specified account and name between the times specified.
        /// </summary>
        public static double IRR(this IPortfolio portfolio, Account accountType, TwoName names, DateTime earlierTime, DateTime laterTime)
        {
            return portfolio.CalculateValue(accountType,
                names,
                vl =>
                {
                    ICurrency currency = portfolio.Currency(vl);
                    return vl.IRR(currency, earlierTime, laterTime);
                },
                defaultValue: double.NaN);
        }
    }
}
