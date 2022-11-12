using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Common.Structure.ReportWriting;
using FinancialStructures.Database.Export.Statistics;
using FinancialStructures.Database.Statistics;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Export
{
    [TestFixture]
    public sealed class PortfolioStatisticsTests
    {
        private const string test =
@"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£1,102.20</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£27,186.30</td><td></td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>5/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>0.95</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>1/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Bank Account Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Santander</th><td>Current</td><td></td><td>£101.10</td><td></td><td></td>
</tr>
<tr>
<th scope=""row"">Halifax</th><td>Current</td><td>HKD</td><td>£1,001.10</td><td></td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>BankAccount</td><td></td><td>£1,102.20</td><td></td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>0</td><td>0</td><td></td><td>31/12/9999</td><td>1/1/1</td><td>1/1/1</td><td>1/1/1</td><td>0</td><td data-negative>-∞</td><td></td>
</tr>
</tbody>
</table>
<h2>Analysis By Sector</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>RecentChange</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>NumberOfAccounts</th><th>FirstDate</th><th>LatestDate</th><th>Notes</th>
</tr></thead>
<tbody>
</tbody>
</table>
<h2>Portfolio Notes</h2>
";

        [Test]
        public void CanCreateStatistics()
        {
            var portfolio = TestDatabase.Databases[TestDatabaseName.TwoSecTwoBank];
            var settings = PortfolioStatisticsSettings.DefaultSettings();
            settings.DateToCalculate = new DateTime(2021, 12, 19);
            var portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            var exportSettings = PortfolioStatisticsExportSettings.DefaultSettings();
            var statsString = portfolioStatistics.ExportString(includeHtmlHeaders: false, DocumentType.Html, exportSettings);
            string actual = statsString.ToString();
            Assert.AreEqual(test, actual);
        }

        private const string FilteredOutput =
@"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£1,102.20</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£27,186.30</td><td></td><td></td>
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
<th scope=""row"">Security</th><td>Prudential</td><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>1</td><td>5/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">Security</th><td>BlackRock</td><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>1</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Security</th><td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>0.95</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>2</td><td>1/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Bank Account Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Santander</th><td>Current</td><td></td><td>£101.10</td><td></td><td></td>
</tr>
<tr>
<th scope=""row"">Halifax</th><td>Current</td><td>HKD</td><td>£1,001.10</td><td></td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>BankAccount</td><td></td><td>£1,102.20</td><td></td><td></td>
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
<th scope=""row"">Pension</th><td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>0</td><td>0</td><td></td><td>0</td><td>31/12/9999</td><td>1/1/1</td><td>1/1/1</td><td>1/1/1</td><td>0</td><td data-negative>-∞</td><td></td>
</tr>
</tbody>
</table>
<h2>Analysis By Sector</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>RecentChange</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>NumberOfAccounts</th><th>FirstDate</th><th>LatestDate</th><th>Notes</th>
</tr></thead>
<tbody>
</tbody>
</table>
<h2>Portfolio Notes</h2>
";

        [Test]
        public void CanSortOnNonDisplayedStatistics()
        {
            var portfolio = TestDatabase.Databases[TestDatabaseName.TwoSecTwoBank];
            var settings = new PortfolioStatisticsSettings(
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
            var portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            var exportSettings = new PortfolioStatisticsExportSettings(
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
            var statsString = portfolioStatistics.ExportString(includeHtmlHeaders: false, DocumentType.Html, exportSettings);
            string actual = statsString.ToString();
            Assert.AreEqual(FilteredOutput, actual);
        }

        private static IEnumerable<TestCaseData> SortSecurityData()
        {
            yield return new TestCaseData(Statistic.Company, SortDirection.Ascending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>5/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>1/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>0</td><td>0</td><td></td><td>31/12/9999</td><td>1/1/1</td><td>1/1/1</td><td>1/1/1</td><td>0</td><td data-negative>-∞</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.Company, SortDirection.Descending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>5/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>1/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>0</td><td>0</td><td></td><td>31/12/9999</td><td>1/1/1</td><td>1/1/1</td><td>1/1/1</td><td>0</td><td data-negative>-∞</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.LatestValue, SortDirection.Ascending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>5/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>1/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>0</td><td>0</td><td></td><td>31/12/9999</td><td>1/1/1</td><td>1/1/1</td><td>1/1/1</td><td>0</td><td data-negative>-∞</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.LatestValue, SortDirection.Descending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>Sectors</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Pension</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>Asset</td><td></td><td>£0.00</td><td></td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td></td><td>£26,084.10</td><td></td><td></td>
</tr>
</tbody>
</table>
<h2>Fund Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£14,666.84</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>5/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td></td><td>£556.05</td><td>£101.10</td><td>5.5</td><td>£100.00</td><td>£113.16</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£14,553.68</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>1/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Pension Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Totals</th><td>Pension</td><td></td><td>£0.00</td><td>0</td><td>0</td><td>0</td><td>£0.00</td><td>0</td><td>0</td><td>£0.00</td><td>£0.00</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>NaN</td><td>0</td><td>0</td><td></td><td>31/12/9999</td><td>1/1/1</td><td>1/1/1</td><td>1/1/1</td><td>0</td><td data-negative>-∞</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
        }

        [TestCaseSource(nameof(SortSecurityData))]
        public void CanSortSecurityCorrectly(Statistic securitySortField, SortDirection securitySortDirection, string expectedOutput)
        {
            var portfolio = TestDatabase.Databases[TestDatabaseName.TwoSec];
            var settings = new PortfolioStatisticsSettings(
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
            var portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            var exportSettings = new PortfolioStatisticsExportSettings(
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
            var statsString = portfolioStatistics.ExportString(false, DocumentType.Html, exportSettings);

            string actualOutput = statsString.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
        }
    }
}
