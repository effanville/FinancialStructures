using System;

namespace FinancePortfolioDatabase
{
    /// <summary>
    /// A migration from one version of an object to another.
    /// </summary>
    /// <typeparam name="T">The object to record a migration on.</typeparam>
    public sealed class Migration<T>
    {
        private readonly Action<T> fMigrationAction;

        /// <summary>
        /// The version upon which this migration takes effect. This is the minimum version for 
        /// which one must perform this migration.
        /// </summary>
        public Version MinimumVersion
        {
            get;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public Migration(Version version, Action<T> migrationAction)
        {
            MinimumVersion = version;
            fMigrationAction = migrationAction;
        }

        /// <summary>
        /// The mechanism to perform the migration.
        /// </summary>
        public void EnactMigration(T objectToMigrate, Version oldVersion, Version newVersion)
        {
            if (!IsMigrationRequired(MinimumVersion, oldVersion, newVersion))
            {
                return;
            }

            fMigrationAction(objectToMigrate);
        }

        private static bool IsMigrationRequired(Version migrationVersion, Version oldVersion, Version newVersion)
        {
            if (oldVersion == newVersion)
            {
                return false;
            }

            if (migrationVersion < oldVersion)
            {
                return false;
            }

            if (newVersion <= migrationVersion)
            {
                return true;
            }

            return false;
        }
    }
}
