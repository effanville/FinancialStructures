using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using StructureCommon.FinanceFunctions;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        public static DateTime SectorFirstDate(this IPortfolio portfolio, string sector)
        {
            var output = DateTime.Today;
            foreach (ISecurity sec in portfolio.SectorSecurities(sector))
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
                foreach (ISecurity fund in portfolio.Funds)
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

        public static List<ISecurity> SectorSecurities(this IPortfolio portfolio, string sectorName)
        {
            var securities = new List<ISecurity>();
            foreach (ISecurity security in portfolio.Funds)
            {
                if (security.IsSectorLinked(sectorName))
                {
                    securities.Add(security);
                }
            }
            securities.Sort();
            return securities;
        }

        public static List<DayValue_Named> SectorInvestments(this IPortfolio portfolio, string sectorName)
        {
            var output = new List<DayValue_Named>();
            foreach (ISecurity security in portfolio.SectorSecurities(sectorName))
            {
                output.AddRange(security.AllInvestmentsNamed());
            }

            return output;
        }

        public static double SectorProfit(this IPortfolio portfolio, string sectorName)
        {
            double value = 0;
            foreach (ISecurity security in portfolio.SectorSecurities(sectorName))
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
