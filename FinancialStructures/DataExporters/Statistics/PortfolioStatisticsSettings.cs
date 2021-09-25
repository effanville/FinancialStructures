﻿using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Statistics;

namespace FinancialStructures.DataExporters.Statistics
{
    /// <summary>
    /// Class containing all settings for creation of a <see cref="PortfolioStatistics"/> object.
    /// </summary>
    public sealed class PortfolioStatisticsSettings
    {
        /// <summary>
        /// Should benchmarks be included in the sector table.
        /// </summary>
        public bool IncludeBenchmarks
        {
            get;
            set;
        }

        /// <summary>
        /// Only display accounts that have non zero current value.
        /// </summary>
        public bool DisplayValueFunds
        {
            get;
            set;
        }

        /// <summary>
        /// Options on displaying Securities.
        /// </summary>
        public StatisticTableOptions SecurityDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying bank accounts.
        /// </summary>
        public StatisticTableOptions BankAccountDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying sectors.
        /// </summary>
        public StatisticTableOptions SectorDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Constructor setting all values.
        /// </summary>
        public PortfolioStatisticsSettings(
            bool includeBenchmarks,
            bool displayValueFunds,
            bool includeSecurities,
            Statistic securitySortField,
            SortDirection securitySortDirection,
            List<Statistic> securityDisplayFields,
            bool includeBankAccounts,
            Statistic bankAccSortField,
            SortDirection bankAccSortDirection,
            List<Statistic> bankAccDisplayFields,
            bool includeSectors,
            Statistic sectorSortField,
            SortDirection sectorSortDirection,
            List<Statistic> sectorDisplayFields)
        {
            IncludeBenchmarks = includeBenchmarks;
            DisplayValueFunds = displayValueFunds;
            SecurityDisplayOptions = new StatisticTableOptions(includeSecurities, securitySortField, securitySortDirection, securityDisplayFields);
            BankAccountDisplayOptions = new StatisticTableOptions(includeBankAccounts, bankAccSortField, bankAccSortDirection, bankAccDisplayFields);
            SectorDisplayOptions = new StatisticTableOptions(includeSectors, sectorSortField, sectorSortDirection, sectorDisplayFields);
        }

        /// <summary>
        /// Constructor populating with default settings.
        /// </summary>
        public static PortfolioStatisticsSettings DefaultSettings()
        {
            return new PortfolioStatisticsSettings(
                includeBenchmarks: true,
                displayValueFunds: true,
                includeSecurities: true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.AllStatistics().ToList(),
                true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultSectorStats().ToList());
        }
    }
}
