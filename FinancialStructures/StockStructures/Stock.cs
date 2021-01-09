using System;
using System.Collections.Generic;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.StockStructures
{
    public partial class Stock : IStock
    {
        private List<StockDay> fValuations;
        internal int LastAccessedValuationIndex = 0;
        /// <summary>
        /// The name of the stock in question.
        /// </summary>
        public NameData Name
        {
            get;
            set;
        }

        /// <summary>
        /// Values associated to this stock in order earliest -> latest.
        /// </summary>
        public List<StockDay> Valuations
        {
            get
            {
                return fValuations;
            }
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
        public Stock(string company, string name, string url)
        {
            Name = new NameData(name.Trim(), company.Trim(), "", url.Trim());
            Valuations = new List<StockDay>();
        }

        /// <summary>
        /// Adds a value to the Stock. Note this does not sort the values, so <see cref="Sort"/> should be called after this.
        /// </summary>
        public void AddValue(DateTime time, double open, double high, double low, double close, double volume)
        {
            Valuations.Add(new StockDay(time, open, high, low, close, volume));
        }

        /// <summary>
        /// Sorts the values in the stock.
        /// </summary>
        public void Sort()
        {
            Valuations.Sort();
        }
    }
}
