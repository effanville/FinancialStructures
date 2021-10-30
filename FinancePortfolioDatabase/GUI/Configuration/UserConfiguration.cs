using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Common.Structure.Reporting;
using FinancePortfolioDatabase.GUI.ViewModels.Stats;

namespace FinancePortfolioDatabase.GUI.Configuration
{
    /// <summary>
    /// Contains user specific configuration for the ui.
    /// </summary>
    [DataContract]
    public sealed partial class UserConfiguration : IConfiguration
    {
        private readonly IReadOnlyList<Migration<UserConfiguration>> fMigrations;
        private readonly Type[] fExpectedConfigurationTypes = new Type[]
        {
            typeof(StatsDisplayConfiguration),
            typeof(StatsCreatorConfiguration),
            typeof(ExportHistoryConfiguration),
            typeof(ExportStatsConfiguration),
            typeof(ExportReportConfiguration)
        };

        /// <summary>
        /// Name of the child configuration for the stats window.
        /// </summary>
        public const string StatsDisplay = nameof(StatsViewModel);

        /// <summary>
        /// Name of the child configuration for the <see cref="StatsCreatorWindowViewModel"/>.
        /// </summary>
        public const string StatsCreator = nameof(StatsCreatorWindowViewModel);

        /// <summary>
        /// Name of the child configuration for the <see cref="ExportStatsViewModel"/>
        /// </summary>
        public const string StatsOptions = nameof(ExportStatsViewModel);

        /// <summary>
        /// Name of the child configuration for the <see cref="ExportHistoryViewModel"/>
        /// </summary>
        public const string HistoryOptions = nameof(ExportHistoryViewModel);

        /// <summary>
        /// Name of the child configuration for the <see cref="ExportHistoryViewModel"/>
        /// </summary>
        public const string ReportOptions = nameof(ExportReportViewModel);

        /// <summary>
        /// The version of the program this version is associated to.
        /// </summary>
        [DataMember(Order = 1)]
        public Version ProgramVersion
        {
            get;
            set;
        }

        /// <inheritdoc/>
        [DataMember(Order = 2)]
        public Dictionary<string, IConfiguration> ChildConfigurations
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public bool HasLoaded
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserConfiguration()
        {
            ChildConfigurations = new Dictionary<string, IConfiguration>
            {
                { StatsDisplay, new StatsDisplayConfiguration() },
                { StatsCreator, new StatsCreatorConfiguration() }
            };
            fMigrations = new List<Migration<UserConfiguration>>() { new Migration<UserConfiguration>(new Version(), AddExportCommandMigration) };
        }

        private void AddExportCommandMigration(UserConfiguration config)
        {
            config.ChildConfigurations[StatsCreator].ChildConfigurations.Add(ReportOptions, new ExportReportConfiguration());
        }

        /// <summary>
        /// Static constructor enabling loading of config from file if the file exists.
        /// </summary>
        public static UserConfiguration LoadFromUserConfigFile(string filePath, IFileSystem fileSystem, IReportLogger logger = null)
        {
            UserConfiguration userConfig = new UserConfiguration();
            if (fileSystem.File.Exists(filePath))
            {
                userConfig.LoadConfiguration(filePath, fileSystem, logger);
            }

            return userConfig;
        }

        /// <inheritdoc/>
        public void StoreConfiguration(object viewModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RestoreFromConfiguration(object viewModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void LoadConfiguration(string filePath, IFileSystem fileSystem, IReportLogger logger = null)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                DataContractSerializer serializer = new DataContractSerializer(typeof(UserConfiguration), fExpectedConfigurationTypes);
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Open))
                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    UserConfiguration configuration = (UserConfiguration)serializer.ReadObject(reader);
                    foreach (var migration in fMigrations)
                    {
                        migration.EnactMigration(configuration, configuration.ProgramVersion, assembly.GetName().Version);
                    }

                    ChildConfigurations = configuration.ChildConfigurations;
                }
            }
            catch
            {
                _ = logger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Could not load configuration file, reverting to default values.");
            }
        }

        /// <inheritdoc/>
        public void SaveConfiguration(string filePath, IFileSystem fileSystem, IReportLogger logger = null)
        {
            try
            {
                if (ProgramVersion == default)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    ProgramVersion = assembly.GetName().Version;
                }

                string dir = fileSystem.Path.GetDirectoryName(filePath);
                _ = fileSystem.Directory.CreateDirectory(dir);
                DataContractSerializer serializer = new DataContractSerializer(typeof(UserConfiguration), fExpectedConfigurationTypes);
                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented; // indent the Xml so it's human readable
                    serializer.WriteObject(writer, this);
                }
            }
            catch
            {
                _ = logger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, "Could not save configuration file.");
            }
        }
    }
}
