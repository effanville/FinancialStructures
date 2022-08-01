using FinancialStructures.DataStructures;

namespace FinancialStructures.Database.Extensions
{
    /// <summary>
    /// Contains extension methods for migrating data from old to new.
    /// </summary>
    public static class DataMigrations
    {
        /// <summary>
        /// Converts all security trades of type <see cref="TradeType.ShareReprice"/> to the type
        /// <see cref="TradeType.ShareReset"/>.
        /// </summary>
        public static void MigrateRepriceToReset(this IPortfolio portfolio)
        {
            var securities = portfolio.FundsThreadSafe;
            foreach (var security in securities)
            {
                security.MigrateRepriceToReset();
            }
        }
    }
}
