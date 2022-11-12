using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.Database.Statistics;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    /// <summary>
    /// Static extensions for calculating <see cref="AccountStatistics"/> classes for portfolio.
    /// </summary>
    public static class PortfolioAccountStats
    {
        /// <summary>
        /// Generates statistics for the portfolio. This generates a list of statistics for each element of the
        /// <see cref="Account"/> type specified.
        /// </summary>
        /// <param name="portfolio">The portfolio to generate statistics for.</param>
        /// <param name="dateToCalculate"></param>
        /// <param name="account">The type of account data to display.</param>
        /// <param name="displayValueFunds">Whether funds with 0 latest value should be displayed only, or all funds displayed.</param>
        /// <param name="displayTotals">Whether totals values should be calculated</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, DateTime dateToCalculate, Account account, bool displayValueFunds, bool displayTotals = true, Statistic[] statisticsToDisplay = null)
        {
            switch (account)
            {
                case Account.Security:
                default:
                {
                    return GenerateFromList(portfolio.FundsThreadSafe, portfolio, dateToCalculate, account, displayValueFunds, displayTotals, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSecurityStats());

                }
                case Account.BankAccount:
                {
                    return GenerateFromList(portfolio.BankAccountsThreadSafe, portfolio, dateToCalculate, account, displayValueFunds, displayTotals, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats());
                }
                case Account.Benchmark:
                {
                    return GenerateFromList(portfolio.BenchMarksThreadSafe, portfolio, dateToCalculate, account, displayValueFunds, displayTotals, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats());
                }
                case Account.Currency:
                {
                    return GenerateFromList(portfolio.CurrenciesThreadSafe, portfolio, dateToCalculate, account, displayValueFunds, displayTotals, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats());
                }
                case Account.Asset:
                {
                    return GenerateFromList(portfolio.Assets, portfolio, dateToCalculate, account, displayValueFunds, displayTotals, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultAssetStats());
                }
                case Account.Pension:
                {
                    return GenerateFromList(portfolio.Pensions, portfolio, dateToCalculate, account, displayValueFunds, displayTotals, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSecurityStats());
                }
                case Account.All:
                {
                    List<AccountStatistics> stats = new List<AccountStatistics>();
                    stats.AddRange(portfolio.GetStats(dateToCalculate, Account.Security, displayValueFunds, displayTotals, statisticsToDisplay));
                    stats.AddRange(portfolio.GetStats(dateToCalculate, Account.BankAccount, displayValueFunds, displayTotals, statisticsToDisplay));
                    stats.AddRange(portfolio.GetStats(dateToCalculate, Account.Pension, displayValueFunds, displayTotals, statisticsToDisplay));
                    stats.AddRange(portfolio.GetStats(dateToCalculate, Account.Asset, displayValueFunds, displayTotals, statisticsToDisplay));
                    stats.Sort();
                    return stats;
                }
            }
        }

        private static List<AccountStatistics> GenerateFromList(IReadOnlyList<IValueList> values, IPortfolio portfolio, DateTime dateToCalculate, Account account, bool displayValueFunds, bool displayTotals, Statistic[] statisticsToDisplay)
        {
            List<AccountStatistics> stats = new List<AccountStatistics>();
            foreach (IValueList security in values)
            {
                DailyValuation latest = security.LatestValue();
                if ((displayValueFunds && latest?.Value > 0) || !displayValueFunds)
                {
                    stats.Add(new AccountStatistics(portfolio, dateToCalculate, account, security.Names, statisticsToDisplay));
                }
            }

            stats.Sort();
            if (displayTotals)
            {
                stats.Add(new AccountStatistics(portfolio, dateToCalculate, account.ToTotals(), new NameData("Totals", ""), statisticsToDisplay));
            }
            return stats;
        }

        /// <summary>
        /// Generates statistics for the portfolio. This generates a list of statistics containing only one element with statistics of the
        /// <see cref="Account"/> with the specified name only.
        /// </summary>
        /// <param name="portfolio">The portfolio to generate statistics for.</param>
        /// <param name="dateToCalculate"></param>
        /// <param name="account">The type of account data to display.</param>
        /// <param name="name">The name of the account to query for.</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed.</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, DateTime dateToCalculate, Account account, TwoName name, Statistic[] statisticsToDisplay = null)
        {
            List<AccountStatistics> stats = new List<AccountStatistics>();
            if (portfolio != null)
            {
                switch (account)
                {
                    case Account.Security:
                    case Account.Pension:
                    default:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSecurityStats()));
                        break;
                    }
                    case Account.BankAccount:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                        break;
                    }
                    case Account.Benchmark:
                    {
                        if (portfolio.Exists(Account.Benchmark, new TwoName(null, name.Name)))
                        {
                            stats.Add(new AccountStatistics(portfolio, dateToCalculate, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats()));
                        }
                        break;
                    }
                    case Account.Currency:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));

                        break;
                    }
                    case Account.Asset:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultAssetStats()));
                        break;
                    }
                    case Account.All:
                        break;
                }
            }

            return stats;
        }

        /// <summary>
        /// Generates statistics for the portfolio. This generates a list of statistics containing all totals over the specified
        /// <see cref="Totals"/> value..
        /// </summary>
        /// <param name="portfolio">The portfolio to generate statistics for.</param>
        /// <param name="dateToCalculate"></param>
        /// <param name="total">The type of Totals data to display.</param>
        /// <param name="displayValueFunds">Whether funds with 0 latest value should be displayed only, or all funds displayed.</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed.</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, DateTime dateToCalculate, Totals total, bool displayValueFunds = true, Statistic[] statisticsToDisplay = null)
        {
            switch (total)
            {
                case Totals.All:
                {
                    List<AccountStatistics> stats = new List<AccountStatistics>
                    {
                        new AccountStatistics(portfolio, dateToCalculate, total, new NameData("Totals", "All"), statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics())
                    };
                    return stats;
                }
                case Totals.Security:
                case Totals.SecurityCompany:
                case Totals.SecuritySector:
                case Totals.Pension:
                case Totals.PensionCompany:
                case Totals.PensionSector:
                default:
                {
                    return GenerateFromList(portfolio.Companies(total.ToAccount()), portfolio, dateToCalculate, total, displayValueFunds, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSecurityStats());
                }
                case Totals.BankAccount:
                case Totals.BankAccountCompany:
                {
                    return GenerateFromList(portfolio.Companies(total.ToAccount()), portfolio, dateToCalculate, total, displayValueFunds, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats());
                }
                case Totals.BankAccountSector:
                {
                    return GenerateFromList(portfolio.Companies(total.ToAccount()), portfolio, dateToCalculate, total, displayValueFunds, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats());
                }
                case Totals.Asset:
                case Totals.AssetCompany:
                {
                    return GenerateFromList(portfolio.Companies(total.ToAccount()), portfolio, dateToCalculate, total, displayValueFunds, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultAssetStats());
                }
                case Totals.AssetSector:
                {
                    return GenerateFromList(portfolio.Companies(total.ToAccount()), portfolio, dateToCalculate, total, displayValueFunds, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats());
                }
                case Totals.Benchmark:
                case Totals.Currency:
                case Totals.Company:
                case Totals.Sector:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                case Totals.PensionCurrency:
                {
                    return null;
                }
            }
        }

        private static List<AccountStatistics> GenerateFromList(IReadOnlyList<string> values, IPortfolio portfolio, DateTime dateToCalculate, Totals totals, bool displayValueFunds, Statistic[] statisticsToDisplay)
        {
            List<AccountStatistics> stats = new List<AccountStatistics>();
            foreach (string company in values)
            {
                decimal latest = portfolio.TotalValue(totals, new TwoName(company));
                if ((displayValueFunds && latest > 0) || !displayValueFunds)
                {
                    stats.Add(new AccountStatistics(portfolio, dateToCalculate, totals, new NameData(company, "Totals"), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSecurityStats()));
                }
            }
            return stats;
        }

        /// <summary>
        /// Generates statistics for the portfolio. This generates a list of statistics containing only one element with statistics of the
        /// <see cref="Totals"/> with the specified name only.
        /// </summary>
        /// <param name="portfolio">The portfolio to generate statistics for.</param>
        /// <param name="dateToCalculate">The date to calculate stats on.</param>
        /// <param name="total">The type of account data to display.</param>
        /// <param name="name">The name of the account to query for.</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed.</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, DateTime dateToCalculate, Totals total, TwoName name, Statistic[] statisticsToDisplay = null)
        {
            if (portfolio != null)
            {
                List<AccountStatistics> stats = new List<AccountStatistics>();
                switch (total)
                {
                    case Totals.All:
                    default:
                    case Totals.Security:
                    case Totals.Pension:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, total, new TwoName("Totals", total.ToString()), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSecurityStats()));
                        break;
                    }
                    case Totals.BankAccount:
                    case Totals.Currency:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, total, new TwoName("Totals", total.ToString()), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                        break;
                    }
                    case Totals.BankAccountCompany:
                    case Totals.BankAccountSector:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, total, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                        break;
                    }
                    case Totals.SecuritySector:
                    case Totals.SecurityCompany:
                    case Totals.PensionSector:
                    case Totals.PensionCompany:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, total, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSecurityStats()));
                        break;
                    }
                    case Totals.Sector:
                    case Totals.Benchmark:
                    {
                        stats.Add(new AccountStatistics(portfolio, dateToCalculate, total, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats()));
                        break;
                    }
                    case Totals.Company:
                    case Totals.CurrencySector:
                    case Totals.SecurityCurrency:
                    case Totals.BankAccountCurrency:
                        break;
                }

                return stats;
            }

            return null;
        }
    }
}
