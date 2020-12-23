using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using StructureCommon.Extensions;

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
            switch (account)
            {
                case Account.Security:
                {
                    if (!portfolio.TryGetSecurity(name, out var security))
                    {
                        return;
                    }

                    Value = (security.LatestValue().Day - security.FirstValue().Day).Days / ((double)365 * security.Count());
                    return;
                }
                case Account.Benchmark:
                case Account.BankAccount:
                case Account.Currency:
                {
                    if (!portfolio.TryGetAccount(account, name, out var bankAcc))
                    {
                        return;
                    }

                    Value = ((bankAcc.LatestValue().Day - bankAcc.FirstValue().Day).Days) / ((double)365 * bankAcc.Count());
                    return;
                }
                default:
                    return;
            }

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
