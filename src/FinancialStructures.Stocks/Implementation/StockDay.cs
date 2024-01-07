using System;

namespace FinancialStructures.Stocks.Implementation
{
    /// <summary>
    /// Class containing all data pertaining to a stock.
    /// </summary>
    public class StockDay : IComparable<StockDay>
    {
        /// <summary>
        /// The start time of the interval this data is about.
        /// </summary>
        public DateTime Start
        {
            get;
            set;
        }

        /// <summary>
        /// The length of time of the interval this data is about.
        /// </summary>
        public TimeSpan Duration
        {
            get;
            set;
        } = TimeSpan.FromHours(8.5);

        /// <summary>
        /// The ending time of the interval this data is about.
        /// </summary>
        public DateTime End => Start + Duration;

        /// <summary>
        /// The opening price in the interval.
        /// </summary>
        public decimal Open
        {
            get;
            set;
        }

        /// <summary>
        /// The high value in this interval
        /// </summary>
        public decimal High
        {
            get;
            set;
        }

        /// <summary>
        /// The low value in this interval.
        /// </summary>
        public decimal Low
        {
            get;
            set;
        }

        /// <summary>
        /// The closing value in this interval.
        /// </summary>
        public decimal Close
        {
            get;
            set;
        }

        /// <summary>
        /// The trading volume experienced in the interval.
        /// </summary>
        public decimal Volume
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor setting nothing.
        /// </summary>
        public StockDay()
        {
        }

        /// <summary>
        /// Constructor setting all values.
        /// </summary>
        public StockDay(DateTime time, decimal open, decimal high, decimal low, decimal close, decimal volume)
            : this()
        {
            Start = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        /// <summary>
        /// Constructor setting all values.
        /// </summary>
        public StockDay(DateTime time, decimal open, decimal high, decimal low, decimal close, decimal volume, TimeSpan duration)
            : this(time, open, high, low, close, volume)
        {
            Duration = duration;
        }

        /// <summary>
        /// Get the relevant value based on the datastream.
        /// </summary>
        public decimal Value(StockDataStream data)
        {
            return data switch
            {
                StockDataStream.Open => Open,
                StockDataStream.High => High,
                StockDataStream.Low => Low,
                StockDataStream.CloseOpen => Close / Open,
                StockDataStream.HighOpen => High / Open,
                StockDataStream.LowOpen => Low / Open,
                StockDataStream.Volume => Volume,
                _ => Close,
            };
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Start}-O{Open}-H{High}-L{Low}-C{Close}-V{Volume}";
        }

        /// <inheritdoc/>
        public int CompareTo(StockDay obj)
        {
            if (obj is StockDay otherPrice)
            {
                return Start.CompareTo(otherPrice.Start);
            }

            return 0;
        }

        /// <summary>
        /// Copy only the open value into a new StockDay.
        /// </summary>
        /// <returns></returns>
        public StockDay CopyAsOpenOnly()
        {
            return new StockDay(Start, Open, 0, 0, 0, 0, Duration);
        }
    }
}
