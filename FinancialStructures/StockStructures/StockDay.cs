using System;

namespace FinancialStructures.StockStructures
{
    /// <summary>
    /// Class containing all data pertaining to a stock.
    /// </summary>
    public class StockDay : IComparable<StockDay>
    {
        /// <summary>
        /// The start time of the interval this data is about.
        /// </summary>
        public DateTime Time
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
        } = TimeSpan.FromDays(1);

        /// <summary>
        /// The opening price in the interval.
        /// </summary>
        public double Open
        {
            get;
            set;
        }

        /// <summary>
        /// The high value in this interval
        /// </summary>
        public double High
        {
            get;
            set;
        }

        /// <summary>
        /// The low value in this interval.
        /// </summary>
        public double Low
        {
            get;
            set;
        }

        /// <summary>
        /// The closing value in this interval.
        /// </summary>
        public double Close
        {
            get;
            set;
        }

        /// <summary>
        /// The trading volume experienced in the interval.
        /// </summary>
        public double Volume
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
        public StockDay(DateTime time, double open, double high, double low, double close, double volume)
        {
            Time = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }
        /// <summary>
        /// Get the relevant value based on the datastream.
        /// </summary>
        public double Value(StockDataStream data)
        {
            switch (data)
            {
                case (StockDataStream.Open):
                    return Open;
                case (StockDataStream.High):
                    return High;
                case (StockDataStream.Low):
                    return Low;
                case (StockDataStream.CloseOpen):
                    return Close / Open;
                case (StockDataStream.HighOpen):
                    return High / Open;
                case (StockDataStream.LowOpen):
                    return Low / Open;
                case StockDataStream.Volume:
                    return Volume;
                case (StockDataStream.Close):
                default:
                    return Close;
            }
        }


        /// <inheritdoc/>
        public int CompareTo(StockDay obj)
        {
            if (obj is StockDay otherPrice)
            {
                return Time.CompareTo(otherPrice.Time);
            }

            return 0;
        }
    }
}
