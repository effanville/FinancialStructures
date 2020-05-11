using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.StatisticStructures;
using StructureCommon.FinanceFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        public static int LongestName(this IPortfolio portfolio)
        {
            var names = portfolio.Names(AccountType.Security);
            if (names.Any())
            {
                return names.Max().Length;
            }
            return 0;
        }

        public static int LongestCompany(this IPortfolio portfolio)
        {
            var companies = portfolio.Companies(AccountType.Security);
            companies.Sort();
            if (companies != null)
            {
                return companies.Select(c => c.Length).Max();
            }
            return 0;
        }

        /// <summary>
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        public static DateTime FirstValueDate(this IPortfolio portfolio)
        {
            var output = DateTime.Today;
            foreach (ISecurity sec in portfolio.Funds)
            {
                if (sec.Any())
                {
                    ICurrency currency = portfolio.Currencies.Find(cur => cur.Name == sec.Currency);
                    var securityEarliest = sec.FirstValue(currency).Day;
                    if (securityEarliest < output)
                    {
                        output = securityEarliest;
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double TotalProfit(this IPortfolio portfolio)
        {
            double total = 0;
            foreach (ISecurity security in portfolio.Funds)
            {
                if (security.Any())
                {
                    ICurrency currency = portfolio.Currencies.Find(cur => cur.Name == security.Currency);
                    total += portfolio.Profit(security.Names);
                }
            }

            return total;
        }

        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double RecentChange(this IPortfolio portfolio)
        {
            double total = 0;
            foreach (ISecurity desired in portfolio.Funds)
            {
                if (desired.Any())
                {
                    total += portfolio.RecentChange(desired.Names);
                }
            }

            return total;
        }

        /// <summary>
        /// If possible, returns the IRR of all securities over the time period.
        /// </summary>
        public static double IRRPortfolio(this IPortfolio portfolio, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.NumberOf(AccountType.Security) == 0)
            {
                return double.NaN;
            }
            double earlierValue = 0;
            double laterValue = 0;
            var Investments = new List<DailyValuation>();

            foreach (ISecurity security in portfolio.Funds)
            {
                if (security.Any())
                {
                    ICurrency currency = portfolio.Currencies.Find(cur => cur.Name == security.Currency);
                    earlierValue += security.Value(earlierTime, currency).Value;
                    laterValue += security.Value(laterTime, currency).Value;
                    Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime, currency));
                }
            }

            return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
        }

        /// <summary>
        /// The total value of all securities and bank accounts on the date specified
        /// </summary>
        public static double Value(this IPortfolio portfolio, DateTime date)
        {
            return portfolio.TotalValue(AccountType.Security, date) + portfolio.TotalValue(AccountType.BankAccount, date);
        }

        public static List<DatabaseStatistics> GenerateDatabaseStatistics(this IPortfolio portfolio)
        {
            var names = new List<DatabaseStatistics>();
            foreach (ISecurity sec in portfolio.Funds)
            {
                names.Add(new DatabaseStatistics(sec.Company, sec.Name, sec.FirstValue().Day, sec.LatestValue().Day, sec.Count(), (sec.LatestValue().Day - sec.FirstValue().Day).Days / (365 * (double)sec.Count())));
            }
            foreach (ICashAccount bankAcc in portfolio.BankAccounts)
            {
                names.Add(new DatabaseStatistics(bankAcc.Name, bankAcc.Company, bankAcc.FirstValue().Day, bankAcc.LatestValue().Day, bankAcc.Count(), 365 * (double)bankAcc.Count() / (bankAcc.LatestValue().Day - bankAcc.FirstValue().Day).Days));
            }

            return names;
        }

        public async static Task<List<PortfolioDaySnapshot>> GenerateHistoryStats(this IPortfolio portfolio, int daysGap)
        {
            var outputs = new List<PortfolioDaySnapshot>();
            var calculationDate = portfolio.FirstValueDate();
            await Task.Run(() => BackGroundTask(calculationDate, portfolio, outputs, daysGap));
            return outputs;
        }

        private static void BackGroundTask(DateTime calculationDate, IPortfolio portfolio, List<PortfolioDaySnapshot> outputs, int daysGap)
        {
            while (calculationDate < DateTime.Today)
            {
                var calcuationDateStatistics = new PortfolioDaySnapshot(calculationDate, portfolio);
                outputs.Add(calcuationDateStatistics);
                calculationDate = calculationDate.AddDays(daysGap);
            }
            if (calculationDate == DateTime.Today)
            {
                var calcuationDateStatistics = new PortfolioDaySnapshot(calculationDate, portfolio);
                outputs.Add(calcuationDateStatistics);
            }
        }
    }
}
