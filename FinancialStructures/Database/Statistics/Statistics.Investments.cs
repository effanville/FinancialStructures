using System.Collections.Generic;
using System.Linq;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// Class containing static extension methods for statistics for a portfolio.
    /// </summary>
    public static partial class Statistics
    {
        /// <summary>
        /// Returns a list of all the investments in the given type.
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="accountType"></param>
        /// <param name="Name"></param>
        public static List<Labelled<TwoName, DailyValuation>> TotalInvestments(this IPortfolio portfolio, Totals accountType, TwoName Name = null)
        {
            switch (accountType)
            {
                case Totals.Security:
                {
                    List<Labelled<TwoName, DailyValuation>> output = new List<Labelled<TwoName, DailyValuation>>();
                    List<string> companies = portfolio.Companies(Account.Security).ToList();
                    companies.Sort();
                    foreach (string comp in companies)
                    {
                        output.AddRange(portfolio.TotalInvestments(Totals.SecurityCompany, new TwoName(comp)));
                    }
                    output.Sort();
                    return output;
                }
                case Totals.SecurityCompany:
                {
                    List<Labelled<TwoName, DailyValuation>> output = new List<Labelled<TwoName, DailyValuation>>();
                    foreach (ISecurity sec in portfolio.CompanyAccounts(Account.Security, Name.Company))
                    {
                        ICurrency currency = portfolio.Currency(Account.Security, sec);
                        output.AddRange(sec.AllInvestmentsNamed(currency));
                    }

                    return output;
                }
                case Totals.SecuritySector:
                {
                    List<Labelled<TwoName, DailyValuation>> output = new List<Labelled<TwoName, DailyValuation>>();
                    foreach (ISecurity security in portfolio.SectorAccounts(Account.Security, Name))
                    {
                        output.AddRange(security.AllInvestmentsNamed());
                    }

                    return output;
                }
                case Totals.All:
                {
                    return portfolio.TotalInvestments(Totals.Security);
                }
                default:
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the investments in the object specified
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="accountType">The type of account to look for.</param>
        /// <param name="names">The name of the account.</param>
        /// <returns></returns>
        public static List<Labelled<TwoName, DailyValuation>> Investments(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            switch (accountType)
            {
                case Account.Security:
                {
                    if (portfolio.TryGetAccount(Account.Security, names, out IValueList desired))
                    {
                        if (desired.Any())
                        {
                            ISecurity security = desired as ISecurity;
                            ICurrency currency = portfolio.Currency(Account.Security, security);
                            return security.AllInvestmentsNamed(currency);
                        }
                    }

                    return null;
                }
                default:
                {
                    return null;
                }
            }
        }
    }
}
