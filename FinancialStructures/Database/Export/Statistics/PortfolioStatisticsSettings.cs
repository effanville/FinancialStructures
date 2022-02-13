using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Database.Statistics;

namespace FinancialStructures.Database.Export.Statistics
{
    /// <summary>
    /// Class containing all settings for creation of a <see cref="PortfolioStatistics"/> object.
    /// </summary>
    public sealed class PortfolioStatisticsSettings
    {
        /// <summary>
        /// The date to calculate statistics upon.
        /// </summary>
        public DateTime DateToCalculate
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
        /// Should benchmarks be included in the sector table.
        /// </summary>
        public bool GenerateBenchmarks
        {
            get;
            set;
        }

        /// <summary>
        /// Options on displaying Securities.
        /// </summary>
        public GenerateOptions<Statistic> SecurityGenerateOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying bank accounts.
        /// </summary>
        public GenerateOptions<Statistic> BankAccountGenerateOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying sectors.
        /// </summary>
        public GenerateOptions<Statistic> SectorGenerateOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying assets.
        /// </summary>
        public GenerateOptions<Statistic> AssetGenerateOptions
        {
            get;
        }

        /// <summary>
        /// Constructor setting all values.
        /// </summary>
        public PortfolioStatisticsSettings(
            DateTime dateToCalculate,
            bool displayValueFunds,
            bool generateBenchmarks,
            bool includeSecurities,
            IReadOnlyList<Statistic> securityDisplayFields,
            bool includeBankAccounts,
            IReadOnlyList<Statistic> bankAccDisplayFields,
            bool includeSectors,
            IReadOnlyList<Statistic> sectorDisplayFields,
            bool includeAssets,
            IReadOnlyList<Statistic> assetDisplayFields)
        {
            DateToCalculate = dateToCalculate;
            DisplayValueFunds = displayValueFunds;
            GenerateBenchmarks = generateBenchmarks;
            SecurityGenerateOptions = new GenerateOptions<Statistic>(includeSecurities, securityDisplayFields);
            BankAccountGenerateOptions = new GenerateOptions<Statistic>(includeBankAccounts, bankAccDisplayFields);
            SectorGenerateOptions = new GenerateOptions<Statistic>(includeSectors, sectorDisplayFields);
            AssetGenerateOptions = new GenerateOptions<Statistic>(includeAssets, assetDisplayFields);
        }

        /// <summary>
        /// Constructor populating with default settings.
        /// </summary>
        public static PortfolioStatisticsSettings DefaultSettings()
        {
            return new PortfolioStatisticsSettings(
                DateTime.Today,
                displayValueFunds: false,
                generateBenchmarks: true,
                includeSecurities: true,
                securityDisplayFields: AccountStatisticsHelpers.DefaultSecurityStats().ToList(),
                includeBankAccounts: true,
                bankAccDisplayFields: AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                includeSectors: true,
                sectorDisplayFields: AccountStatisticsHelpers.DefaultSectorStats().ToList(),
                includeAssets: false,
                assetDisplayFields: AccountStatisticsHelpers.DefaultAssetStats().ToList());
        }
    }
}
