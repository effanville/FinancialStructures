﻿using FinancialStructures.FinanceFunctionsList;
using System;
using System.Collections.Generic;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    public partial class TimeList
    {
        public TimeList Inverted()
        {
            var invertedValues = new List<DailyValuation>();
            if (fValues != null && fValues.Count > 0)
            {
                foreach (var value in fValues)
                {
                    invertedValues.Add(new DailyValuation(value.Day, 1 / value.Value));
                }
            }

            return new TimeList(invertedValues);
        }

        /// <summary>
        /// Adds all values in the list.
        /// </summary>
        public double Sum()
        {
            if (fValues != null && fValues.Count > 0)
            {
                double sum = 0;
                foreach (var val in fValues)
                {
                    sum += val.Value;
                }

                return sum;
            }

            return double.NaN;
        }

        /// <summary>
        /// returns the CAR of the timelist between the dates provided.
        /// </summary>
        public double CAR(DateTime earlierTime, DateTime laterTime)
        {
            var earlierValue = NearestEarlierValue(earlierTime);
            var laterValue = NearestEarlierValue(laterTime);
            if (earlierValue == null || laterValue == null)
            {
                return double.NaN;
            }

            return FinancialFunctions.CAR(earlierValue, laterValue);
        }
        /// <summary>
        /// Returns internal rate of return of the values in the TimeList
        /// </summary>
        internal double IRR(DailyValuation latestValue)
        {
            // if have only one investment easy to return the CAR.
            if (Count() == 1)
            {
                return FinancialFunctions.CAR(latestValue, FirstValuation());
            }

            return FinancialFunctions.IRR(fValues, latestValue);
        }

        /// <summary>
        /// Returns the internal rate of return between <param name="latestValue"/> and <param name="startValue"/>
        /// </summary>
        internal double IRRTime(DailyValuation startValue, DailyValuation latestValue)
        {
            if (startValue == null || latestValue == null)
            {
                return double.NaN;
            }

            // if have only one investment easy to return the CAR.
            if (Count() == 1)
            {
                return FinancialFunctions.CAR(latestValue, startValue);
            }

            return FinancialFunctions.IRRTime(startValue, GetValuesBetween(startValue.Day, latestValue.Day), latestValue);
        }
    }
}
