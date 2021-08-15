using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.DataStructures;

namespace FinancePortfolioDatabase.GUI
{
    public sealed class DisplayConstants
    {
        public static List<TradeType> TradeTypes => Enum.GetValues(typeof(TradeType)).Cast<TradeType>().ToList();

        public DisplayConstants()
        {
        }
    }
}
