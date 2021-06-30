using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using Common.Structure.Extensions;

namespace FinancialStructures.Statistics
{
    internal class StatisticEntryYearDensity : StatisticBase
    {
        internal StatisticEntryYearDensity()
            : base(Statistic.EntryYearDensity)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out var bankAcc))
            {
                return;
            }

            Value = ((bankAcc.LatestValue().Day - bankAcc.FirstValue().Day).Days) / ((double)365 * bankAcc.Count());
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = (portfolio.LatestDate(total, name.Company) - portfolio.FirstValueDate(total, name.Company)).Days / ((double)365 * portfolio.EntryDistribution(total, name).Count());
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsNumeric ? Value.TruncateToString(4) : StringValue;
        }
    }
}
