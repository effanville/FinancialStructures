using NUnit.Framework;
using StructureCommon.DataStructures;
using System;

namespace DataStructures_UnitTests
{
    public class DailyValuationTests
    {
        [TestCase("1/1/2018", 1, "1/1/2019", 0.0, -1)]
        [TestCase("1/1/2020", 1, "1/1/2019", 0.0, 1)]
        [TestCase("1/1/2018", 1, "1/1/2018", 0.0, 0)]
        public void ComparisonTests(DateTime firstDate, double firstValue, DateTime secondDate, double secondValue, int result)
        {
            DailyValuation first = new DailyValuation(firstDate, firstValue);

            DailyValuation second = new DailyValuation(secondDate, secondValue);
            int comparison = first.CompareTo(second);

            Assert.AreEqual(result, comparison);
        }

        [TestCase("1/1/2018", 1)]
        public void CopyTests(DateTime date, double value)
        {
            DailyValuation data = new DailyValuation(date, value);

            DailyValuation newData = data.Copy();

            newData.SetDay(DateTime.Parse("1/1/2019"));
            Assert.AreNotEqual(data, newData);
        }
    }
}
