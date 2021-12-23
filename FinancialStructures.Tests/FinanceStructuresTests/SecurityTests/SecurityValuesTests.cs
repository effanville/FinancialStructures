using System;
using System.Collections.Generic;

using Common.Structure.DataStructures;

using FinancialStructures.FinanceStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;

using NUnit.Framework;

namespace FinancialStructures.Tests.FinanceStructuresTests.SecurityTests
{
    [TestFixture]
    public sealed class SecurityValuesTests
    {
        private static IEnumerable<TestCaseData> LastInvestmentData()
        {
            yield return new TestCaseData(
                SecurityConstructor.Empty().Item,
                null)
                .SetName("LastInvestment-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default().Item,
                new DailyValuation(new DateTime(2010, 1, 1), 200m))
                .SetName("LastInvestment-DefaultSec");


            yield return new TestCaseData(
                SecurityConstructor.Secondary().Item,
                new DailyValuation(new DateTime(2012, 5, 5), 21022.96m))
                .SetName("LastInvestment-SecondarySec");
        }

        [TestCaseSource(nameof(LastInvestmentData))]
        public void LastInvestmentTests(ISecurity valueList, DailyValuation expectedValue)
        {
            DailyValuation actualValue = valueList.LastInvestment();

            Assert.Multiple(() =>
            {
                if (actualValue != null)
                {
                    Assert.AreEqual(expectedValue.Day, actualValue.Day);
                    Assert.AreEqual(expectedValue.Value, actualValue.Value);
                }
                else
                {
                    Assert.IsNull(expectedValue);
                }
            });
        }
    }
}
