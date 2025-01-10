using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.FinanceStructuresTests;

public class ValueListCollectionTests
{
    [TestCase(Account.Security, Account.Security, 1, 1)]
    [TestCase(Account.Security, Account.BankAccount, 0, 0)]
    [TestCase(Account.Security, Account.All, 0, 0)]
    [TestCase(Account.Security, Account.Unknown, 0, 0)]
    [TestCase(Account.All, Account.Security, 1, 1)]
    [TestCase(Account.All, Account.BankAccount, 1, 1)]
    [TestCase(Account.All, Account.Unknown, 1, 1)]
    [TestCase(Account.All, Account.All, 1, 1)]
    public void GIVENCollection_THENCanAddValueList(
        Account collectionAccountType,
        Account addAccountType,
        int expectedCalls,
        int expectedElements)
    {
        int numberCalls = 0;
        ValueListCollection<ISecurity, Security> collection = new(
            collectionAccountType,
            (a, n) =>
            {
                numberCalls++;
                return new Security(a, n);
            });

        _ = collection.TryAdd(addAccountType, new NameData("Comp", "Name"));
        Assert.Multiple(() =>
        {
            Assert.That(numberCalls, Is.EqualTo(expectedCalls));
            Assert.That(collection.Values, Has.Count.EqualTo(expectedElements));
        });
    }

    [TestCase(Account.Security, "comp", "name", "comp", "name", false)]
    [TestCase(Account.BankAccount, "comp", "name", "comp", "name", false)]
    [TestCase(Account.All, "comp", "name", "comp", "name", false)]
    [TestCase(Account.Unknown, "comp", "name", "comp", "name", false)]
    [TestCase(Account.Security, "comp", "name", "comp2", "name", true)]
    [TestCase(Account.BankAccount, "comp", "name", "comp2", "name", true)]
    [TestCase(Account.All, "comp", "name", "comp2", "name", true)]
    [TestCase(Account.Unknown, "comp", "name", "comp2", "name", true)]
    [TestCase(Account.Security, "comp", "name", "comp", "name2", true)]
    [TestCase(Account.BankAccount, "comp", "name", "comp", "name2", true)]
    [TestCase(Account.All, "comp", "name", "comp", "name2", true)]
    [TestCase(Account.Unknown, "comp", "name", "comp", "name2", true)]
    public void GIVENCollection_THENCanUpdateKey(
    Account addAccountType,
    string oldCompany,
    string oldName,
    string newCompany,
    string newName,
    bool expectedUpdated)
    {
        ValueListCollection<ISecurity, Security> collection = new(
            addAccountType,
            (a, n) => new Security(a, n));

        NameData oldData = new NameData(oldCompany, oldName);
        _ = collection.TryAdd(addAccountType, oldData);

        bool updated = collection.TryUpdateKey(oldData, new TwoName(newCompany, newName));

        Assert.That(updated, Is.EqualTo(expectedUpdated));
    }

    [Test]
    public void GIVEN_Collection_THENCanGetValueList()
    {
        ValueListCollection<ISecurity, Security> collection = new(
            Account.Security,
            (a, n) => new Security(a, n));

        Security testList = new Security(Account.Security, new NameData("Comp", "Name"));
        collection.AddValueList(testList);

        bool ret = collection.TryGetAndCast(
            new TwoName("Comp", "Name"),
            out ISecurity result);
        Assert.Multiple(() =>
        {
            Assert.That(ret, Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(testList));
        });
    }
}