﻿using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Tests
{
    public static class TestDatabase
    {
        private static Dictionary<TestDatabaseName, IPortfolio> fDatabases;
        private static readonly string TestName = "saved";
        public static Dictionary<TestDatabaseName, IPortfolio> Databases
        {
            get
            {
                if (fDatabases == null)
                {
                    fDatabases = new Dictionary<TestDatabaseName, IPortfolio>();

                    DatabaseConstructor constructor = new DatabaseConstructor()
                        .SetName(TestName)
                        .SetCurrencyAsGBP()
                        .WithDefaultBankAccount();
                    fDatabases.Add(TestDatabaseName.OneBank, constructor.Database.Copy());

                    _ = constructor.WithDefaultSecurity();
                    fDatabases.Add(TestDatabaseName.OneSecOneBank, constructor.Database.Copy());

                    _ = constructor.WithSecondaryBankAccount();
                    fDatabases.Add(TestDatabaseName.OneSecTwoBank, constructor.Database.Copy());

                    _ = constructor.WithSecondarySecurity();
                    fDatabases.Add(TestDatabaseName.TwoSecTwoBank, constructor.Database.Copy());

                    _ = constructor
                        .SetCurrencyAsGBP()
                        .WithDefaultCurrency();
                    fDatabases.Add(TestDatabaseName.TwoSecTwoBankCur, constructor.Database.Copy());

                    _ = constructor
                        .ClearDatabase()
                        .SetCurrencyAsGBP()
                        .SetName(TestName);

                    _ = constructor.WithDefaultBankAccount()
                        .WithSecondaryBankAccount();
                    fDatabases.Add(TestDatabaseName.TwoBank, constructor.Database.Copy());
                    _ = constructor
                        .SetCurrencyAsGBP()
                        .WithDefaultCurrency();
                    fDatabases.Add(TestDatabaseName.TwoBankCur, constructor.Database.Copy());

                    _ = constructor
                        .ClearDatabase()
                        .SetCurrencyAsGBP()
                        .SetName(TestName)
                        .WithDefaultSecurity();
                    fDatabases.Add(TestDatabaseName.OneSec, constructor.Database.Copy());

                    _ = constructor.WithSecondarySecurity();

                    fDatabases.Add(TestDatabaseName.TwoSec, constructor.Database.Copy());

                    _ = constructor
                        .SetCurrencyAsGBP()
                        .WithDefaultCurrency();
                    fDatabases.Add(TestDatabaseName.TwoSecCur, constructor.Database.Copy());
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
            return DatabaseConstructor.DefaultName(acctype);
        }

        public static TwoName SecondaryName(Account acctype)
        {
            return DatabaseConstructor.SecondaryName(acctype);
        }
    }
}
