using Effanville.FinancialStructures.NamingStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.NamingStructuresTests
{
    [TestFixture]
    public sealed class NameTests
    {
        [Test]
        public void CanCreate()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            TwoName name = new TwoName(surname, forename);

            Assert.That(name.Name, Is.EqualTo(forename));
            Assert.That(name.Company, Is.EqualTo(surname));
        }

        [TestCase("Bloggs", "Joe", "Bloggs", "Joe", true)]
        [TestCase("Bloggs", "Joe", "Bloggs", "Mark", false)]
        [TestCase("Bloggs", "Joe", "Simon", "Joe", false)]
        [TestCase("Bloggs", "Joe", "Smith", "Alan", false)]
        [TestCase("Bloggs", "Joe", "Bloggs", null, false)]
        [TestCase("Bloggs", "Joe", null, "Joe", false)]
        [TestCase("Bloggs", "Joe", null, null, false)]
        [TestCase("Bloggs", null, "Bloggs", "Joe", false)]
        [TestCase(null, "Joe", "Bloggs", "Joe", false)]
        [TestCase(null, null, "Bloggs", "Joe", false)]
        [TestCase("Bloggs", null, "Bloggs", null, true)]
        [TestCase(null, "Joe", null, "Joe", true)]
        [TestCase(null, null, null, null, true)]
        public void EqualityCorrect(string surname, string forename, string testingSurname, string testingForename, bool expected)
        {
            TwoName player = new TwoName(surname, forename);
            Assert.That(player.Equals(new TwoName(testingSurname, testingForename)), Is.EqualTo(expected));
        }

        [TestCase("Bloggs", "Joe", "Bloggs-Joe")]
        [TestCase("", "Joe", "Joe")]
        [TestCase("Bloggs", "", "Bloggs")]
        [TestCase("", "", "")]
        public void ToStringCorrect(string surname, string forename, string expected)
        {
            TwoName name = new TwoName(surname, forename);

            Assert.That(name.Name, Is.EqualTo(forename));
            Assert.That(name.Company, Is.EqualTo(surname));
            Assert.That(name.ToString(), Is.EqualTo(expected));
        }
    }
}
