using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Extensions.DataEdit;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.Persistence;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Saving
{
    [TestFixture]
    public sealed class SavingTests
    {
        private static IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> OldStyleTestLists()
        {
            yield return ("empty", new DatabaseConstructor().SetName("saved").GetInstance(),
   @"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved.xml"">
    <Funds />
    <BankAccounts />
    <Currencies />
    <BenchMarks />
    <Assets />
</MyFunds>
</AllData>");
            Portfolio portfolio = new DatabaseConstructor().SetName("saved").GetInstance();
            _ = portfolio.TryAdd(Account.Security, new NameData("company", "name"));
            _ = portfolio.TryAdd(Account.BankAccount, new NameData("bank", "account"));
            _ = portfolio.TryAdd(Account.Currency, new NameData("gbp", "hkd"));
            _ = portfolio.TryAdd(Account.Benchmark, new NameData("first", "last"));
            yield return ("AccountsNoData", portfolio,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved"">
    <Funds>
      <Security>
        <Names>
          <Company>company</Company>
          <Name>name</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
        <Shares>
          <Values />
        </Shares>
        <UnitPrice>
          <Values />
        </UnitPrice>
        <Investments>
          <Values />
        </Investments>
        <SecurityTrades />
       </Security>
    </Funds>
    <BankAccounts>
      <CashAccount>
        <Names>
          <Company>bank</Company>
          <Name>account</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
        <Amounts>
          <Values />
        </Amounts>
      </CashAccount>
    </BankAccounts>
    <Currencies>
      <Currency>
        <Names>
          <Company>gbp</Company>
          <Name>hkd</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </Currency>
    </Currencies>
    <BenchMarks>
      <Sector>
        <Names>
          <Company>first</Company>
          <Name>last</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </Sector>
    </BenchMarks>
    <Assets />
    <Pensions />
  </MyFunds>
</AllData>");
            Portfolio portfolioWithData = new DatabaseConstructor().SetName("saved").GetInstance();
            TwoName name = new TwoName("company", "name");
            _ = portfolioWithData.TryAdd(Account.Security, new NameData("company", "name", "GBP", "http://temp.com", new HashSet<string>() { "UK", "China" }, "some information"));
            _ = portfolioWithData.TryAddOrEditTradeData(Account.Security, name, new SecurityTrade(new DateTime(2015, 1, 1)), new SecurityTrade(TradeType.Buy, name, new DateTime(2015, 1, 1), 5, 1.2m, 0.0m));
            _ = portfolioWithData.TryAddOrEditData(Account.Security, name, new DailyValuation(new DateTime(2015, 1, 1), 1.2m), new DailyValuation(new DateTime(2015, 1, 1), 1.2m));
            _ = portfolioWithData.TryAdd(Account.BankAccount, new NameData("bank", "account"));
            _ = portfolioWithData.TryAdd(Account.Currency, new NameData("gbp", "hkd"));
            _ = portfolioWithData.TryAdd(Account.Benchmark, new NameData("first", "last"));
            yield return ("AccountsWithData", portfolioWithData, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<AllData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <MyFunds>\r\n    <FilePath>c:/temp/saved.xml</FilePath>\r\n    <Funds>\r\n      <Security>\r\n        <Names>\r\n          <Company>company</Company>\r\n          <Name>name</Name>\r\n          <Url>http://temp.com</Url>\r\n          <Currency>GBP</Currency>\r\n          <Sectors>\r\n            <string>UK</string>\r\n            <string>China</string>\r\n          </Sectors>\r\n          <Notes>some information</Notes>\r\n        </Names>\r\n        <Values>\r\n          <Values />\r\n        </Values>\r\n        <Shares>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2015-01-01T00:00:00</Day>\r\n              <Value>5</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Shares>\r\n        <UnitPrice>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2015-01-01T00:00:00</Day>\r\n              <Value>1.2</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </UnitPrice>\r\n        <Investments>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2015-01-01T00:00:00</Day>\r\n              <Value>6</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Investments>\r\n        <SecurityTrades />\r\n      </Security>\r\n    </Funds>\r\n    <BankAccounts>\r\n      <CashAccount>\r\n        <Names>\r\n          <Company>bank</Company>\r\n          <Name>account</Name>\r\n          <Sectors />\r\n        </Names>\r\n        <Values>\r\n          <Values />\r\n        </Values>\r\n        <Amounts>\r\n          <Values />\r\n        </Amounts>\r\n      </CashAccount>\r\n    </BankAccounts>\r\n    <Currencies>\r\n      <Currency>\r\n        <Names>\r\n          <Company>gbp</Company>\r\n          <Name>hkd</Name>\r\n          <Sectors />\r\n        </Names>\r\n        <Values>\r\n          <Values />\r\n        </Values>\r\n      </Currency>\r\n    </Currencies>\r\n    <BenchMarks>\r\n      <Sector>\r\n        <Names>\r\n          <Company>first</Company>\r\n          <Name>last</Name>\r\n          <Sectors />\r\n        </Names>\r\n        <Values>\r\n          <Values />\r\n        </Values>\r\n      </Sector>\r\n    </BenchMarks>\r\n  </MyFunds>\r\n</AllData>");

            Dictionary<TestDatabaseName, IPortfolio> testDatabases = TestDatabase.Databases;
            yield return (TestDatabaseName.OneBank.ToString(),
                testDatabases[TestDatabaseName.OneBank], "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<AllData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <MyFunds>\r\n    <FilePath>c:/temp/saved.xml</FilePath>\r\n    <BaseCurrency>GBP</BaseCurrency>\r\n    <Funds />\r\n    <BankAccounts>\r\n      <CashAccount>\r\n        <Names>\r\n          <Company>Santander</Company>\r\n          <Name>Current</Name>\r\n          <Sectors />\r\n        </Names>\r\n        <Values>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>125.2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>90.6</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>77.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>101.1</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Values>\r\n        <Amounts>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>125.2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>90.6</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>77.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>101.1</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Amounts>\r\n      </CashAccount>\r\n    </BankAccounts>\r\n    <Currencies />\r\n    <BenchMarks />\r\n  </MyFunds>\r\n</AllData>");
            yield return (TestDatabaseName.OneSec.ToString(), testDatabases[TestDatabaseName.OneSec], "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<AllData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <MyFunds>\r\n    <FilePath>c:/temp/saved.xml</FilePath>\r\n    <BaseCurrency>GBP</BaseCurrency>\r\n    <Funds>\r\n      <Security>\r\n        <Names>\r\n          <Company>BlackRock</Company>\r\n          <Name>UK Stock</Name>\r\n          <Sectors />\r\n        </Names>\r\n        <Values>\r\n          <Values />\r\n        </Values>\r\n        <Shares>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>1.5</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>17.3</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>4</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>5.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>5.5</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Shares>\r\n        <UnitPrice>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>125.2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>90.6</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>77.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>101.1</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </UnitPrice>\r\n        <Investments>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>200</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Investments>\r\n      </Security>\r\n    </Funds>\r\n    <BankAccounts />\r\n    <Currencies />\r\n    <BenchMarks />\r\n  </MyFunds>\r\n</AllData>");
            yield return (TestDatabaseName.OneSecOneBank.ToString(), testDatabases[TestDatabaseName.OneSecOneBank], "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<AllData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <MyFunds>\r\n    <FilePath>c:/temp/saved.xml</FilePath>\r\n    <BaseCurrency>GBP</BaseCurrency>\r\n    <Funds>\r\n      <Security>\r\n        <Names>\r\n          <Company>BlackRock</Company>\r\n          <Name>UK Stock</Name>\r\n          <Sectors />\r\n        </Names>\r\n        <Values>\r\n          <Values />\r\n        </Values>\r\n        <Shares>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>1.5</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>17.3</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>4</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>5.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>5.5</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Shares>\r\n        <UnitPrice>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>125.2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>90.6</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>77.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>101.1</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </UnitPrice>\r\n        <Investments>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Investments>\r\n      </Security>\r\n    </Funds>\r\n    <BankAccounts>\r\n      <CashAccount>\r\n        <Names>\r\n          <Company>Santander</Company>\r\n          <Name>Current</Name>\r\n          <Sectors />\r\n        </Names>\r\n        <Values>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>125.2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>90.6</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>77.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>101.1</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Values>\r\n        <Amounts>\r\n          <Values>\r\n            <DailyValuation>\r\n              <Day>2010-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2011-01-01T00:00:00</Day>\r\n              <Value>100</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2012-05-01T00:00:00</Day>\r\n              <Value>125.2</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2015-04-03T00:00:00</Day>\r\n              <Value>90.6</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2018-05-06T00:00:00</Day>\r\n              <Value>77.7</Value>\r\n            </DailyValuation>\r\n            <DailyValuation>\r\n              <Day>2020-01-01T00:00:00</Day>\r\n              <Value>101.1</Value>\r\n            </DailyValuation>\r\n          </Values>\r\n        </Amounts>\r\n      </CashAccount>\r\n    </BankAccounts>\r\n    <Currencies />\r\n    <BenchMarks />\r\n  </MyFunds>\r\n</AllData>");
        }

        private static IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> NewStyleTestLists()
        {
            yield return ("empty",
                new DatabaseConstructor().SetName("saved").GetInstance(),
            @"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved"">
    <Funds />
    <BankAccounts />
    <Currencies />
    <BenchMarks />
    <Assets />
    <Pensions />
    <Notes />
  </MyFunds>
</AllData>");
            Portfolio portfolio = new DatabaseConstructor().SetName("saved").GetInstance();
            _ = portfolio.TryAdd(Account.Security, new NameData("company", "name"));
            _ = portfolio.TryAdd(Account.BankAccount, new NameData("bank", "account"));
            _ = portfolio.TryAdd(Account.Currency, new NameData("gbp", "hkd"));
            _ = portfolio.TryAdd(Account.Benchmark, new NameData("first", "last"));
            yield return ("AccountsNoData",
                portfolio,
   @"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved"">
    <Funds>
      <Security>
        <Names>
          <Company>company</Company>
          <Name>name</Name>
          <Sectors />
        </Names>
        <Shares>
          <Values />
        </Shares>
        <UnitPrice>
          <Values />
        </UnitPrice>
        <Investments>
          <Values />
        </Investments>
        <SecurityTrades />
      </Security>
    </Funds>
    <BankAccounts>
      <CashAccount>
        <Names>
          <Company>bank</Company>
          <Name>account</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </CashAccount>
    </BankAccounts>
    <Currencies>
      <Currency>
        <Names>
          <Company>gbp</Company>
          <Name>hkd</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </Currency>
    </Currencies>
    <BenchMarks>
      <Sector>
        <Names>
          <Company>first</Company>
          <Name>last</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </Sector>
    </BenchMarks>
    <Assets />
    <Pensions />
    <Notes />
  </MyFunds>
</AllData>");
            Portfolio portfolioWithData = new DatabaseConstructor().SetName("saved").GetInstance();
            _ = portfolioWithData.TryAdd(Account.Security, new NameData("company", "name", "GBP", "http://temp.com", new HashSet<string>() { "UK", "China" }, "some information"));
            _ = portfolioWithData.TryAddOrEditTradeData(Account.Security, new TwoName("company", "name"), new SecurityTrade(new DateTime(2015, 1, 1)), new SecurityTrade(TradeType.Buy, new TwoName("company", "name"), new DateTime(2015, 1, 1), 5, 1.2m, 0));
            _ = portfolioWithData.TryAddOrEditData(Account.Security, new TwoName("company", "name"), new DailyValuation(new DateTime(2015, 1, 1), 1.2m), new DailyValuation(new DateTime(2015, 1, 1), 1.2m));
            _ = portfolioWithData.TryAdd(Account.BankAccount, new NameData("bank", "account"));
            _ = portfolioWithData.TryAdd(Account.Currency, new NameData("gbp", "hkd"));
            _ = portfolioWithData.TryAdd(Account.Benchmark, new NameData("first", "last"));
            yield return ("AccountsWithData", portfolioWithData,
   @"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved"">
    <Funds>
      <Security>
        <Names>
          <Company>company</Company>
          <Name>name</Name>
          <Url>http://temp.com</Url>
          <Currency>GBP</Currency>
          <Sectors>
            <string>UK</string>
            <string>China</string>
          </Sectors>
          <Notes>some information</Notes>
        </Names>
        <Shares>
          <Values>
            <DV D=""2015-01-01T00:00:00"" V=""5.0"" />
          </Values>
        </Shares>
        <UnitPrice>
          <Values>
            <DV D=""2015-01-01T00:00:00"" V=""1.2"" />
          </Values>
        </UnitPrice>
        <Investments>
          <Values>
            <DV D=""2015-01-01T00:00:00"" V=""6.00"" />
          </Values>
        </Investments>
        <SecurityTrades>
          <SecurityTrade TradeType=""Buy"" Company=""company"" Name=""name"" Day=""2015-01-01T00:00:00"" NumberShares=""5"" UnitPrice=""1.2"" TradeCosts=""0"" />
        </SecurityTrades>
      </Security>
    </Funds>
    <BankAccounts>
      <CashAccount>
        <Names>
          <Company>bank</Company>
          <Name>account</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </CashAccount>
    </BankAccounts>
    <Currencies>
      <Currency>
        <Names>
          <Company>gbp</Company>
          <Name>hkd</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </Currency>
    </Currencies>
    <BenchMarks>
      <Sector>
        <Names>
          <Company>first</Company>
          <Name>last</Name>
          <Sectors />
        </Names>
        <Values>
          <Values />
        </Values>
      </Sector>
    </BenchMarks>
    <Assets />
    <Pensions />
    <Notes />
  </MyFunds>
</AllData>");

            Dictionary<TestDatabaseName, IPortfolio> testDatabases = TestDatabase.Databases;
            yield return (TestDatabaseName.OneBank.ToString(), testDatabases[TestDatabaseName.OneBank],
@"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved"">
    <BaseCurrency>GBP</BaseCurrency>
    <Funds />
    <BankAccounts>
      <CashAccount>
        <Names>
          <Company>Santander</Company>
          <Name>Current</Name>
          <Sectors />
        </Names>
        <Values>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2011-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2012-05-01T00:00:00"" V=""125.2"" />
            <DV D=""2015-04-03T00:00:00"" V=""90.6"" />
            <DV D=""2018-05-06T00:00:00"" V=""77.7"" />
            <DV D=""2020-01-01T00:00:00"" V=""101.1"" />
          </Values>
        </Values>
      </CashAccount>
    </BankAccounts>
    <Currencies />
    <BenchMarks />
    <Assets />
    <Pensions />
    <Notes />
  </MyFunds>
</AllData>");
            yield return (TestDatabaseName.OneSec.ToString(), testDatabases[TestDatabaseName.OneSec],
@"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved"">
    <BaseCurrency>GBP</BaseCurrency>
    <Funds>
      <Security>
        <Names>
          <Company>BlackRock</Company>
          <Name>UK Stock</Name>
          <Sectors />
        </Names>
        <Shares>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""2.0"" />
            <DV D=""2011-01-01T00:00:00"" V=""1.5"" />
            <DV D=""2012-05-01T00:00:00"" V=""17.3"" />
            <DV D=""2015-04-03T00:00:00"" V=""4"" />
            <DV D=""2018-05-06T00:00:00"" V=""5.7"" />
            <DV D=""2020-01-01T00:00:00"" V=""5.5"" />
          </Values>
        </Shares>
        <UnitPrice>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2011-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2012-05-01T00:00:00"" V=""125.2"" />
            <DV D=""2015-04-03T00:00:00"" V=""90.6"" />
            <DV D=""2018-05-06T00:00:00"" V=""77.7"" />
            <DV D=""2020-01-01T00:00:00"" V=""101.1"" />
          </Values>
        </UnitPrice>
        <Investments>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""200.000"" />
          </Values>
        </Investments>
        <SecurityTrades>
          <SecurityTrade TradeType=""Buy"" Company=""BlackRock"" Name=""UK Stock"" Day=""2010-01-01T00:00:00"" NumberShares=""2.0"" UnitPrice=""100.0"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2011-01-01T00:00:00"" NumberShares=""1.5"" UnitPrice=""100.0"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2012-05-01T00:00:00"" NumberShares=""17.3"" UnitPrice=""125.2"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2015-04-03T00:00:00"" NumberShares=""4"" UnitPrice=""90.6"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2018-05-06T00:00:00"" NumberShares=""5.7"" UnitPrice=""77.7"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2020-01-01T00:00:00"" NumberShares=""5.5"" UnitPrice=""101.1"" TradeCosts=""0.0"" />
        </SecurityTrades>
      </Security>
    </Funds>
    <BankAccounts />
    <Currencies />
    <BenchMarks />
    <Assets />
    <Pensions />
    <Notes />
  </MyFunds>
</AllData>");
            yield return (TestDatabaseName.OneSecOneBank.ToString(), testDatabases[TestDatabaseName.OneSecOneBank],
@"<?xml version=""1.0"" encoding=""utf-8""?>
<AllData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MyFunds Name=""saved"">
    <BaseCurrency>GBP</BaseCurrency>
    <Funds>
      <Security>
        <Names>
          <Company>BlackRock</Company>
          <Name>UK Stock</Name>
          <Sectors />
        </Names>
        <Shares>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""2.0"" />
            <DV D=""2011-01-01T00:00:00"" V=""1.5"" />
            <DV D=""2012-05-01T00:00:00"" V=""17.3"" />
            <DV D=""2015-04-03T00:00:00"" V=""4"" />
            <DV D=""2018-05-06T00:00:00"" V=""5.7"" />
            <DV D=""2020-01-01T00:00:00"" V=""5.5"" />
          </Values>
        </Shares>
        <UnitPrice>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2011-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2012-05-01T00:00:00"" V=""125.2"" />
            <DV D=""2015-04-03T00:00:00"" V=""90.6"" />
            <DV D=""2018-05-06T00:00:00"" V=""77.7"" />
            <DV D=""2020-01-01T00:00:00"" V=""101.1"" />
          </Values>
        </UnitPrice>
        <Investments>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""200.000"" />
          </Values>
        </Investments>
        <SecurityTrades>
          <SecurityTrade TradeType=""Buy"" Company=""BlackRock"" Name=""UK Stock"" Day=""2010-01-01T00:00:00"" NumberShares=""2.0"" UnitPrice=""100.0"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2011-01-01T00:00:00"" NumberShares=""1.5"" UnitPrice=""100.0"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2012-05-01T00:00:00"" NumberShares=""17.3"" UnitPrice=""125.2"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2015-04-03T00:00:00"" NumberShares=""4"" UnitPrice=""90.6"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2018-05-06T00:00:00"" NumberShares=""5.7"" UnitPrice=""77.7"" TradeCosts=""0.0"" />
          <SecurityTrade TradeType=""ShareReset"" Company=""BlackRock"" Name=""UK Stock"" Day=""2020-01-01T00:00:00"" NumberShares=""5.5"" UnitPrice=""101.1"" TradeCosts=""0.0"" />
        </SecurityTrades>
      </Security>
    </Funds>
    <BankAccounts>
      <CashAccount>
        <Names>
          <Company>Santander</Company>
          <Name>Current</Name>
          <Sectors />
        </Names>
        <Values>
          <Values>
            <DV D=""2010-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2011-01-01T00:00:00"" V=""100.0"" />
            <DV D=""2012-05-01T00:00:00"" V=""125.2"" />
            <DV D=""2015-04-03T00:00:00"" V=""90.6"" />
            <DV D=""2018-05-06T00:00:00"" V=""77.7"" />
            <DV D=""2020-01-01T00:00:00"" V=""101.1"" />
          </Values>
        </Values>
      </CashAccount>
    </BankAccounts>
    <Currencies />
    <BenchMarks />
    <Assets />
    <Pensions />
    <Notes />
  </MyFunds>
</AllData>");
        }

        private static IEnumerable<TestCaseData> WriteSerializationData(string testName)
        {
            IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> tests = NewStyleTestLists();
            foreach ((string name, IPortfolio testPortfolio, string XmlString) test in tests)
            {
                yield return new TestCaseData(test.XmlString, test.testPortfolio).SetName($"{testName}-{test.name}");
            }
        }

        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(WriteXmlTests) })]
        public void WriteXmlTests(string expectedXml, IPortfolio times)
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.xml";

            XmlPortfolioPersistence xmlPersistence = new XmlPortfolioPersistence();
            XmlFilePersistenceOptions options = new XmlFilePersistenceOptions(savePath, tempFileSystem);
            xmlPersistence.Save(times, options, null);

            string file = tempFileSystem.File.ReadAllText(savePath);

            Assert.AreEqual(expectedXml, file);
        }

        private static IEnumerable<TestCaseData> ReadSerializationData(string testName)
        {
            IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> oldTests = OldStyleTestLists();
            foreach ((string name, IPortfolio testPortfolio, string XmlString) test in oldTests)
            {
                yield return new TestCaseData(test.XmlString, test.testPortfolio).SetName($"{testName}old-{test.name}");
            }

            IEnumerable<(string name, IPortfolio testPortfolio, string XmlString)> newTests = NewStyleTestLists();
            foreach ((string name, IPortfolio testPortfolio, string XmlString) test in newTests)
            {
                yield return new TestCaseData(test.XmlString, test.testPortfolio).SetName($"{testName}-{test.name}");
            }
        }

        [TestCaseSource(nameof(ReadSerializationData), new object[] { nameof(ReadXmlTests) })]
        public void ReadXmlTests(string expectedXml, IPortfolio times)
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.xml";
            tempFileSystem.AddFile(savePath, new MockFileData(expectedXml));
            XmlPortfolioPersistence xmlPersistence = new XmlPortfolioPersistence();
            IPortfolio loadedPortfolio = xmlPersistence.Load(new XmlFilePersistenceOptions(savePath, tempFileSystem), null);

            AreEqual(times, loadedPortfolio);
        }


        [TestCaseSource(nameof(WriteSerializationData), new object[] { nameof(RoundTripSaveTests) })]
        public void RoundTripSaveTests(string expectedXml, IPortfolio database)
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.xml";

            XmlPortfolioPersistence xmlPersistence = new XmlPortfolioPersistence();
            XmlFilePersistenceOptions options = new XmlFilePersistenceOptions(savePath, tempFileSystem);
            xmlPersistence.Save(database, options, null);

            string file = tempFileSystem.File.ReadAllText(savePath);

            Assert.AreEqual(expectedXml, file);

            IPortfolio loadedPortfolio = xmlPersistence.Load(new XmlFilePersistenceOptions(savePath, tempFileSystem), null);

            AreEqual(database, loadedPortfolio);
        }

        private void AreEqual(IPortfolio expected, IPortfolio actual)
        {
            if (expected == null | actual == null)
            {
                Assert.IsTrue(expected == null && actual == null);
            }

            Assert.AreEqual(expected.Name, actual.Name);

            if (expected.Funds.Count == actual.Funds.Count)
            {
                for (int i = 0; i < expected.Funds.Count; i++)
                {
                    ISecurity expectedSec = expected.Funds[i];
                    ISecurity actualSec = actual.Funds[i];
                    Assert.AreEqual(expectedSec.Names, actualSec.Names);
                }
            }
            else
            {
                Assert.IsTrue(false, "Funds dont have the same number.");
            }

            if (expected.BankAccounts.Count == actual.BankAccounts.Count)
            {
                for (int i = 0; i < expected.BankAccounts.Count; i++)
                {
                    IExchangeableValueList expectedSec = expected.BankAccounts[i];
                    IExchangeableValueList actualSec = actual.BankAccounts[i];
                    Assert.AreEqual(expectedSec.Names, actualSec.Names);
                }
            }
            else
            {
                Assert.IsTrue(false, "Funds dont have the same number.");
            }

            if (expected.Currencies.Count == actual.Currencies.Count)
            {
                for (int i = 0; i < expected.Currencies.Count; i++)
                {
                    ICurrency expectedSec = expected.Currencies[i];
                    ICurrency actualSec = actual.Currencies[i];
                    Assert.AreEqual(expectedSec.Names, actualSec.Names);
                }
            }
            else
            {
                Assert.IsTrue(false, "Funds dont have the same number.");
            }

            if (expected.BenchMarks.Count == actual.BenchMarks.Count)
            {
                for (int i = 0; i < expected.BenchMarks.Count; i++)
                {
                    IValueList expectedBenchMark = expected.BenchMarks[i];
                    IValueList actualBenchMark = actual.BenchMarks[i];
                    Assert.AreEqual(expectedBenchMark.Names, actualBenchMark.Names);
                }
            }
            else
            {
                Assert.IsTrue(false, "Funds dont have the same number.");
            }

            CollectionAssert.AreEqual(expected.BaseCurrency, actual.BaseCurrency);
        }
    }
}
