using System;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.DataEdit;
using Effanville.FinancialStructures.FinanceStructures;
using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.DataAccess
{
    [TestFixture]
    internal class DataAdd
    {
        [Test]
        public void TestAddDataFromOther()
        {
            var constructor = new DatabaseConstructor()
                .WithDefaultSecurity()
                .WithDefaultBankAccount()
                .WithDefaultCurrency()
                .WithDefaultAsset()
                .GetInstance();
            var defaultSecurityName = DatabaseConstructor.DefaultName(Account.Security);
            var defaultBankAccountName = DatabaseConstructor.DefaultName(Account.BankAccount);
            var defaultCurrencyName = DatabaseConstructor.DefaultName(Account.Currency);
            var defaultAssetName = DatabaseConstructor.DefaultName(Account.Asset);
            var newValue = new DailyValuation(new DateTime(2022, 5, 6), 4);
            constructor.TryAddOrEditData(Account.Security, defaultSecurityName, newValue, newValue);
            constructor.TryAddOrEditData(Account.BankAccount, defaultBankAccountName, newValue, newValue);

            var newCurrencyValue = new DailyValuation(new DateTime(2022, 5, 6), 11.2m);
            constructor.TryAddOrEditData(Account.Currency, defaultCurrencyName, newCurrencyValue, newCurrencyValue);
            constructor.TryAddOrEditData(Account.Asset, defaultAssetName, newCurrencyValue, newCurrencyValue);

            var otherDatabase = new DatabaseConstructor()
                .WithDefaultSecurity()
                .WithDefaultBankAccount()
                .WithDefaultCurrency()
                .WithDefaultAsset()
                .GetInstance();

            otherDatabase.ImportValuesFrom(constructor);

            otherDatabase.TryGetAccount(Account.Security, defaultSecurityName, out IValueList secList);
            var value = secList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(22, value.Value);

            otherDatabase.TryGetAccount(Account.BankAccount, defaultBankAccountName, out IValueList bankList);
            var value2 = bankList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(4, value2.Value);

            otherDatabase.TryGetAccount(Account.Currency, defaultCurrencyName, out IValueList currencyList);
            var value3 = currencyList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(11.2m, value3.Value);


            otherDatabase.TryGetAccount(Account.Asset, defaultAssetName, out IValueList assetList);
            var value4 = assetList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(-19988.80m, value4.Value);
        }
    }
}
