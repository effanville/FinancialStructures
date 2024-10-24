﻿using System;

using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
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
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            Totals companyTotals = account.ToTotals().ToCompanyTotal();
            Value = portfolio.Fraction(companyTotals, account, name, date);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            if (total == Totals.Company || total == Totals.SecurityCompany || total == Totals.All)
            {
                Value = 1;
            }
        }
    }
}
