using System.Collections.Generic;

using Effanville.FinancialStructures.Database.Statistics;

namespace Effanville.FinancialStructures.Database.Export.Statistics
{
    /// <summary>
    /// A class containing settings for the export to file of a <see cref="PortfolioStatistics"/> object.
    /// </summary>
    public sealed class PortfolioStatisticsExportSettings
    {
        /// <summary>
        /// Display with spacing in tables.
        /// </summary>
        public bool Spacing
        {
            get;
        }

        /// <summary>
        /// Display with colours.
        /// </summary>
        public bool Colours
        {
            get;
        }

        /// <summary>
        /// Options on displaying Securities.
        /// </summary>
        public TableOptions<Statistic> SecurityDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying bank accounts.
        /// </summary>
        public TableOptions<Statistic> BankAccountDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying sectors.
        /// </summary>
        public TableOptions<Statistic> SectorDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Options on displaying assets.
        /// </summary>
        public TableOptions<Statistic> AssetDisplayOptions
        {
            get;
        }
        
        /// <summary>
        /// Options on displaying currencies.
        /// </summary>
        public TableOptions<Statistic> CurrencyDisplayOptions
        {
            get;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PortfolioStatisticsExportSettings(
            bool spacing,
            bool colours,
            bool includeSecurities,
            Statistic securitySortField,
            SortDirection securitySortDirection,
            IReadOnlyList<Statistic> securityDisplayFields,
            bool includeBankAccounts,
            Statistic bankAccSortField,
            SortDirection bankAccSortDirection,
            IReadOnlyList<Statistic> bankAccDisplayFields,
            bool includeSectors,
            Statistic sectorSortField,
            SortDirection sectorSortDirection,
            IReadOnlyList<Statistic> sectorDisplayFields,
            bool includeAssets,
            Statistic assetSortField,
            SortDirection assetSortDirection,
            IReadOnlyList<Statistic> assetDisplayFields,
            bool includeCurrencies,
            Statistic currencySortField,
            SortDirection currencySortDirection,
            IReadOnlyList<Statistic> currencyDisplayFields)
        {
            Spacing = spacing;
            Colours = colours;
            SecurityDisplayOptions = new TableOptions<Statistic>(includeSecurities, securitySortField, securitySortDirection, securityDisplayFields);
            BankAccountDisplayOptions = new TableOptions<Statistic>(includeBankAccounts, bankAccSortField, bankAccSortDirection, bankAccDisplayFields);
            SectorDisplayOptions = new TableOptions<Statistic>(includeSectors, sectorSortField, sectorSortDirection, sectorDisplayFields);
            AssetDisplayOptions = new TableOptions<Statistic>(includeAssets, assetSortField, assetSortDirection, assetDisplayFields);
            CurrencyDisplayOptions = new TableOptions<Statistic>(includeCurrencies, currencySortField, currencySortDirection, currencyDisplayFields);
        }

        /// <summary>
        /// Creates default settings.
        /// </summary>
        /// <returns></returns>
        public static PortfolioStatisticsExportSettings DefaultSettings() 
            => new(
                spacing: false,
                colours: false,
                includeSecurities: true,
                Statistic.Company,
                SortDirection.Ascending,
                AccountStatisticsHelpers.DefaultSecurityStats(),
                includeBankAccounts: true,
                Statistic.Company,
                SortDirection.Ascending,
                AccountStatisticsHelpers.DefaultBankAccountStats(),
                includeSectors: true,
                Statistic.Company,
                SortDirection.Ascending,
                AccountStatisticsHelpers.DefaultSectorStats(),
                includeAssets: true,
                Statistic.Company,
                SortDirection.Ascending,
                AccountStatisticsHelpers.DefaultAssetStats(),
                includeCurrencies: true,
                Statistic.Company,
                SortDirection.Ascending,
                AccountStatisticsHelpers.DefaultCurrencyStats());
    }
}
