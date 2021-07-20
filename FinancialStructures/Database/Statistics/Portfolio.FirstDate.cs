using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class PortfolioStatisticGenerators
    {
        /// <summary>
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime FirstValueDate(this IPortfolio portfolio, Totals elementType, TwoName name = null)
        {
            DateTime output = DateTime.Today;
            switch (elementType)
            {
                case Totals.Security:
                {
                    foreach (ISecurity sec in portfolio.FundsThreadSafe)
                    {
                        if (sec.Any())
                        {
                            DateTime securityEarliest = sec.FirstValue().Day;
                            if (securityEarliest < output)
                            {
                                output = securityEarliest;
                            }
                        }
                    }

                    break;
                }
                case Totals.SecurityCompany:
                {
                    foreach (ISecurity sec in portfolio.CompanySecurities(name.Company))
                    {
                        if (sec.Any())
                        {
                            if (sec.FirstValue().Day < output)
                            {
                                output = sec.FirstValue().Day;
                            }
                        }
                    }

                    break;
                }
                case Totals.Sector:
                {
                    foreach (ISecurity sector in portfolio.SectorSecurities(name.Name))
                    {
                        if (sector.Any())
                        {
                            if (sector.FirstValue().Day < output)
                            {
                                output = sector.FirstValue().Day;
                            }
                        }
                    }

                    break;
                }
                case Totals.BankAccount:
                {
                    foreach (ICashAccount cashAccount in portfolio.BankAccountsThreadSafe)
                    {
                        if (cashAccount.Any())
                        {
                            if (cashAccount.FirstValue().Day < output)
                            {
                                output = cashAccount.FirstValue().Day;
                            }
                        }
                    }

                    break;
                }
                case Totals.Benchmark:
                {
                    foreach (IValueList benchMark in portfolio.BenchMarksThreadSafe)
                    {
                        if (benchMark.Any())
                        {
                            if (benchMark.FirstValue().Day < output)
                            {
                                output = benchMark.FirstValue().Day;
                            }
                        }
                    }

                    break;
                }
                case Totals.Currency:
                {
                    foreach (IValueList currency in portfolio.CurrenciesThreadSafe)
                    {
                        if (currency.Any())
                        {
                            if (currency.FirstValue().Day < output)
                            {
                                output = currency.FirstValue().Day;
                            }
                        }
                    }

                    break;
                }
                case Totals.All:
                {
                    var earlySecurity = portfolio.FirstValueDate(Totals.Security);
                    var earlyBank = portfolio.FirstValueDate(Totals.BankAccount);
                    output = earlySecurity < earlyBank ? earlySecurity : earlyBank;
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime FirstDate(this IPortfolio portfolio, Account elementType, TwoName name)
        {
            if (portfolio.TryGetAccount(elementType, name, out var desired))
            {
                return desired.FirstValue()?.Day ?? DateTime.MinValue;
            }

            return DateTime.MinValue;
        }
    }
}