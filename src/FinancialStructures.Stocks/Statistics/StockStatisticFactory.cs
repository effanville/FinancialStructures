using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Effanville.FinancialStructures.Stocks.Statistics
{
    /// <summary>
    /// Generator for various statistic calculators for a stock.
    /// </summary>
    public class StockStatisticFactory
    {
        private List<Type> _admissibleStockStatistics;

        private List<Type> AdmissibleStockStatistics
        {
            get
            {
                if (_admissibleStockStatistics == null)
                {
                    Type type = typeof(IStockStatistic);
                    _admissibleStockStatistics = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                        .ToList();
                }

                return _admissibleStockStatistics;
            }
        }
        
        public IStockStatistic Create(StockStatisticSettings settings)
        {
            Type stockType = AdmissibleStockStatistics.FirstOrDefault(x => x.Name == settings.StatType);
            ConstructorInfo ctor = stockType.GetConstructor(new[] { typeof(StockStatisticSettings) });
            if (ctor == null)
            {
                return null;
            }

            object instance = ctor.Invoke(new object[] { settings });
            return instance as IStockStatistic;
        }
    }
}
