using System;
using System.Collections.Generic;

namespace FinancialStructures.Database.Export.History
{
    public sealed partial class PortfolioHistory
    {
        /// <summary>
        /// History settings for a specific type of account.
        /// </summary>
        public sealed class AccountSettings
        {
            /// <summary>
            /// The type of account the settings are for
            /// </summary>
            public Account AccountType
            {
                get;
                set;
            }

            /// <summary>
            /// Should values be generated
            /// </summary>
            public bool GenerateValues
            {
                get;
                set;
            }

            /// <summary>
            /// Should rates be generated
            /// </summary>
            public bool GenerateRates
            {
                get;
                set;
            }

            /// <summary>
            /// Construct an instance.
            /// </summary>
            public AccountSettings(
                Account accountType,
                bool generateValues,
                bool generateRates)
            {
                AccountType = accountType;
                GenerateValues = generateValues;
                GenerateRates = generateRates;
            }

            /// <summary>
            /// Default settings for Securities.
            /// </summary>
            public static AccountSettings DefaultSecuritySettings => new AccountSettings(Account.Security, true, false);

            /// <summary>
            /// Default settings for Bank Accounts.
            /// </summary>
            public static AccountSettings DefaultBankAccountSettings => new AccountSettings(Account.BankAccount, true, false);

            /// <summary>
            /// Default settings for Sectors.
            /// </summary>
            public static AccountSettings DefaultSectorSettings => new AccountSettings(Account.Benchmark, true, false);

            /// <summary>
            /// Default settings for assets.
            /// </summary>
            public static AccountSettings DefaultAssetSettings => new AccountSettings(Account.Asset, true, false);

            /// <summary>
            /// Default settings for pensions.
            /// </summary>
            public static AccountSettings DefaultPensionSettings => new AccountSettings(Account.Pension, true, false);
        }

        /// <summary>
        /// Contains settings for a <see cref="PortfolioHistory"/>.
        /// </summary>
        public sealed class Settings
        {
            /// <summary>
            /// Settings for each account type.
            /// </summary>
            public Dictionary<Account, AccountSettings> SettingsByAccount
            {
                get;
                set;
            } = new Dictionary<Account, AccountSettings>();

            /// <summary>
            /// The earliest date to calculate data on.
            /// </summary>
            public DateTime EarliestDate
            {
                get;
                set;
            }

            /// <summary>
            /// The Last date to calculate data on.
            /// </summary>
            public DateTime LastDate
            {
                get;
                set;
            }

            /// <summary>
            /// The gap between days to record values for.
            /// </summary>
            public int SnapshotIncrement
            {
                get;
                set;
            }

            /// <summary>
            /// The max number of iterations to use for calculating
            /// IRR.
            /// </summary>
            public int MaxIRRIterations
            {
                get;
                set;
            }

            /// <summary>
            /// Construct an instance.
            /// </summary>
            public Settings()
                : this(default, default, 20, default, default, default, default, default, maxIRRIterations: 15)
            {
            }

            /// <summary>
            /// Construct an instance.
            /// </summary>
            public Settings(
                DateTime earliestDate = default,
                DateTime lastDate = default,
                int snapshotIncrement = 20,
                bool generateSecurityValues = true,
                bool generateSecurityRates = false,
                bool generateBankAccountValues = true,
                bool generateSectorValues = true,
                bool generateSectorRates = false,
                bool generateAssetValues = true,
                bool generateAssetRates = false,
                bool generatePensionValues = true,
                bool generatePensionRates = false,
                int maxIRRIterations = 10)
                : this(
                    earliestDate,
                    lastDate,
                    snapshotIncrement,
                    new AccountSettings(Account.Security, generateSecurityValues, generateSecurityRates),
                    new AccountSettings(Account.BankAccount, generateBankAccountValues, false),
                    new AccountSettings(Account.Benchmark, generateSectorValues, generateSectorRates),
                    new AccountSettings(Account.Asset, generateAssetValues, generateAssetRates),
                    new AccountSettings(Account.Pension, generatePensionValues, generatePensionRates),
                    maxIRRIterations)
            {
            }

            /// <summary>
            /// Default constructor.
            /// </summary>
            public Settings(
                DateTime earliestDate = default,
                DateTime lastDate = default,
                int snapshotIncrement = 20,
                AccountSettings securitySettings = default,
                AccountSettings bankAccountSettings = default,
                AccountSettings sectorSettings = default,
                AccountSettings assetSettings = default,
                AccountSettings pensionSettings = default,
                int maxIRRIterations = 10)
            {
                EarliestDate = earliestDate;
                LastDate = lastDate;
                SnapshotIncrement = snapshotIncrement;
                SettingsByAccount.Add(Account.Security, securitySettings ?? AccountSettings.DefaultSecuritySettings);
                SettingsByAccount.Add(Account.BankAccount, bankAccountSettings ?? AccountSettings.DefaultBankAccountSettings);
                SettingsByAccount.Add(Account.Benchmark, sectorSettings ?? AccountSettings.DefaultSectorSettings);
                SettingsByAccount.Add(Account.Asset, assetSettings ?? AccountSettings.DefaultAssetSettings);
                SettingsByAccount.Add(Account.Pension, pensionSettings ?? AccountSettings.DefaultPensionSettings);
                MaxIRRIterations = maxIRRIterations;
            }

            /// <summary>
            /// Retrieve the settings for the account type. Returns null if no
            /// settings exist.
            /// </summary>
            public AccountSettings this[Account account]
            {
                get
                {
                    if (SettingsByAccount.TryGetValue(account, out var value))
                    {
                        return value;
                    }

                    return null;
                }
            }
        }
    }
}
