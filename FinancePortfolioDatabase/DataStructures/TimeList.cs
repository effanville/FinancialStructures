﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    /// <summary>
    /// Sorted list of values, with last value the most recent, and first the oldest.
    /// </summary>
    /// <remarks>This list is sorted, with oldest value the first and latest the last.</remarks>
    public partial class TimeList
    {

        /// <summary>
        /// Collection of data within the TimeList.
        /// </summary>
        private List<DailyValuation> fValues;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public List<DailyValuation> Values
        {
            get { return fValues; }
            set { fValues = value; }
        }

        public DailyValuation this[int index]
        {
            get { return new DailyValuation(fValues[index]); }
        }

        /// <summary>
        /// Constructor adding values.
        /// </summary>
        /// <remarks>
        /// For testing only.
        /// </remarks>
        internal TimeList(List<DailyValuation> values)
        {
            fValues = values;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TimeList()
        {
            fValues = new List<DailyValuation>();
        }

        /// <summary>
        /// Returns true if contains any entries. 
        /// </summary>
        public bool Any()
        {
            return fValues != null && fValues.Any();
        }

        /// <summary>
        /// Returns the number of valuations in the timelist.
        /// </summary>
        public int Count()
        {
            return fValues.Count;
        }

        /// <summary>
        /// Adds value to the data.
        /// </summary>
        private void AddData(DateTime date, double value)
        {
            var valuation = new DailyValuation(date, value);
            fValues.Add(valuation);
            Sort();
        }

        /// <summary>
        /// Orders the list according to date.
        /// </summary>
        private void Sort()
        {
            if (fValues != null && fValues.Any())
            {
                fValues = fValues.OrderBy(x => x.Day).ToList();
            }
        }

        /// <summary>
        /// Checks if value on <param name="date"/> exists. If exists then index is output.
        /// </summary>
        internal bool ValueExists(DateTime date, out int index)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        index = i;
                        return true;
                    }
                }
            }

            index = -1;
            return false;
        }

        /// <summary>
        /// Adds value to the data only if value of the date doesn't currently exist.
        /// </summary>
        internal bool TryAddValue(DateTime date, double value)
        {    
            for (int i = 0; i < fValues.Count; i++)  
            {  
                if (fValues[i].Day == date)
                {
                    return false;
                }
            }

            AddData(date, value);

            return true;
        }

        /// <summary>
        /// Edits data on <paramref name="date"/> and replaces existing value with <paramref name="value"/>.
        /// </summary>
        internal bool TryEditData(DateTime date, double value)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        fValues[i].Value = value;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Edits the data on date specified. If data doesn't exist then adds the data.
        /// </summary>
        internal void TryEditDataOtherwiseAdd(DateTime date, double value)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        if (value == 0)
                        {
                            fValues.RemoveAt(i);
                            return;
                        }

                        fValues[i].Value = value;
                        return;
                    }
                }

                AddData(date, value);
            }
        }

        /// <summary>
        /// Deletes data if exists. If deletes, returns true.
        /// </summary>
        internal bool TryDeleteValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {

                        fValues.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// obtains first instance of the value for the date requested. Returns false if no data.
        /// </summary>
        internal bool TryGetValue(DateTime date, out double value)
        {
            value = 0;
            if (fValues != null && fValues.Any())
            {
                for (int i = 0; i < fValues.Count; i++)
                {
                    if (fValues[i].Day == date)
                    {
                        value = fValues[i].Value;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the DailyValuation on or before the date specified.
        /// </summary>
        internal DailyValuation GetNearestEarlierValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (date < GetFirstDate())
                {
                    return null;
                }

                if (Count() == 1)
                {
                    return fValues[0];
                }

                if (date > GetLatestDate())
                {
                    return GetLatestValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means 
                // the first value later.
                for (int i = Count() - 1; i > -1; i--)
                {
                    if (date > fValues[i].Day)
                    {
                        return fValues[i];
                    }
                    if (date == fValues[i].Day)
                    {
                        return fValues[i];
                    }
                }
            }
            
            return null;
        }

        /// <summary>
        /// Returns DailyValuation closest to the date but earlier to it. 
        /// If a strictly earlier one cannot be found then return null.
        /// </summary>
        internal DailyValuation GetLastEarlierValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                // Some cases can return early.
                if (Count() == 1 || date <= GetFirstDate())
                {
                    return null;
                }

                if (date > GetLatestDate())
                {
                    return GetLatestValuation();
                }

                // go back in time until find a valuation that is after the date we want
                // Then the value we want is the previous in the vector.
                for (int i = Count() - 1; i > 0; i--)
                {
                    if (date == fValues[i].Day)
                    {
                        return fValues[i-1];
                    }
                    if (date > fValues[i].Day)
                    {
                        return fValues[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        internal bool  TryGetNearestEarlierValue(DateTime date, out DailyValuation value)
        {
            value = GetNearestEarlierValue(date);
            if (value == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        internal DailyValuation GetNearestLaterValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (Count() == 1)
                {
                    return fValues[0];
                }

                if (date > GetLatestDate())
                {
                    return null;
                }

                if (date < GetFirstDate())
                {
                    return GetFirstValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means 
                // the first value later.
                for (int i = 0; i < Count(); i++)
                {
                    if (date > fValues[i].Day)
                    {
                        return fValues[i];
                    }
                }
            }
            
            return null;
        }

        /// <summary>
        /// returns nearest valuation in the timelist to the date provided.
        /// </summary>
        internal DailyValuation GetNearestValue(DateTime date)
        {
            if (fValues != null && fValues.Any())
            {
                if (Count() == 1)
                {
                    return fValues[0];
                }

                if (date > GetLatestDate())
                {
                    return GetLatestValuation();
                }

                if (date < GetFirstDate())
                {
                    return GetFirstValuation();
                }

                // list sorted with earliest at start. First occurence greater than value means 
                // the first value later.
                for (int i = 0; i < Count(); i++)
                {
                    if (date < fValues[i].Day)
                    {
                        if (fValues[i].Day - date < date - fValues[i - 1].Day)
                        {
                            return fValues[i];
                        }

                        return fValues[i - 1];
                    }
                }
            }
            
            return null;
        }

        /// <summary>
        /// Returns the first date held in the vector, or default if cannot find any data
        /// </summary>
        internal DateTime GetFirstDate()
        {
            if (fValues != null && fValues.Any())
            { 
                return fValues[0].Day;
            }

            return new DateTime();
        }

        /// <summary>
        /// Returns first value held, or 0 if no data.
        /// </summary>
        internal double GetFirstValue()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[0].Value;
            }
            return 0.0;
        }

        /// <summary>
        /// Returns first pair of date and value, or null if this doesn't exist.
        /// </summary>
        /// <returns></returns>
        internal DailyValuation GetFirstValuation()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[0];
            }

            return null;
        }

        /// <summary>
        /// Returns latest date held, or default if no data.
        /// </summary>
        internal DateTime GetLatestDate()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[fValues.Count() - 1].Day;
            }
            return new DateTime();
        }

        /// <summary>
        /// Returns latest value, or 0 if no data held.
        /// </summary>
        internal double GetLatestValue()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[fValues.Count() - 1].Value;
            }

            return 0.0;
        }

        /// <summary>
        /// Returns a pair of date and value of the most recently held data, or null if no data held.
        /// </summary>
        internal DailyValuation GetLatestValuation()
        {
            if (fValues != null && fValues.Any())
            {
                return fValues[fValues.Count() - 1];
            }

            return null;
        }

        /// <summary>
        /// returns all valuations on or between the two dates specified, or empty list if none held.
        /// </summary>
        internal List<DailyValuation> GetValuesBetween(DateTime earlierTime, DateTime laterTime)
        {
            var valuesBetween = new List<DailyValuation>();

            foreach (DailyValuation value in fValues)
            {
                if (value.Day >= earlierTime && value.Day <= laterTime)
                {
                    valuesBetween.Add(value);
                }
            }

            return valuesBetween;
        }
    }
}
