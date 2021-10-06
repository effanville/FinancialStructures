using System;
using FinancialStructures.DataStructures;
using NUnit.Framework;

namespace FinancialStructures.Tests.DataStructuresTests
{
    [TestFixture]
    public sealed class SecurityDayDataTests
    {
        [TestCase("1/1/2019", "2/1/2019", -1)]
        [TestCase("1/1/2019", "1/1/2019", 0)]
        [TestCase("5/1/2019", "1/1/2019", 1)]
        public void ComparisonTests(DateTime first, DateTime second, int expected)
        {
            SecurityDayData one = new SecurityDayData(first, 0, 0, 0);
            SecurityDayData two = new SecurityDayData(second, 0, 0, 0);
            Assert.AreEqual(expected, one.CompareTo(two));
        }

        [TestCase("1/1/2019", 1, 1, 1, "01/01/2019, 1, 1, 1")]
        [TestCase("1/1/2015", 1.45, 2.2, 3.4, "01/01/2015, 1.45, 2.2, 3.4")]
        public void ToStringTests(DateTime first, double unit, double share, double inv, string expected)
        {
            SecurityDayData one = new SecurityDayData(first, unit, share, inv);
            Assert.AreEqual(expected, one.ToString());
        }
    }
}
