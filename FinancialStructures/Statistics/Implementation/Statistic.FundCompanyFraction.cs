using System;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    /// <summary>
    /// Statistic for the <see cref="Statistic.FundCompanyFraction"/> enum value. This
    /// calculates the fraction held in an account out of the company for that
    /// account.
    /// </summary>
    internal class StatisticFundCompanyFraction : StatisticBase
    {
        /// <summary>
        /// Default constructor setting relevant Statistic.
        /// </summary>
        internal StatisticFundCompanyFraction()
            : base(Statistic.FundCompanyFraction)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Totals companyTotals = account.ToTotals().ToCompanyTotal();
            Value = portfolio.Fraction(companyTotals, account, name, DateTime.Today);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            if (total == Totals.Company || total == Totals.SecurityCompany || total == Totals.All)
            {
                Value = 1;
            }
        }
    }
}
