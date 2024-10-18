using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

using Effanville.Common.Structure.ReportWriting;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Export.Statistics;
using Effanville.FinancialStructures.Database.Statistics;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database.Export
{
    [TestFixture]
    public sealed class PortfolioStatisticsTests
    {
        private const string Test =
@"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>0.95</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£1,102.20</td><td>£253.80</td><td>0.04</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£27,186.30</td><td>-£14,299.88</td><td>1</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.98</td><td>1.22</td><td>36.48</td><td>36.48</td><td>2010-01-05</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>6</td><td>0.6</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>18.3</td><td>10.76</td><td>74.32</td><td>83.26</td><td>2010-01-01</td><td>2010-01-01</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>0.95</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>1.26</td><td>1.51</td><td>35.94</td><td>35.94</td><td>2010-01-01</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Bank Account Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Santander</th><td>Current</td><td></td><td>£101.10</td><td>£23.40</td><td>0.09</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Halifax</th><td>Current</td><td>HKD</td><td>£1,001.10</td><td>£230.40</td><td>0.9</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>BankAccount</td><td></td><td>£1,102.20</td><td>£253.80</td><td>0.04</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td>0001-01-01</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
</tbody>
</table>
<h2>Asset Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>RecentChange</th><th>Investment</th><th>Profit</th><th>Debt</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Asset</td><td>£0.00</td><td>£0.00</td><td>£0.00</td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
</tbody>
</table>
<h2>Analysis By Sector</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>RecentChange</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>NumberOfAccounts</th><th>FirstDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
</tbody>
</table>
<h2>Portfolio Notes</h2>
";

        [Test]
        public void CanCreateStatistics()
        {
            IPortfolio portfolio = TestDatabase.Databases[TestDatabaseName.TwoSecTwoBank];
            PortfolioStatisticsSettings settings = PortfolioStatisticsSettings.DefaultSettings();
            settings.DateToCalculate = new DateTime(2021, 12, 19);
            PortfolioStatistics portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            PortfolioStatisticsExportSettings exportSettings = PortfolioStatisticsExportSettings.DefaultSettings();
            ReportBuilder statsString = portfolioStatistics.ExportString(includeHtmlHeaders: false, DocumentType.Html, exportSettings);
            string actual = statsString.ToString();
            Assert.That(actual, Is.EqualTo(Test));
        }

        private const string FilteredOutput =
@"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>0.95</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£1,102.20</td><td>£253.80</td><td>0.04</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£27,186.30</td><td>-£14,299.88</td><td>1</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Security</th><td>Prudential</td><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0.98</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>1</td><td>2010-01-05</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td>6</td><td>0.6</td><td></td>
</tr>
<tr>
<th scope=""row"">Security</th><td>BlackRock</td><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>18.3</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>1</td><td>2010-01-01</td><td>2010-01-01</td><td>2010-01-01</td><td>2020-01-01</td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Security</th><td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>0.95</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>1.26</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>2</td><td>2010-01-01</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Bank Account Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Santander</th><td>Current</td><td></td><td>£101.10</td><td>£23.40</td><td>0.09</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Halifax</th><td>Current</td><td>HKD</td><td>£1,001.10</td><td>£230.40</td><td>0.9</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>BankAccount</td><td></td><td>£1,102.20</td><td>£253.80</td><td>0.04</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Pension</th><td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td></td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td>0001-01-01</td><td>0001-01-01</td><td>0</td><td>-0</td><td></td>
</tr>
</tbody>
</table>
<h2>Analysis By Sector</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>RecentChange</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>NumberOfAccounts</th><th>FirstDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
</tbody>
</table>
<h2>Portfolio Notes</h2>
";

        [Test]
        public void CanSortOnNonDisplayedStatistics()
        {
            IPortfolio portfolio = TestDatabase.Databases[TestDatabaseName.TwoSecTwoBank];
            PortfolioStatisticsSettings settings = new PortfolioStatisticsSettings(
                DateTime.Today,
                displayValueFunds: false,
                generateBenchmarks: true,
                includeSecurities: true,
                securityDisplayFields: AccountStatisticsHelpers.AllStatistics().ToList(),
                includeBankAccounts: true,
                bankAccDisplayFields: AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                includeSectors: true,
                sectorDisplayFields: AccountStatisticsHelpers.DefaultSectorStats().ToList(),
                includeAssets: false,
                assetDisplayFields: AccountStatisticsHelpers.DefaultAssetStats().ToList());
            settings.DateToCalculate = new DateTime(2021, 12, 19);
            PortfolioStatistics portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            PortfolioStatisticsExportSettings exportSettings = new PortfolioStatisticsExportSettings(
                spacing: false,
                colours: false,
                includeSecurities: true,
                Statistic.UnitPrice,
                SortDirection.Descending,
                new List<Statistic>
                {
                    Statistic.Company,
                    Statistic.Name,
                    Statistic.Currency,
                    Statistic.LatestValue,
                    Statistic.Profit,
                    Statistic.IRR3M,
                    Statistic.IRR6M,
                    Statistic.IRR1Y,
                    Statistic.IRR5Y
                },
                includeBankAccounts: true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                includeSectors: true,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultSectorStats().ToList(),
                includeAssets: false,
                Statistic.Company,
                SortDirection.Descending,
                AccountStatisticsHelpers.DefaultAssetStats().ToList());
            ReportBuilder statsString = portfolioStatistics.ExportString(includeHtmlHeaders: false, DocumentType.Html, exportSettings);
            string actual = statsString.ToString();
            Assert.That(actual, Is.EqualTo(FilteredOutput));
        }

        private static IEnumerable<TestCaseData> SortSecurityData()
        {
            yield return new TestCaseData(Statistic.Company, SortDirection.Ascending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>18.3</td><td>10.76</td><td>74.32</td><td>83.26</td><td>2010-01-01</td><td>2010-01-01</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.98</td><td>1.22</td><td>36.48</td><td>36.48</td><td>2010-01-05</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>6</td><td>0.6</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>1.26</td><td>1.51</td><td>35.94</td><td>35.94</td><td>2010-01-01</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td>0001-01-01</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.Company, SortDirection.Descending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.98</td><td>1.22</td><td>36.48</td><td>36.48</td><td>2010-01-05</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>6</td><td>0.6</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>18.3</td><td>10.76</td><td>74.32</td><td>83.26</td><td>2010-01-01</td><td>2010-01-01</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>1.26</td><td>1.51</td><td>35.94</td><td>35.94</td><td>2010-01-01</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td>0001-01-01</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.LatestValue, SortDirection.Ascending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>18.3</td><td>10.76</td><td>74.32</td><td>83.26</td><td>2010-01-01</td><td>2010-01-01</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.98</td><td>1.22</td><td>36.48</td><td>36.48</td><td>2010-01-05</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>6</td><td>0.6</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>1.26</td><td>1.51</td><td>35.94</td><td>35.94</td><td>2010-01-01</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td>0001-01-01</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.LatestValue, SortDirection.Descending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>FirstDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>0</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td>-£14,553.68</td><td>1</td><td>1</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.98</td><td>1.22</td><td>36.48</td><td>36.48</td><td>2010-01-05</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>6</td><td>0.6</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>18.3</td><td>10.76</td><td>74.32</td><td>83.26</td><td>2010-01-01</td><td>2010-01-01</td><td>2010-01-01</td><td>2020-01-01</td><td></td><td>6</td><td>0.59</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>1.26</td><td>1.51</td><td>35.94</td><td>35.94</td><td>2010-01-01</td><td>2012-05-05</td><td>2012-05-05</td><td>2020-01-01</td><td></td><td>11</td><td>1.09</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>Sectors</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td><td>9999-12-31</td><td>0001-01-01</td><td>0001-01-01</td><td>0001-01-01</td><td></td><td>0</td><td>-0</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
        }

        [TestCaseSource(nameof(SortSecurityData))]
        public void CanSortSecurityCorrectly(Statistic securitySortField, SortDirection securitySortDirection, string expectedOutput)
        {
            IPortfolio portfolio = TestDatabase.Databases[TestDatabaseName.TwoSec];
            PortfolioStatisticsSettings settings = new PortfolioStatisticsSettings(
                new DateTime(2021, 12, 19),
                displayValueFunds: true,
                generateBenchmarks: true,
                includeSecurities: true,
                securityDisplayFields: AccountStatisticsHelpers.DefaultSecurityStats().ToList(),
                includeBankAccounts: false,
                bankAccDisplayFields: AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                includeSectors: false,
                sectorDisplayFields: AccountStatisticsHelpers.DefaultSectorStats().ToList(),
                includeAssets: false,
                assetDisplayFields: AccountStatisticsHelpers.DefaultAssetStats().ToList());
            PortfolioStatistics portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            PortfolioStatisticsExportSettings exportSettings = new PortfolioStatisticsExportSettings(
                spacing: false,
                colours: false,
                includeSecurities: true,
                securitySortField: securitySortField,
                securitySortDirection: securitySortDirection,
                securityDisplayFields: AccountStatisticsHelpers.DefaultSecurityStats().ToList(),
                includeBankAccounts: false,
                Statistic.NumberOfAccounts,
                SortDirection.Ascending,
                bankAccDisplayFields: AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                includeSectors: false,
                Statistic.NumberOfAccounts,
                SortDirection.Ascending,
                sectorDisplayFields: AccountStatisticsHelpers.DefaultSectorStats().ToList(),
                includeAssets: false,
                Statistic.NumberOfAccounts,
                SortDirection.Ascending,
                assetDisplayFields: AccountStatisticsHelpers.DefaultAssetStats().ToList());
            ReportBuilder statsString = portfolioStatistics.ExportString(false, DocumentType.Html, exportSettings);

            string actualOutput = statsString.ToString();
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
