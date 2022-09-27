using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Database.Statistics;

namespace FinancialStructures.Database.Export.Statistics
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
        /// Default constructor.
        /// </summary>
        public PortfolioStatisticsExportSettings(
            bool spacing,
            bool colours,
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
            List<Statistic> sectorDisplayFields,
            bool includeAssets,
            Statistic assetSortField,
            SortDirection assetSortDirection,
            List<Statistic> assetDisplayFields)
        {
            Spacing = spacing;
            Colours = colours;
            SecurityDisplayOptions = new TableOptions<Statistic>(includeSecurities, securitySortField, securitySortDirection, securityDisplayFields);
            BankAccountDisplayOptions = new TableOptions<Statistic>(includeBankAccounts, bankAccSortField, bankAccSortDirection, bankAccDisplayFields);
            SectorDisplayOptions = new TableOptions<Statistic>(includeSectors, sectorSortField, sectorSortDirection, sectorDisplayFields);
            AssetDisplayOptions = new TableOptions<Statistic>(includeAssets, assetSortField, assetSortDirection, assetDisplayFields);
        }

        /// <summary>
        /// Creates default settings.
        /// </summary>
        /// <returns></returns>
        public static PortfolioStatisticsExportSettings DefaultSettings()
        {
            return new PortfolioStatisticsExportSettings(
                spacing: false,
                colours: false,
                includeSecurities: true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultSecurityStats().ToList(),
                includeBankAccounts: true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                includeSectors: true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultSectorStats().ToList(),
                includeAssets: false,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultAssetStats().ToList());
        }
    }
}
