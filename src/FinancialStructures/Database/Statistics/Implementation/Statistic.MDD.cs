using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.MathLibrary.Finance;
using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticMDD : StatisticBase
    {
        internal StatisticMDD()
            : base(Statistic.MDD)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            DateTime firstDate = valueList.FirstValue()?.Day ?? DateTime.MaxValue;
            DateTime lastDate = valueList.LatestValue()?.Day ?? DateTime.MinValue;
            Value = Calculate(valueList, firstDate, lastDate);
        }
        
        double Calculate(IValueList valueList, DateTime earlierTime, DateTime laterTime)
        {
            List<DailyValuation> values = valueList.ListOfValues()
                .Where(value => value.Day >= earlierTime 
                                && value.Day <= laterTime && !value.Value.Equals(0.0m)).ToList();
            decimal dd = FinanceFunctions.MDD(values);
            if(dd == decimal.MaxValue)
            {
                return 0.0;
            }

            return (double)dd;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.TotalMDD(total, name);
        }
    }
}
