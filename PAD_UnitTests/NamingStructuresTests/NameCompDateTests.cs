using System;
using FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace FinancialStructures_UnitTests.NamingStructuresTests
{
    class NameCompDateTests
    {
        [TestCase("2000/1/1", "2000/1/1", true)]
        [TestCase("2001/1/1", "2000/1/1", false)]
        [TestCase("2000/1/1", "2001/1/1", false)]
        public void EqualityTests(DateTime date, DateTime otherDate, bool equal)
        {
            var firstName = new NameCompDate("company", "name", "currency", "url", null, date);
            var secondName = new NameCompDate("company", "name", "currency", "url", null, otherDate);

            Assert.AreEqual(equal, firstName.Equals(secondName));
        }
    }
}
