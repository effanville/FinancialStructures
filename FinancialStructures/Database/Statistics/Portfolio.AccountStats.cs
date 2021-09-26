using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.Statistics;

namespace FinancialStructures.Database.Statistics
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
        /// <param name="account">The type of account data to display.</param>
        /// <param name="displayValueFunds">Whether funds with 0 latest value should be displayed only, or all funds displayed.</param>
        /// <param name="displayTotals">Whether totals values should be calculated</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, Account account, bool displayValueFunds, bool displayTotals = true, Statistic[] statisticsToDisplay = null)
        {
            if (portfolio != null)
            {
                var stats = new List<AccountStatistics>();
                switch (account)
                {
                    case Account.Security:
                    default:
                    {
                        foreach (ISecurity security in portfolio.FundsThreadSafe)
                        {
                            var latest = security.LatestValue();
                            if ((displayValueFunds && latest?.Value > 0) || !displayValueFunds)
                            {
                                stats.Add(new AccountStatistics(portfolio, account, security.Names, statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics()));
                            }
                        }

                        stats.Sort();
                        if (displayTotals)
                        {
                            stats.Add(new AccountStatistics(portfolio, Totals.Security, new NameData("Totals", ""), statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics()));
                        }
                        break;
                    }
                    case Account.BankAccount:
                    {
                        foreach (IExchangableValueList acc in portfolio.BankAccountsThreadSafe)
                        {
                            var latest = acc.LatestValue();
                            if ((displayValueFunds && latest?.Value > 0) || !displayValueFunds)
                            {
                                stats.Add(new AccountStatistics(portfolio, account, acc.Names, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                            }
                        }

                        stats.Sort();

                        if (displayTotals)
                        {
                            stats.Add(new AccountStatistics(portfolio, Totals.BankAccount, new NameData("Totals", ""), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                        }
                        break;
                    }
                    case Account.Benchmark:
                    {
                        foreach (IValueList acc in portfolio.BenchMarksThreadSafe)
                        {
                            var latest = acc.LatestValue();
                            if ((displayValueFunds && latest?.Value > 0) || !displayValueFunds)
                            {
                                stats.Add(new AccountStatistics(portfolio, account, acc.Names, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats()));
                            }
                        }

                        stats.Sort();

                        if (displayTotals)
                        {

                            stats.Add(new AccountStatistics(portfolio, Totals.Benchmark, new NameData("Totals", ""), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats()));
                        }
                        break;
                    }
                    case Account.Currency:
                    {
                        foreach (ICurrency acc in portfolio.CurrenciesThreadSafe)
                        {
                            var latest = acc.LatestValue();
                            if ((displayValueFunds && latest?.Value > 0) || !displayValueFunds)
                            {
                                stats.Add(new AccountStatistics(portfolio, account, acc.Names, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                            }
                        }

                        stats.Sort();
                        if (displayTotals)
                        {
                            stats.Add(new AccountStatistics(portfolio, Totals.Currency, new NameData("Totals", ""), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                        }
                        break;
                    }
                    case Account.All:
                    {
                        stats.AddRange(portfolio.GetStats(Account.Security, displayValueFunds, displayTotals, statisticsToDisplay));
                        stats.AddRange(portfolio.GetStats(Account.BankAccount, displayValueFunds, displayTotals, statisticsToDisplay));
                        stats.Sort();
                        break;
                    }
                }

                return stats;
            }

            return null;
        }

        /// <summary>
        /// Generates statistics for the portfolio. This generates a list of statistics containing only one element with statistics of the
        /// <see cref="Account"/> with the specified name only.
        /// </summary>
        /// <param name="portfolio">The portfolio to generate statistics for.</param>
        /// <param name="account">The type of account data to display.</param>
        /// <param name="name">The name of the account to query for.</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed.</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, Account account, TwoName name, Statistic[] statisticsToDisplay = null)
        {
            var stats = new List<AccountStatistics>();
            if (portfolio != null)
            {
                switch (account)
                {
                    case Account.Security:
                    default:
                    {
                        stats.Add(new AccountStatistics(portfolio, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics()));

                        break;
                    }
                    case Account.BankAccount:
                    {
                        stats.Add(new AccountStatistics(portfolio, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));

                        break;
                    }
                    case Account.Benchmark:
                    {
                        if (portfolio.Exists(Account.Benchmark, new TwoName(null, name.Name)))
                        {
                            stats.Add(new AccountStatistics(portfolio, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats()));
                        }
                        break;
                    }
                    case Account.Currency:
                    {
                        stats.Add(new AccountStatistics(portfolio, account, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));

                        break;
                    }
                }
            }

            return stats;
        }

        /// <summary>
        /// Generates statistics for the portfolio. This generates a list of statistics containing all totals over the specified
        /// <see cref="Totals"/> value..
        /// </summary>
        /// <param name="portfolio">The portfolio to generate statistics for.</param>
        /// <param name="total">The type of Totals data to display.</param>
        /// <param name="displayValueFunds">Whether funds with 0 latest value should be displayed only, or all funds displayed.</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed.</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, Totals total, bool displayValueFunds = true, Statistic[] statisticsToDisplay = null)
        {
            if (portfolio != null)
            {
                var stats = new List<AccountStatistics>();
                switch (total)
                {
                    case Totals.All:
                    {
                        stats.Add(new AccountStatistics(portfolio, total, new NameData("Totals", "All"), statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics()));
                        break;
                    }
                    case Totals.Security:
                    case Totals.SecurityCompany:
                    case Totals.SecuritySector:
                    default:
                    {
                        foreach (string company in portfolio.Companies(total.ToAccount()))
                        {
                            double latest = portfolio.TotalValue(total, new TwoName(company));
                            if ((displayValueFunds && latest > 0) || !displayValueFunds)
                            {
                                stats.Add(new AccountStatistics(portfolio, total, new NameData(company, "Totals"), statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics()));
                            }
                        }

                        break;
                    }
                    case Totals.BankAccount:
                    case Totals.BankAccountCompany:
                    {
                        foreach (string bankAccount in portfolio.Companies(total.ToAccount()))
                        {
                            double latest = portfolio.TotalValue(total, new TwoName(bankAccount));
                            if ((displayValueFunds && latest > 0) || !displayValueFunds)
                            {
                                stats.Add(new AccountStatistics(portfolio, total, new NameData(bankAccount, "Totals"), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                            }
                        }
                        break;
                    }
                    case Totals.BankAccountSector:
                    {
                        foreach (string bankAccount in portfolio.Companies(total.ToAccount()))
                        {
                            double latest = portfolio.TotalValue(total, new TwoName(bankAccount));
                            if ((displayValueFunds && latest > 0) || !displayValueFunds)
                            {
                                stats.Add(new AccountStatistics(portfolio, total, new NameData(bankAccount, "Totals"), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats()));
                            }
                        }
                        break;
                    }
                    case Totals.Benchmark:
                    {
                        break;
                    }
                    case Totals.Currency:
                    {
                        break;
                    }
                }

                return stats;
            }

            return null;
        }

        /// <summary>
        /// Generates statistics for the portfolio. This generates a list of statistics containing only one element with statistics of the
        /// <see cref="Totals"/> with the specified name only.
        /// </summary>
        /// <param name="portfolio">The portfolio to generate statistics for.</param>
        /// <param name="total">The type of account data to display.</param>
        /// <param name="name">The name of the account to query for.</param>
        /// <param name="statisticsToDisplay">The array of statistics to be displayed.</param>
        public static List<AccountStatistics> GetStats(this IPortfolio portfolio, Totals total, TwoName name, Statistic[] statisticsToDisplay = null)
        {
            if (portfolio != null)
            {
                var stats = new List<AccountStatistics>();
                switch (total)
                {
                    case Totals.All:
                    default:
                    case Totals.Security:
                    {
                        stats.Add(new AccountStatistics(portfolio, total, new TwoName("Totals", total.ToString()), statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics()));
                        break;
                    }
                    case Totals.BankAccount:
                    case Totals.Currency:
                    {
                        stats.Add(new AccountStatistics(portfolio, total, new TwoName("Totals", total.ToString()), statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                        break;
                    }
                    case Totals.BankAccountCompany:
                    case Totals.BankAccountSector:
                    {
                        stats.Add(new AccountStatistics(portfolio, total, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultBankAccountStats()));
                        break;
                    }
                    case Totals.SecuritySector:
                    case Totals.SecurityCompany:
                    {
                        stats.Add(new AccountStatistics(portfolio, total, name, statisticsToDisplay ?? AccountStatisticsHelpers.AllStatistics()));
                        break;
                    }
                    case Totals.Sector:
                    case Totals.Benchmark:
                    {
                        stats.Add(new AccountStatistics(portfolio, total, name, statisticsToDisplay ?? AccountStatisticsHelpers.DefaultSectorStats()));
                        break;
                    }
                }

                return stats;
            }

            return null;
        }

    }
}
