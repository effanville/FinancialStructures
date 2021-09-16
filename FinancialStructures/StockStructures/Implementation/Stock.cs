using System;
using System.Collections.Generic;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.StockStructures.Implementation
{
    /// <summary>
    /// Simulates a Stock.
    /// </summary>
    public partial class Stock : IStock
    {
        private List<StockDay> fValuations;
        internal int LastValueIndex = 0;

        /// <inheritdoc/>
        public string Ticker
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public NameData Name
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public List<StockDay> Valuations
        {
            get => fValuations;
            set
            {
                fValuations = value;
                fValuations.Sort();
            }
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Stock()
        {
        }

        /// <summary>
        /// Constructor setting basic name information
        /// </summary>
        public Stock(string ticker, string company, string name, string url)
        {
            Ticker = ticker;
            Name = new NameData(name.Trim(), company.Trim(), "", url.Trim());
            Valuations = new List<StockDay>();
        }

        /// <inheritdoc/>
        public void AddValue(DateTime time, double open, double high, double low, double close, double volume)
        {
            Valuations.Add(new StockDay(time, open, high, low, close, volume));
        }

        /// <inheritdoc/>
        public void Sort()
        {
            Valuations.Sort();
        }
    }
}
