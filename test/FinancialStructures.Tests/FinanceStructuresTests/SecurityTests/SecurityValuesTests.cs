using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Tests.TestDatabaseConstructor;

using FinancialStructures.FinanceStructures;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.FinanceStructuresTests.SecurityTests
{
    [TestFixture]
    public sealed class SecurityValuesTests
    {
        private static IEnumerable<TestCaseData> LastInvestmentData()
        {
            yield return new TestCaseData(
                SecurityConstructor.Empty(),
                null)
                .SetName("LastInvestment-NoEntry");

            yield return new TestCaseData(
                SecurityConstructor.Default(),
                new DailyValuation(new DateTime(2010, 1, 1), 200m))
                .SetName("LastInvestment-DefaultSec");


            yield return new TestCaseData(
                SecurityConstructor.Secondary(),
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
