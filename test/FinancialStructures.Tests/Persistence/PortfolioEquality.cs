using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.FinanceStructures;
using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Persistence
{
    internal static class PortfolioEquality
    {
        internal static void AreEqual(IPortfolio expected, IPortfolio actual)
        {
            if (expected == null | actual == null)
            {
                Assert.That(expected == null && actual == null, Is.True);
            }

            Assert.That(actual.Name, Is.EqualTo(expected.Name));

            if (expected.Funds.Count == actual.Funds.Count)
            {
                for (int i = 0; i < expected.Funds.Count; i++)
                {
                    ISecurity expectedSec = expected.Funds[i];
                    ISecurity actualSec = actual.Funds[i];
                    Assert.That(actualSec.Names, Is.EqualTo(expectedSec.Names));
                }
            }
            else
            {
                Assert.That(actual.Funds, Has.Count.EqualTo(expected.Funds.Count), "Funds dont have the same number.");
            }

            if (expected.BankAccounts.Count == actual.BankAccounts.Count)
            {
                for (int i = 0; i < expected.BankAccounts.Count; i++)
                {
                    IExchangeableValueList expectedSec = expected.BankAccounts[i];
                    IExchangeableValueList actualSec = actual.BankAccounts[i];
                    Assert.That(actualSec.Names, Is.EqualTo(expectedSec.Names));
                }
            }
            else
            {
                Assert.That(actual.BankAccounts, Has.Count.EqualTo(expected.BankAccounts.Count), "BankAccounts dont have the same number.");
            }

            if (expected.Currencies.Count == actual.Currencies.Count)
            {
                for (int i = 0; i < expected.Currencies.Count; i++)
                {
                    ICurrency expectedSec = expected.Currencies[i];
                    ICurrency actualSec = actual.Currencies[i];
                    Assert.That(actualSec.Names, Is.EqualTo(expectedSec.Names));
                }
            }
            else
            {
                Assert.That(actual.Currencies.Count, Is.EqualTo(expected.Currencies.Count), "Currencies dont have the same number.");
            }

            if (expected.BenchMarks.Count == actual.BenchMarks.Count)
            {
                for (int i = 0; i < expected.BenchMarks.Count; i++)
                {
                    IValueList expectedBenchMark = expected.BenchMarks[i];
                    IValueList actualBenchMark = actual.BenchMarks[i];
                    Assert.That(actualBenchMark.Names, Is.EqualTo(expectedBenchMark.Names));
                }
            }
            else
            {
                Assert.That(actual.BenchMarks.Count, Is.EqualTo(expected.BenchMarks.Count), "Funds dont have the same number.");
            }

            Assert.That(actual.BaseCurrency, Is.EqualTo(expected.BaseCurrency));
        }
    }
}
