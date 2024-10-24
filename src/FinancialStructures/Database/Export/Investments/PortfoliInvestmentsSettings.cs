﻿using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Export.Investments
{
    /// <summary>
    /// Settings for generating portfolio investments <see cref="PortfolioInvestments"/>
    /// </summary>
    public sealed class PortfolioInvestmentSettings
    {
        /// <summary>
        /// The type of <see cref="Totals"/> to generate investments for.
        /// </summary>
        public Totals TotalsType
        {
            get;
            set;
        }

        /// <summary>
        /// Any name associated to the totals type.
        /// </summary>
        public TwoName Name
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PortfolioInvestmentSettings(Totals totalsType = Totals.Security, TwoName name = null)
        {
            TotalsType = totalsType;
            Name = name;
        }
    }
}
