using System;
using System.Collections.Generic;
using System.Linq;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.Download;

internal static class ValueListValidationExtensions
{
    internal static void UpdateAndCheck(this IValueList valueList, decimal valueToUpdate, IReportLogger logger, List<DownloadResult> results)
    {
        decimal latestValue = valueList.LatestValue()?.Value ?? 0.0m;
        valueList.SetData(DateTime.Today, valueToUpdate);

        DownloadResult result = results.FirstOrDefault(x => x.Name.IsEqualTo(valueList.Names));
        if (result != null)
        {
            result.Value = valueToUpdate;
            result.Success = true;
        }
        decimal newLatestValue = valueList.LatestValue()?.Value ?? 0.0m;
        if (newLatestValue == 0.0m || latestValue == 0.0m)
        {
            return;
        }

        decimal scaleFactor = latestValue / newLatestValue;
        if (scaleFactor > 50 || scaleFactor < 0.02m)
        {
            logger.Warn(nameof(ValueListValidationExtensions),
                $"Account {valueList.Names} has large change in value from {latestValue} to {newLatestValue}.");
        }
    }
}
