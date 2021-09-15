using System.IO;
using System.IO.Abstractions.TestingHelpers;
using FinancePortfolioDatabase.GUI.Configuration;
using FinancePortfolioDatabase.Tests.TestHelpers;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests
{
    [TestFixture]
    public sealed class ConfigurationTests
    {
        private readonly string DefaultSerializedConfiguration =
@"<UserConfiguration xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/FinancePortfolioDatabase.GUI.Configuration"">
  <ChildConfigurations xmlns:d2p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
    <d2p1:KeyValueOfstringanyType>
      <d2p1:Key>StatsViewModel</d2p1:Key>
      <d2p1:Value i:type=""StatsDisplayConfiguration"">
        <ChildConfigurations />
        <DisplayValueFunds>false</DisplayValueFunds>
        <HasLoaded>false</HasLoaded>
      </d2p1:Value>
    </d2p1:KeyValueOfstringanyType>
    <d2p1:KeyValueOfstringanyType>
      <d2p1:Key>StatsCreatorWindowViewModel</d2p1:Key>
      <d2p1:Value i:type=""StatsCreatorConfiguration"">
        <ChildConfigurations>
          <d2p1:KeyValueOfstringanyType>
            <d2p1:Key>ExportStatsViewModel</d2p1:Key>
            <d2p1:Value i:type=""ExportStatsConfiguration"">
              <BankColumnNames xmlns:d8p1=""http://schemas.datacontract.org/2004/07/Common.Structure.DisplayClasses"" />
              <BankDirection>Ascending</BankDirection>
              <BankSortingField>AccountType</BankSortingField>
              <ChildConfigurations i:nil=""true"" />
              <DisplayConditions xmlns:d8p1=""http://schemas.datacontract.org/2004/07/Common.Structure.DisplayClasses"" />
              <HasLoaded>false</HasLoaded>
              <SectorColumnNames xmlns:d8p1=""http://schemas.datacontract.org/2004/07/Common.Structure.DisplayClasses"" />
              <SectorDirection>Ascending</SectorDirection>
              <SectorSortingField>AccountType</SectorSortingField>
              <SecurityColumnNames xmlns:d8p1=""http://schemas.datacontract.org/2004/07/Common.Structure.DisplayClasses"" />
              <SecurityDirection>Ascending</SecurityDirection>
              <SecuritySortingField>AccountType</SecuritySortingField>
            </d2p1:Value>
          </d2p1:KeyValueOfstringanyType>
          <d2p1:KeyValueOfstringanyType>
            <d2p1:Key>ExportHistoryViewModel</d2p1:Key>
            <d2p1:Value i:type=""ExportHistoryConfiguration"">
              <ChildConfigurations />
              <HasLoaded>false</HasLoaded>
              <HistoryGapDays>0</HistoryGapDays>
            </d2p1:Value>
          </d2p1:KeyValueOfstringanyType>
        </ChildConfigurations>
        <HasLoaded>false</HasLoaded>
      </d2p1:Value>
    </d2p1:KeyValueOfstringanyType>
  </ChildConfigurations>
</UserConfiguration>";

        [Test]
        public void CanLoadConfig()
        {
            var tempFileSystem = new MockFileSystem();
            string testPath = "c:/temp/user.config";
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\user.config");
            tempFileSystem.AddFile(testPath, new MockFileData(file));
            var config = UserConfiguration.LoadFromUserConfigFile(testPath, tempFileSystem);

            Assert.AreEqual(2, config.ChildConfigurations.Count);
            Assert.Multiple(() =>
            {
                var statsVM = config.ChildConfigurations[UserConfiguration.StatsDisplay] as StatsDisplayConfiguration;
                Assert.AreEqual(0, statsVM.ChildConfigurations.Count);
                Assert.AreEqual(true, statsVM.HasLoaded);
                Assert.AreEqual(true, statsVM.DisplayValueFunds);
            });

            Assert.Multiple(() =>
            {
                var statsCreatorVM = config.ChildConfigurations[UserConfiguration.StatsCreator] as StatsCreatorConfiguration;
                Assert.AreEqual(2, statsCreatorVM.ChildConfigurations.Count);

                var exportStats = statsCreatorVM.ChildConfigurations[UserConfiguration.StatsOptions] as ExportStatsConfiguration;
                Assert.AreEqual(true, exportStats.HasLoaded);

                var exportHistory = statsCreatorVM.ChildConfigurations[UserConfiguration.HistoryOptions] as ExportHistoryConfiguration;
                Assert.AreEqual(20, exportHistory.HistoryGapDays);
                Assert.AreEqual(true, exportHistory.HasLoaded);
            });
        }

        [Test]
        public void CanLoadWithoutConfigFile()
        {
            var tempFileSystem = new MockFileSystem();
            string testPath = "c:/temp/user.config";
            var config = UserConfiguration.LoadFromUserConfigFile(testPath, tempFileSystem);

            Assert.AreEqual(2, config.ChildConfigurations.Count);
            Assert.Multiple(() =>
            {
                var statsVM = config.ChildConfigurations[UserConfiguration.StatsDisplay] as StatsDisplayConfiguration;
                Assert.AreEqual(0, statsVM.ChildConfigurations.Count);
                Assert.AreEqual(false, statsVM.HasLoaded);
                Assert.AreEqual(false, statsVM.DisplayValueFunds);
            });

            Assert.Multiple(() =>
            {
                var statsCreatorVM = config.ChildConfigurations[UserConfiguration.StatsCreator] as StatsCreatorConfiguration;
                Assert.AreEqual(2, statsCreatorVM.ChildConfigurations.Count);

                var exportStats = statsCreatorVM.ChildConfigurations[UserConfiguration.StatsOptions] as ExportStatsConfiguration;
                Assert.AreEqual(false, exportStats.HasLoaded);

                var exportHistory = statsCreatorVM.ChildConfigurations[UserConfiguration.HistoryOptions] as ExportHistoryConfiguration;
                Assert.AreEqual(0, exportHistory.HistoryGapDays);
                Assert.AreEqual(false, exportHistory.HasLoaded);
            });
        }

        [Test]
        public void CanSaveConfig()
        {
            var tempFileSystem = new MockFileSystem();
            var config = new UserConfiguration();
            string testPath = "c:/temp/saved/user.config";
            config.SaveConfiguration(testPath, tempFileSystem);

            string file = tempFileSystem.File.ReadAllText(testPath);
            Assert.AreEqual(DefaultSerializedConfiguration, file);
        }

        [Test]
        public void RoundTripTest()
        {
            var tempFileSystem = new MockFileSystem();
            var config = new UserConfiguration();
            string testPath = "c:/temp/saved/user.config";
            config.SaveConfiguration(testPath, tempFileSystem);

            string file = tempFileSystem.File.ReadAllText(testPath);
            Assert.AreEqual(DefaultSerializedConfiguration, file);

            var newConfig = UserConfiguration.LoadFromUserConfigFile(testPath, tempFileSystem);

            Assert.AreEqual(2, newConfig.ChildConfigurations.Count);
            Assert.Multiple(() =>
            {
                var statsVM = newConfig.ChildConfigurations[UserConfiguration.StatsDisplay] as StatsDisplayConfiguration;
                Assert.AreEqual(0, statsVM.ChildConfigurations.Count);
                Assert.AreEqual(false, statsVM.HasLoaded);
                Assert.AreEqual(false, statsVM.DisplayValueFunds);
            });

            Assert.Multiple(() =>
            {
                var statsCreatorVM = newConfig.ChildConfigurations[UserConfiguration.StatsCreator] as StatsCreatorConfiguration;
                Assert.AreEqual(2, statsCreatorVM.ChildConfigurations.Count);

                var exportStats = statsCreatorVM.ChildConfigurations[UserConfiguration.StatsOptions] as ExportStatsConfiguration;
                Assert.AreEqual(false, exportStats.HasLoaded);

                var exportHistory = statsCreatorVM.ChildConfigurations[UserConfiguration.HistoryOptions] as ExportHistoryConfiguration;
                Assert.AreEqual(0, exportHistory.HistoryGapDays);
                Assert.AreEqual(false, exportHistory.HasLoaded);
            });
        }
    }
}
