using NUnit.Framework;
using StructureCommon.DataStructures;
using StructureCommon.FinanceFunctions;
using System;
using System.Collections.Generic;

namespace MathLibrary.FinancialFunctions_UnitTests
{
    public class RatesTests
    {

        private static List<DailyValuation> Switcher(int i)
        {
            switch (i)
            {
                case 0:
                    return null;
                case 1:
                    return new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2018"), 1000) };
                case 2:
                    return new List<DailyValuation>() { new DailyValuation(DateTime.Parse("1/1/2018"), 1000), new DailyValuation(DateTime.Parse("1/6/2018"), 1000) };
            }

            return null;
        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("1/1/2018", 10.0, "1/1/2019", 20.0, 1.0)]
        [TestCase("1/1/2017", 10.0, "1/1/2019", 90.0, 2.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 90.0, double.NaN)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 0.0, 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2019", 0.0, -1.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 11.0, double.NaN)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 10.0, 0.0)]
        public void CAR_BasicData_Tests(string first, double firstValue, string last, double lastValue, double expected)
        {
            double rate = FinancialFunctions.CAR(DateTime.Parse(first), firstValue, DateTime.Parse(last), lastValue);
            Assert.AreEqual(expected, rate, "CAR is not as expected.");
        }

        [TestCase("1/1/2018", 10.0, "1/1/2019", 20.0, 1.0)]
        [TestCase("1/1/2017", 10.0, "1/1/2019", 90.0, 2.0)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 90.0, double.NaN)]
        [TestCase("1/1/2018", 0.0, "1/1/2019", 0.0, 0.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2019", 0.0, -1.0)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 11.0, double.NaN)]
        [TestCase("1/1/2018", 10.0, "1/1/2018", 10.0, 0.0)]
        public void CAR_ComplexInput_Tests(string first, double firstValue, string last, double lastValue, double expected)
        {
            double rate = FinancialFunctions.CAR(new DailyValuation(DateTime.Parse(first), firstValue), new DailyValuation(DateTime.Parse(last), lastValue));
            Assert.AreEqual(expected, rate, "CAR is not as expected.");
        }

        [TestCase(0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2019", 2000.0, 1.0)]
        [TestCase(2, "1/1/2019", 2000.0, 0.0)]
        [TestCase(2, "1/1/2019", 4000.0, 1.351)]
        public void IRRTests(int switcher, string last, double lastValue, double expected)
        {
            double rate = FinancialFunctions.IRR(Switcher(switcher), new DailyValuation(DateTime.Parse(last), lastValue));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }

        [TestCase(0, "1/1/2018", 1000.0, "1/1/2019", 2000.0, double.NaN)]
        [TestCase(1, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 1.0)]
        [TestCase(1, "1/6/2018", 1000.0, "1/1/2019", 2000.0, 2.261)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 2000.0, 0.0)]
        [TestCase(2, "1/1/2018", 1000.0, "1/1/2019", 4000.0, 1.351)]
        public void IRR_Time_Tests(int switcher, string start, double startValue, string last, double lastValue, double expected)
        {
            double rate = FinancialFunctions.IRRTime(new DailyValuation(DateTime.Parse(start), startValue), Switcher(switcher), new DailyValuation(DateTime.Parse(last), lastValue));
            Assert.AreEqual(expected, rate, 1e-3, "CAR is not as expected.");
        }
    }
}