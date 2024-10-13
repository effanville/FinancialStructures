using System;
using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Extensions
{
    /// <summary>
    /// Contains extension methods specifically for a <see cref="IAmortisableAsset"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IAssetExtensions
    {
        public static decimal Debt(this IAmortisableAsset asset, DateTime time, ICurrency currency = null)
        {
            DailyValuation latestValue = asset.Debt.LatestValuation();
            decimal currencyValue = currency == null ? 1.0m : currency.Value(time)?.Value ?? 1.0m;
            return latestValue?.Value * currencyValue ?? 0.0m;
        }
    }
}