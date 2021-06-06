using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests
{
    public static class TestDatabase
    {
        private static Dictionary<TestDatabaseName, IPortfolio> fDatabases;
        private static string TestFilePath = "c:/temp/saved.xml";
        public static Dictionary<TestDatabaseName, IPortfolio> Databases
        {
            get
            {
                if (fDatabases == null)
                {
                    fDatabases = new Dictionary<TestDatabaseName, IPortfolio>();

                    var constructor = new DatabaseConstructor(TestFilePath);
                    _ = constructor.WithDefaultBankAccount();
                    fDatabases.Add(TestDatabaseName.OneBank, constructor.database.Copy());

                    _ = constructor.WithDefaultSecurity();
                    fDatabases.Add(TestDatabaseName.OneSecOneBank, constructor.database.Copy());

                    _ = constructor.WithSecondaryBankAccount();
                    fDatabases.Add(TestDatabaseName.OneSecTwoBank, constructor.database.Copy());

                    _ = constructor.WithSecondarySecurity();
                    fDatabases.Add(TestDatabaseName.TwoSecTwoBank, constructor.database.Copy());

                    _ = constructor
                        .SetCurrencyAsGBP()
                        .WithDefaultCurrency();
                    fDatabases.Add(TestDatabaseName.TwoSecTwoBankCur, constructor.database.Copy());

                    _ = constructor
                        .ClearDatabase()
                        .SetCurrencyAsGBP()
                        .SetFilePath(TestFilePath);

                    _ = constructor.WithDefaultBankAccount()
                        .WithSecondaryBankAccount();
                    fDatabases.Add(TestDatabaseName.TwoBank, constructor.database.Copy());
                    _ = constructor
                        .SetCurrencyAsGBP()
                        .WithDefaultCurrency();
                    fDatabases.Add(TestDatabaseName.TwoBankCur, constructor.database.Copy());

                    _ = constructor
                        .ClearDatabase()
                        .SetCurrencyAsGBP()
                        .SetFilePath(TestFilePath)
                        .WithDefaultSecurity();
                    fDatabases.Add(TestDatabaseName.OneSec, constructor.database.Copy());

                    _ = constructor.WithSecondarySecurity();

                    fDatabases.Add(TestDatabaseName.TwoSec, constructor.database.Copy());

                    _ = constructor
                        .SetCurrencyAsGBP()
                        .WithDefaultCurrency();
                    fDatabases.Add(TestDatabaseName.TwoSecCur, constructor.database.Copy());
                }

                return fDatabases;
            }
        }

        public static TwoName Name(Account account, NameOrder order)
        {
            switch (order)
            {
                case NameOrder.Default:
                {
                    return DefaultName(account);
                }
                case NameOrder.Secondary:
                {
                    return SecondaryName(account);
                }
                case NameOrder.Tertiary:
                default:
                {
                    return null;
                }
            }
        }

        public static TwoName DefaultName(Account acctype)
        {
            var constructor = new DatabaseConstructor();
            return constructor.DefaultName(acctype);
        }

        public static TwoName SecondaryName(Account acctype)
        {
            var constructor = new DatabaseConstructor();
            return constructor.SecondaryName(acctype);
        }
    }
}
