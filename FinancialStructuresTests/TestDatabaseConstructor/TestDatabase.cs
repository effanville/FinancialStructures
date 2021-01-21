using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public enum TestDatabaseName
    {
        OneBank,
        OneSecOneBank,
        OneSecTwoBank,
        TwoSecTwoBank,
        TwoBank,
        OneSec,
        TwoSec,
        TwoSecCur,
        TwoBankCur,
        TwoSecTwoBankCur
    }

    public enum NameOrder
    {
        Default,
        Secondary,
        Tertiary
    }

    public static class TestDatabase
    {
        private static Dictionary<TestDatabaseName, IPortfolio> fDatabases;
        public static Dictionary<TestDatabaseName, IPortfolio> Databases
        {
            get
            {
                if (fDatabases == null)
                {
                    fDatabases = new Dictionary<TestDatabaseName, IPortfolio>();

                    var constructor = new DatabaseConstructor();
                    constructor.WithDefaultBankAccount();

                    fDatabases.Add(TestDatabaseName.OneBank, constructor.database.Copy());
                    constructor.WithDefaultSecurity();

                    fDatabases.Add(TestDatabaseName.OneSecOneBank, constructor.database.Copy());

                    constructor.WithSecondaryBankAccount();

                    fDatabases.Add(TestDatabaseName.OneSecTwoBank, constructor.database.Copy());

                    constructor.WithSecondarySecurity();

                    fDatabases.Add(TestDatabaseName.TwoSecTwoBank, constructor.database.Copy());
                    constructor.WithDefaultCurrency();

                    fDatabases.Add(TestDatabaseName.TwoSecTwoBankCur, constructor.database.Copy());
                    constructor.ClearDatabase();

                    constructor.WithDefaultBankAccount();
                    constructor.WithSecondaryBankAccount();

                    fDatabases.Add(TestDatabaseName.TwoBank, constructor.database.Copy());
                    constructor.WithDefaultCurrency();

                    fDatabases.Add(TestDatabaseName.TwoBankCur, constructor.database.Copy());

                    constructor.ClearDatabase();

                    constructor.WithDefaultSecurity();

                    fDatabases.Add(TestDatabaseName.OneSec, constructor.database.Copy());

                    constructor.WithSecondarySecurity();

                    fDatabases.Add(TestDatabaseName.TwoSec, constructor.database.Copy());

                    constructor.WithDefaultCurrency();

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
