using System;
using System.Collections.Generic;
using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticSectors : IStatistic
    {
        /// <inheritdoc/>
        public Statistic StatType => Statistic.Sectors;

        /// <inheritdoc/>
        public double Value
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public string StringValue
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public bool IsNumeric => false;

        /// <inheritdoc/>
        public object ValueAsObject => IsNumeric ? Value : StringValue;

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            switch (valueList.AccountType)
            {
                default:
                {
                    StringValue = valueList.Names.SectorsFlat;
                    return;
                }
                case Account.All:
                case Account.Benchmark:
                    return;
            }
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            string identifier = total.GetIdentifier(name);
            IReadOnlyList<IValueList> accounts = portfolio.Accounts(total, identifier);
            HashSet<string> sectors = new HashSet<string>();

            foreach (IValueList account in accounts)
            {
                DailyValuation value = account.Value(date);
                if (value != null && value.Value > 0.0m)
                {
                    sectors.UnionWith(account.Names.Sectors);
                }
            }

            StringValue = NameData.FlattenSectors(sectors).Replace(",", ", ");
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other) => Value.CompareTo(other.Value);

        /// <inheritdoc/>
        public override string ToString() => StringValue;
    }
}
