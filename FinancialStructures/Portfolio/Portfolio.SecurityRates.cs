using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        /// <summary>
        /// returns the currency associated to the security.
        /// </summary>
        private static Currency SecurityCurrency(Portfolio portfolio, Security security)
        {
            var currencyName = security.GetCurrency();
            return portfolio.Currencies.Find(cur => cur.Name == currencyName);
        }

        /// <summary>
        /// returns the latest value of the security, or nan if it doesnt exist.
        /// </summary>
        public static double SecurityLatestValue(this Portfolio portfolio, string company, string name)
        {
            return portfolio.SecurityValue(company, name, DateTime.Today);
        }

        /// <summary>
        /// returns the value of the security on the date specified, or nan if it doesnt exist.
        /// </summary>
        public static double SecurityValue(this Portfolio portfolio, string company, string name, DateTime date)
        {
            if (!portfolio.TryGetSecurity(company, name, out Security desired) || !desired.Any())
            {
                return double.NaN;
            }
            var currency = SecurityCurrency(portfolio, desired);
            return desired.Value(date, currency).Value;
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="company"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double RecentChange(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = SecurityCurrency(portfolio, desired);
                    var needed = desired.LatestValue(currency);
                    return needed.Value - desired.LastEarlierValuation(needed.Day, currency).Value;
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static double Profit(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = SecurityCurrency(portfolio, desired);
                    return desired.LatestValue(currency).Value - desired.TotalInvestment(currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Returns the fraction of investments in the security out of the portfolio.
        /// </summary>
        public static double SecurityFraction(this Portfolio portfolio, string company, string name, DateTime date)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = SecurityCurrency(portfolio, desired);
                    return desired.Value(date, currency).Value / portfolio.AllSecuritiesValue(date);
                }
            }

            return double.NaN;
        }


        /// <summary>
        /// returns the fraction a security has out of its company.
        /// </summary>
        public static double SecurityCompanyFraction(this Portfolio portfolio, string company, string name, DateTime date)
        {
            return portfolio.SecurityFraction(company, name, date) / portfolio.CompanyFraction(company, date);
        }

        /// <summary>
        /// Returns the investments in the security.
        /// </summary>
        private static List<DayValue_Named> SecurityInvestments(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = SecurityCurrency(portfolio, desired);
                    return desired.AllInvestmentsNamed(currency);
                }
            }

            return null;
        }

        /// <summary>
        /// If possible, returns the CAR of the security specified.
        /// </summary>
        public static double CAR(this Portfolio portfolio, string company, string name, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = SecurityCurrency(portfolio, desired);
                    return desired.CAR(earlierTime, laterTime, currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// If possible, returns the IRR of the security specified.
        /// </summary>
        public static double IRR(this Portfolio portfolio, string company, string name)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = SecurityCurrency(portfolio, desired);
                    return desired.IRR(currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// If possible, returns the IRR of the security specified over the time period.
        /// </summary>
        public static double IRR(this Portfolio portfolio, string company, string name, DateTime earlierTime, DateTime laterTime)
        {
            if (portfolio.TryGetSecurity(company, name, out Security desired))
            {
                if (desired.Any())
                {
                    var currency = SecurityCurrency(portfolio, desired);
                    return desired.IRRTime(earlierTime, laterTime, currency);
                }
            }

            return double.NaN;
        }
    }
}
