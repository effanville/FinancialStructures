using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticSectors : IStatistic
    {
        internal StatisticSectors()
        {
            StatType = Statistic.Sectors;
        }

        /// <inheritdoc/>
        public Statistic StatType
        {
            get;
        }

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
        public void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            switch (account)
            {
                case Account.Security:
                case Account.BankAccount:
                case Account.Currency:
                {
                    if (!portfolio.TryGetAccount(account, name, out IValueList desired))
                    {
                        return;
                    }

                    StringValue = desired.Names.SectorsFlat;
                    return;
                }
                default:
                case Account.All:
                case Account.Benchmark:
                    return;
            }
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            var accounts = portfolio.Accounts(total, name);
            HashSet<string> sectors = new HashSet<string>();

            foreach (var account in accounts)
            {
                sectors.UnionWith(account.Names.Sectors);
            }

            StringValue = NameData.FlattenSectors(sectors);
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return StringValue;
        }
    }
}
