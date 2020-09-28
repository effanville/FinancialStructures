using System;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Database.Statistics
{
    public static partial class PortfolioStatisticGenerators
    {
        /// <summary>
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="sectorName">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime FirstValueDate(this IPortfolio portfolio, AccountType elementType, string sectorName = null)
        {
            DateTime output = DateTime.Today;
            switch (elementType)
            {
                case AccountType.Security:
                {
                    foreach (ISecurity sec in portfolio.Funds)
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
                case AccountType.Sector:
                {
                    foreach (ISecurity sec in portfolio.SectorSecurities(sectorName))
                    {
                        if (sec.FirstValue().Day < output)
                        {
                            output = sec.FirstValue().Day;
                        }
                    }
                    break;
                }
                case AccountType.BankAccount:
                {
                    foreach (ICashAccount cashAccount in portfolio.BankAccounts)
                    {
                        if (cashAccount.FirstValue().Day < output)
                        {
                            output = cashAccount.FirstValue().Day;
                        }
                    }

                    break;
                }
                case AccountType.Benchmark:
                {
                    foreach (ICashAccount cashAccount in portfolio.BenchMarks)
                    {
                        if (cashAccount.FirstValue().Day < output)
                        {
                            output = cashAccount.FirstValue().Day;
                        }
                    }

                    break;
                }
                case AccountType.Currency:
                {
                    foreach (ICashAccount cashAccount in portfolio.Currencies)
                    {
                        if (cashAccount.FirstValue().Day < output)
                        {
                            output = cashAccount.FirstValue().Day;
                        }
                    }

                    break;
                }
                case AccountType.All:
                {
                    var earlySecurity = portfolio.FirstValueDate(AccountType.Security);
                    var earlyBank = portfolio.FirstValueDate(AccountType.BankAccount);
                    output = earlySecurity < earlyBank ? earlySecurity : earlyBank;
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Finds the first date of all securities listed with regards to a specific company.
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static DateTime CompanyFirstDate(this IPortfolio portfolio, string company)
        {
            DateTime output = DateTime.Today;
            foreach (ISecurity sec in portfolio.CompanySecurities(company))
            {
                if (sec.FirstValue().Day < output)
                {
                    output = sec.FirstValue().Day;
                }
            }

            return output;
        }
    }
}