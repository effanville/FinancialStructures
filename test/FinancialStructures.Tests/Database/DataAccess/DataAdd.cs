using System;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.DataEdit;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.DataAccess
{
    [TestFixture]
    internal class DataAdd
    {
        [Test]
        public void TestAddDataFromOther()
        {
            Portfolio constructor = new DatabaseConstructor()
                .WithDefaultSecurity()
                .WithDefaultBankAccount()
                .WithDefaultCurrency()
                .WithDefaultAsset()
                .GetInstance();
            TwoName defaultSecurityName = DatabaseConstructor.DefaultName(Account.Security);
            TwoName defaultBankAccountName = DatabaseConstructor.DefaultName(Account.BankAccount);
            TwoName defaultCurrencyName = DatabaseConstructor.DefaultName(Account.Currency);
            TwoName defaultAssetName = DatabaseConstructor.DefaultName(Account.Asset);
            DailyValuation newValue = new DailyValuation(new DateTime(2022, 5, 6), 4);
            constructor.TryAddOrEditData(Account.Security, defaultSecurityName, newValue, newValue);
            constructor.TryAddOrEditData(Account.BankAccount, defaultBankAccountName, newValue, newValue);

            DailyValuation newCurrencyValue = new DailyValuation(new DateTime(2022, 5, 6), 11.2m);
            constructor.TryAddOrEditData(Account.Currency, defaultCurrencyName, newCurrencyValue, newCurrencyValue);
            constructor.TryAddOrEditData(Account.Asset, defaultAssetName, newCurrencyValue, newCurrencyValue);

            Portfolio otherDatabase = new DatabaseConstructor()
                .WithDefaultSecurity()
                .WithDefaultBankAccount()
                .WithDefaultCurrency()
                .WithDefaultAsset()
                .GetInstance();

            otherDatabase.ImportValuesFrom(constructor);

            otherDatabase.TryGetAccount(Account.Security, defaultSecurityName, out IValueList secList);
            DailyValuation value = secList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(22, value.Value);

            otherDatabase.TryGetAccount(Account.BankAccount, defaultBankAccountName, out IValueList bankList);
            DailyValuation value2 = bankList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(4, value2.Value);

            otherDatabase.TryGetAccount(Account.Currency, defaultCurrencyName, out IValueList currencyList);
            DailyValuation value3 = currencyList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(11.2m, value3.Value);


            otherDatabase.TryGetAccount(Account.Asset, defaultAssetName, out IValueList assetList);
            DailyValuation value4 = assetList.Value(new DateTime(2022, 5, 6));
            Assert.AreEqual(-19988.80m, value4.Value);
        }
    }
}
