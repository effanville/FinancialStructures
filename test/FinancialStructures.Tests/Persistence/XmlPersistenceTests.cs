using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Persistence;
using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Persistence
{
    [TestFixture]
    public sealed class XmlPersistenceTests
    {
        private static IEnumerable<TestCaseData> WriteSerializationData(string testName)
        {
            IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> tests = XmlSerializationExamples.NewStyleTestLists();
            foreach ((string name, IPortfolio testPortfolio, string XmlString) test in tests)
            {
                yield return new TestCaseData(test.XmlString, test.testPortfolio).SetName($"{testName}-{test.name}");
            }
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(WriteXmlTests) })]
        public void WriteXmlTests(string expectedXml, IPortfolio times)
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.xml";

            XmlPortfolioPersistence xmlPersistence = new XmlPortfolioPersistence(null);
            XmlFilePersistenceOptions options = new XmlFilePersistenceOptions(savePath, tempFileSystem, PortfolioPersistence.ReadVersion);
            xmlPersistence.Save(times, options);

            string file = tempFileSystem.File.ReadAllText(savePath);

            Assert.That(file, Is.EqualTo(expectedXml));
        }

        private static IEnumerable<TestCaseData> ReadSerializationData(string testName)
        {
            IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> oldTests = XmlSerializationExamples.OldStyleTestLists();
            foreach ((string name, IPortfolio testPortfolio, string XmlString) test in oldTests)
            {
                yield return new TestCaseData(test.XmlString, test.testPortfolio).SetName($"{testName}old-{test.name}");
            }

            IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> newTests = XmlSerializationExamples.NewStyleTestLists();
            foreach ((string name, IPortfolio testPortfolio, string XmlString) test in newTests)
            {
                yield return new TestCaseData(test.XmlString, test.testPortfolio).SetName($"{testName}-{test.name}");
            }
        }

        [TestCaseSource(nameof(ReadSerializationData), new object[] { nameof(ReadXmlTests) })]
        public void ReadXmlTests(string expectedXml, IPortfolio times)
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.xml";
            tempFileSystem.AddFile(savePath, new MockFileData(expectedXml));
            XmlPortfolioPersistence xmlPersistence = new XmlPortfolioPersistence(null);
            IPortfolio loadedPortfolio = xmlPersistence.Load(new XmlFilePersistenceOptions(savePath, tempFileSystem, PortfolioPersistence.ReadVersion));

            PortfolioEquality.AreEqual(times, loadedPortfolio);
        }

        [TestCaseSource(nameof(ReadSerializationData), new object[] { nameof(ReadV1XmlWithV2ReaderTests) })]
        public void ReadV1XmlWithV2ReaderTests(string expectedXml, IPortfolio times)
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.xml";
            tempFileSystem.AddFile(savePath, new MockFileData(expectedXml));
            XmlPortfolioPersistence xmlPersistence = new XmlPortfolioPersistence(null);
            IPortfolio loadedPortfolio = xmlPersistence.Load(new XmlFilePersistenceOptions(savePath, tempFileSystem, "2.0.0.0"));

            PortfolioEquality.AreEqual(times, loadedPortfolio);
        }


        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(RoundTripSaveTests) })]
        public void RoundTripSaveTests(string expectedXml, IPortfolio database)
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.xml";

            XmlPortfolioPersistence xmlPersistence = new XmlPortfolioPersistence(null);
            XmlFilePersistenceOptions options = new XmlFilePersistenceOptions(savePath, tempFileSystem, PortfolioPersistence.ReadVersion);
            xmlPersistence.Save(database, options);

            string file = tempFileSystem.File.ReadAllText(savePath);

            Assert.That(file, Is.EqualTo(expectedXml));

            IPortfolio loadedPortfolio = xmlPersistence.Load(new XmlFilePersistenceOptions(savePath, tempFileSystem, PortfolioPersistence.WriteVersion));

            PortfolioEquality.AreEqual(database, loadedPortfolio);
        }
    }
}
