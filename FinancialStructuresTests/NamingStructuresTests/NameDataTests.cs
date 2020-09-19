using System.Linq;
using FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace FinancialStructures.Tests.NamingStructuresTests
{
    [TestFixture]
    public sealed class NameDataTests
    {
        [TestCase("cat", 1, "cat")]
        [TestCase("cat,dog", 2, "cat,dog")]
        [TestCase("dog,cat", 2, "dog,cat")]
        [TestCase("dog man,cat", 2, "dog man,cat")]
        public void CanSetSectorsFromFlatList(string inputSectors, int expectedNumber, string expected)
        {
            var data = new NameData("company", "name");
            data.SectorsFlat = inputSectors;

            Assert.AreEqual(expectedNumber, data.Sectors.Count);
            Assert.AreEqual(expected, data.SectorsFlat);
        }

        [Test]
        public void HasExpectedSectors()
        {
            var data = new NameData("company", "name");
            data.SectorsFlat = "dog man, cat ,human";

            Assert.AreEqual(3, data.Sectors.Count);
            string[] expected = new string[] { "dog man", "cat", "human" };
            var listing = data.Sectors.ToArray();
            for (int sectorIndex = 0; sectorIndex < expected.Length; sectorIndex++)
            {
                Assert.AreEqual(expected[sectorIndex], listing[sectorIndex]);
            }
        }

        [TestCase("comp", "name", "url", "sectors", "currency", "comp", "name", "url", "sectors", "currency", true)]
        [TestCase("com", "name", "url", "sectors", "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "nam", "url", "sectors", "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "ur", "sectors", "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "url", "sector", "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "url", "sectors", "curr", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase(null, "name", "url", "sectors", "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", null, "url", "sectors", "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", null, "sectors", "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "url", null, "currency", "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "url", "sectors", null, "comp", "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "url", "sectors", "currency", null, "name", "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "url", "sectors", "currency", "comp", null, "url", "sectors", "currency", false)]
        [TestCase("comp", "name", "url", "sectors", "currency", "comp", "name", null, "sectors", "currency", false)]
        [TestCase("comp", "name", "url", "sectors", "currency", "comp", "name", "url", null, "currency", false)]
        [TestCase("comp", "name", "url", "sectors", "currency", "comp", "name", "url", "sectors", null, false)]
        [TestCase(null, "name", "url", "sectors", "currency", null, "name", "url", "sectors", "currency", true)]
        [TestCase("comp", null, "url", "sectors", "currency", "comp", null, "url", "sectors", "currency", true)]
        [TestCase("comp", "name", null, "sectors", "currency", "comp", "name", null, "sectors", "currency", true)]
        [TestCase("comp", "name", "url", null, "currency", "comp", "name", "url", null, "currency", true)]
        [TestCase("comp", "name", "url", "sectors", null, "comp", "name", "url", "sectors", null, true)]
        [TestCase("comp", "name", "url", null, null, "comp", "name", "url", "sectors", null, false)]
        [TestCase("comp", "name", null, "sectors", null, "comp", "name", "url", "sectors", null, false)]
        [TestCase("comp", "name", null, null, "currency", "comp", "name", "url", "sectors", null, false)]
        [TestCase(null, null, null, null, null, null, null, null, null, null, true)]
        public void EqualityTests(string company, string name, string url, string SectorsFlat, string currency, string otherCompany, string otherName, string otherUrl, string otherSectorsFlat, string otherCurrency, bool areEqual)
        {
            var firstName = new NameData(company, name, currency, url);
            firstName.SectorsFlat = SectorsFlat;
            var secondName = new NameData(otherCompany, otherName, otherCurrency, otherUrl);
            secondName.SectorsFlat = otherSectorsFlat;

            Assert.AreEqual(areEqual, firstName.Equals(secondName));
        }
    }
}
