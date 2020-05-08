using FinancialStructures.DataStructures;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DataStructures.TimeList_UnitTests
{
    public class TimeList_RatesTests
    {

        private static TimeList Switcher(int i)
        {
            switch (i)
            {
                case 0:
                    return null;
                case 1:
                    return new TimeList(new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2018"), 1000) });
                case 2:
                    return new TimeList(new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2018"), 1000), new DailyValuation(DateTime.Parse("1/6/2018"), 1000) });
                case 3:
                    return new TimeList(new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2017"), 1000), new DailyValuation(DateTime.Parse("1/1/2018"), 1100), new DailyValuation(DateTime.Parse("1/6/2018"), 1200) });
                case 4:
                    return new TimeList(new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2017"), 1000), new DailyValuation(DateTime.Parse("1/1/2018"), -1100), new DailyValuation(DateTime.Parse("1/6/2018"), 1200) });
            }

            return null;
        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(1, "1/1/2018", "1/1/2019", 0.0)]
        [TestCase(2, "1/1/2018", "1/1/2019", 0.0)]
        [TestCase(2, "1/1/2017", "1/1/2019", double.NaN)]
        [TestCase(3, "1/1/2017", "1/1/2019", 0.1376)]
        public void TimeList_CAR_Tests(int switcher, string earlierDate, string laterDate, double expected)
        {
            double rate = Switcher(switcher).CAR(DateTime.Parse(earlierDate), DateTime.Parse(laterDate));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }

        [TestCase(1, 1000)]
        [TestCase(2, 2000)]
        [TestCase(3, 3300)]
        [TestCase(4, 1100)]
        public void TimeList_Sum_Tests(int switcher, double expected)
        {
            double value = Switcher(switcher).Sum();
            Assert.AreEqual(expected, value, "Values should be equal.");
        }
    }
}