using FinancialStructures.DatabaseInterfaces;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceFunctionsList;
using FinancialStructures.FinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        public static DateTime SectorFirstDate(this IPortfolio portfolio, string sector)
        {
            var output = DateTime.Today;
            foreach (var sec in portfolio.SectorSecurities(sector))
            {
                if (sec.FirstValue().Day < output)
                {
                    output = sec.FirstValue().Day;
                }
            }

            return output;
        }
        public static double SectorValue(this IPortfolio portfolio, string sectorName, DateTime date)
        {
            double sum = 0;
            if (portfolio.Funds != null)
            {
                foreach (var fund in portfolio.Funds)
                {
                    if (fund.IsSectorLinked(sectorName))
                    {
                        sum += fund.NearestEarlierValuation(date).Value;
                    }
                }
            }

            return sum;
        }

        public static int NumberSecuritiesInSector(this IPortfolio portfolio, string sectorName)
        {
            return portfolio.SectorSecurities(sectorName).Count;
        }

        public static List<Security> SectorSecurities(this IPortfolio portfolio, string sectorName)
        {
            var securities = new List<Security>();
            foreach (var sec in portfolio.Funds)
            {
                if (sec.IsSectorLinked(sectorName))
                {
                    securities.Add(sec);
                }
            }
            securities.Sort();
            return securities;
        }

        public static List<DayValue_Named> SectorInvestments(this IPortfolio portfolio, string company)
        {
            var output = new List<DayValue_Named>();
            foreach (var sec in portfolio.SectorSecurities(company))
            {
                output.AddRange(sec.AllInvestmentsNamed());
            }

            return output;
        }

        public static double SectorProfit(this IPortfolio portfolio, string sectorName)
        {
            var securities = portfolio.SectorSecurities(sectorName);
            double value = 0;
            foreach (var security in securities)
            {
                if (security.Any())
                {
                    value += security.LatestValue().Value - security.TotalInvestment();
                }
            }

            return value;
        }

        public static double SectorFraction(this IPortfolio portfolio, string sectorName, DateTime date)
        {
            return portfolio.SectorValue(sectorName, date) / portfolio.Value(date);
        }

        /// <summary>
        /// If possible, returns the IRR of all securities in the sector specified over the time period.
        /// </summary>
        public static double IRRSector(this IPortfolio portfolio, string sectorName, DateTime earlierTime, DateTime laterTime)
        {
            var securities = portfolio.SectorSecurities(sectorName);
            if (securities.Count == 0)
            {
                return double.NaN;
            }
            double earlierValue = 0;
            double laterValue = 0;
            var Investments = new List<DailyValuation>();

            foreach (var security in securities)
            {
                if (security.Any())
                {
                    earlierValue += security.NearestEarlierValuation(earlierTime).Value;
                    laterValue += security.NearestEarlierValuation(laterTime).Value;
                    Investments.AddRange(security.InvestmentsBetween(earlierTime, laterTime));
                }
            }

            return FinancialFunctions.IRRTime(new DailyValuation(earlierTime, earlierValue), Investments, new DailyValuation(laterTime, laterValue));
        }
    }
}
