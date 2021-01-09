using System;
using System.Collections.Generic;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.StockStructures
{
    /// <summary>
    /// The contract for a Stock.
    /// </summary>
    public interface IStock
    {
        NameData Name
        {
            get;
            set;
        }

        List<StockDay> Valuations
        {
            get;
            set;
        }

        void AddValue(DateTime time, double open, double high, double low, double close, double volume);

        void Sort();

        double Value(DateTime date, StockDataStream data = StockDataStream.Close);

        double Value(int index, StockDataStream data = StockDataStream.Close);

        List<double> Values(DateTime date, int numberValuesBefore, int numberValuesAfter = 0, StockDataStream data = StockDataStream.Close);

        DateTime EarliestTime();

        DateTime LastTime();
    }
}
