using System;
using FinancialStructures.PortfolioAPI;
using FinancialStructures_UnitTests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures_UnitTests.PortfolioAPI.Value
{
    public sealed class TotalValueTests
    {
        [TestCase(AccountType.Security, 556.04999999999995)]
        [TestCase(AccountType.BankAccount, 101.1)]
        public void LatestTotalValueOneAccountTests(AccountType accountType, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(accountType));
        }

        [TestCase(AccountType.Security, 26084.099999999999)]
        [TestCase(AccountType.BankAccount, 1102.2)]
        public void LatestTotalValueTests(AccountType accountType, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultFromType(accountType);
            constructor.WithSecondaryFromType(accountType);
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(accountType));
        }


        [TestCase("2009/1/2", 0.0)]
        [TestCase("2010/1/1", 200.0)]
        [TestCase("2010/1/2", 200.0)]
        [TestCase("2015/1/2", 18939.369577246791)]
        [TestCase("2018/1/1", 31652.961158669885)]
        [TestCase("2018/5/5", 33799.402885014038)]
        [TestCase("2018/5/6", 33948.80289893617)]
        [TestCase("2018/5/7", 33966.379079831197)]
        [TestCase("2020/5/1", 26084.099999999999)]
        public void TotalValueTest(DateTime date, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultSecurity();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(AccountType.Security, date));
        }

        [TestCase("2009/1/2", 0.0)]
        [TestCase("2010/1/1", 200.0)]
        [TestCase("2010/1/2", 200.0)]
        [TestCase("2015/1/2", 18939.369577246791)]
        [TestCase("2018/1/1", 31652.961158669885)]
        [TestCase("2018/5/5", 33799.402885014038)]
        [TestCase("2018/5/6", 33948.80289893617)]
        [TestCase("2018/5/7", 33966.379079831197)]
        [TestCase("2020/5/1", 26084.099999999999)]
        public void SecurityTotalValueSingleValueTest(DateTime date, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultSecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(portfolio.Value(AccountType.Security, constructor.DefaultNameQuery(AccountType.Security), date), portfolio.TotalValue(AccountType.Security, date));
        }

        [TestCase("2009/1/2", 0.0)]
        [TestCase("2010/1/1", 1200.0)]
        [TestCase("2010/1/2", 1200.0)]
        [TestCase("2015/1/2", 1250.4000000000001)]
        [TestCase("2018/1/1", 991.20000000000005)]
        [TestCase("2018/5/5", 991.20000000000005)]
        [TestCase("2018/5/6", 848.40000000000009)]
        [TestCase("2018/5/7", 848.40000000000009)]
        [TestCase("2020/5/1", 1102.2)]
        public void BankAccountValuesTest(DateTime date, double expectedValue)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithSecondaryBankAccount();
            var portfolio = constructor.database;
            Assert.AreEqual(expectedValue, portfolio.TotalValue(AccountType.BankAccount, date));
        }

        //Value and TotalValue use different methods to create the value. TotalValue for bank accounts uses
        // <see cref="NearestEarlierValuation"/>, but Value uses a linear interpolation, hence hte difference.
        //[TestCase("2009/1/2")]
        [TestCase("2010/1/1")]
        [TestCase("2010/1/2")]
        //[TestCase("2015/1/2")]
        //[TestCase("2018/1/1")]
        //[TestCase("2018/5/5")]
        [TestCase("2018/5/6")]
        //[TestCase("2018/5/7")]
        [TestCase("2020/5/1")]
        public void BankAccountTotalValueSingleValueEquivalentTest(DateTime date)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            var portfolio = constructor.database;
            Assert.AreEqual(portfolio.TotalValue(AccountType.BankAccount, date), portfolio.Value(AccountType.BankAccount, constructor.DefaultNameQuery(AccountType.BankAccount), date));
        }
    }
}
