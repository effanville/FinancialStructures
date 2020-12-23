﻿using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
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
        public bool IsNumeric
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public object ValueAsObject
        {
            get
            {
                return IsNumeric ? (object)Value : (object)StringValue;
            }
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            switch (account)
            {
                case Account.Security:
                {
                    if (!portfolio.TryGetSecurity(name, out var desired))
                    {
                        return;
                    }

                    StringValue = desired.Names.SectorsFlat;
                    return;
                }
                case Account.BankAccount:
                case Account.Currency:
                {
                    if (!portfolio.TryGetAccount(account, name, out var desired))
                    {
                        return;
                    }

                    StringValue = desired.Names.SectorsFlat;
                    return;
                }
                default:
                    return;
            }
        }

        /// <inheritdoc/>
        public void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = 0.0;
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
