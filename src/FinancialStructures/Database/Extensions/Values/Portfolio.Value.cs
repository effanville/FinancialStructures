﻿using System;

using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Extensions.Values
{
    public static partial class Values
    {
        /// <summary>
        /// Get the latest value of the selected element.
        /// </summary>
        /// <param name="portfolio">The portfolio to calculate values for</param>
        /// <param name="account">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <returns>The latest value if it exists.</returns>
        public static decimal LatestValue(this IPortfolio portfolio, Account account, TwoName name) 
            => portfolio.Value(account, name, DateTime.Today);

        /// <summary>
        /// Get the value of the selected element on the date provided. For a sector the name is only the surname
        /// </summary>
        /// <param name="portfolio">The portfolio to calculate values for</param>
        /// <param name="account">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <param name="date">The date on which to find the value.</param>
        /// <returns>The  value if it exists.</returns>
        public static decimal Value(this IPortfolio portfolio, Account account, TwoName name, DateTime date) 
            => portfolio.CalculateValue(
                account,
                name,
                ValueCalculator.DefaultCalculator(date),
                ValueCalculator.Calculators(portfolio, date),
                defaultValue: ValueCalculator.DefaultValue(account));
    }
}
