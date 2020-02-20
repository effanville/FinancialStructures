using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.DataStructures
{
    public partial class TimeList
    {
        /// <summary>
        /// Returns linearly interpolated value of the List on the date provided.
        /// </summary>
        internal DayValue Value(DateTime date)
        {
            if (fValues == null)
            {
                return new DayValue(DateTime.Today, 1.0);
            }
            if (Count() == 1)
            {
                return fValues[0].Copy();
            }
            if (date < FirstDate())
            {
                return FirstValuation();
            }
            if (date >= LatestDate())
            {
                return LatestValuation();
            }

            var earlier = NearestEarlierValue(date);
            var later = NearestLaterValue(date);

            double value = earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (date - earlier.Day).Days;
            return new DayValue(date, value);
        }

        /// <summary>
        /// Returns linearly interpolated value of the List on the date provided.
        /// </summary>
        internal DayValue ValueZeroBefore(DateTime date)
        {
            if (fValues == null)
            {
                return new DayValue(DateTime.Today, 1.0);
            }

            if (date < FirstDate())
            {
                return new DayValue(date, 0.0);
            }
            if (date >= LatestDate())
            {
                return LatestValuation();
            }
            if (Count() == 1)
            {
                return fValues[0].Copy();
            }

            var earlier = NearestEarlierValue(date);
            var later = NearestLaterValue(date);

            double value = earlier.Value + (later.Value - earlier.Value) / (later.Day - earlier.Day).Days * (date - earlier.Day).Days;
            return new DayValue(date, value);
        }

        /// <summary>
        /// Returns the DailyValuation on or before the date specified.
        /// </summary>
        internal DayValue NearestEarlierValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (date < FirstDate())
                {
                    return new DayValue(date, 0.0);
                }

                if (Count() == 1)
                {
                    return fValues[0].Copy();
                }

                if (date > LatestDate())
                {
                    return LatestValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means 
                // the first value later.
                for (int i = Count() - 1; i > -1; i--)
                {
                    if (date > fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                    if (date == fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                }
            }

            return new DayValue(date, 0.0);
        }

        /// <summary>
        /// Returns DailyValuation closest to the date but earlier to it. 
        /// If a strictly earlier one cannot be found then return null.
        /// </summary>
        internal DayValue RecentPreviousValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                // Some cases can return early.
                if (Count() == 1 || date <= FirstDate())
                {
                    return new DayValue(date, 0.0);
                }

                if (date > LatestDate())
                {
                    return LatestValuation();
                }

                // go back in time until find a valuation that is after the date we want
                // Then the value we want is the previous in the vector.
                for (int i = Count() - 1; i > 0; i--)
                {
                    if (date == fValues[i].Day)
                    {
                        return fValues[i - 1].Copy();
                    }
                    if (date > fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                }
            }

            return new DayValue(date, 0.0);
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        internal bool TryGetNearestEarlierValue(DateTime date, out DayValue value)
        {
            value = NearestEarlierValue(date);
            if (value == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        internal DayValue NearestLaterValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (Count() == 1)
                {
                    return fValues[0].Copy();
                }

                if (date > LatestDate())
                {
                    return null;
                }

                if (date < FirstDate())
                {
                    return FirstValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means 
                // the first value later.
                for (int i = 0; i < Count(); i++)
                {
                    if (date < fValues[i].Day)
                    {
                        return fValues[i].Copy();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        internal DayValue NearestValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (Count() == 1)
                {
                    return fValues[0].Copy();
                }

                if (date > LatestDate())
                {
                    return LatestValuation();
                }

                if (date < FirstDate())
                {
                    return FirstValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means 
                // the first value later.
                for (int i = 0; i < Count(); i++)
                {
                    if (date < fValues[i].Day)
                    {
                        if (fValues[i].Day - date < date - fValues[i - 1].Day)
                        {
                            return fValues[i].Copy();
                        }

                        return fValues[i - 1].Copy();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first date held in the vector, or default if cannot find any data
        /// </summary>
        internal DateTime FirstDate()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[0].Copy().Day;
            }

            return new DateTime();
        }

        /// <summary>
        /// Returns first value held, or 0 if no data.
        /// </summary>
        internal double FirstValue()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[0].Copy().Value;
            }
            return 0.0;
        }

        /// <summary>
        /// Returns first pair of date and value, or null if this doesn't exist.
        /// </summary>
        /// <returns></returns>
        internal DayValue FirstValuation()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[0].Copy();
            }

            return null;
        }

        /// <summary>
        /// Returns latest date held, or default if no data.
        /// </summary>
        internal DateTime LatestDate()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[fValues.Count() - 1].Copy().Day;
            }
            return new DateTime();
        }

        /// <summary>
        /// Returns latest value, or 0 if no data held.
        /// </summary>
        internal double LatestValue()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[fValues.Count() - 1].Copy().Value;
            }

            return 0.0;
        }

        /// <summary>
        /// Returns a pair of date and value of the most recently held data, or null if no data held.
        /// </summary>
        internal DayValue LatestValuation()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[fValues.Count() - 1].Copy();
            }

            return null;
        }

        /// <summary>
        /// returns all valuations on or between the two dates specified, or empty list if none held.
        /// </summary>
        internal List<DayValue> GetValuesBetween(DateTime earlierTime, DateTime laterTime)
        {
            var valuesBetween = new List<DayValue>();

            foreach (DayValue value in fValues)
            {
                if (value.Day >= earlierTime && value.Day <= laterTime)
                {
                    valuesBetween.Add(value.Copy());
                }
            }

            return valuesBetween;
        }
    }
}
