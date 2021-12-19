using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Common.Structure.FileAccess;
using FinancialStructures.Database.Export.Statistics;
using FinancialStructures.Database.Statistics;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Export
{
    [TestFixture]
    public sealed class PortfolioStatisticsTests
    {
        private const string test =
@"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""utf-8"" http-equiv=""x-ua-compatible"" content=""IE=11""/>
<title>Statement for funds as of 19/12/2021</title>
<style>
html, h1, h2, h3, h4, h5, h6 { font-family: ""Arial"", cursive, sans-serif; }
h1 { font-family: ""Arial"", cursive, sans-serif; margin-top: 1.5em; }
h2 { font-family: ""Arial"", cursive, sans-serif; margin-top: 1.5em; }
body{ font-family: ""Arial"", cursive, sans-serif; font-size: 10px; }
table { border-collapse: collapse; }
table, th, td { border: 1px solid black; }
caption { margin-bottom: 1.2em; font-family: ""Arial"", cursive, sans-serif; font-size:medium; }
tr { text-align: center; }
div { max-width: 1000px; max-height: 600px; margin: left; margin-bottom: 1.5em; }
th{ height: 1.5em; }
p { line-height: 1.5em; margin-bottom: 1.5em;}
</style>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js""></script>
<script src=""https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js""></script>
</head>
<body>
<h1>saved - Statement on 19/12/2021</h1>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td>£26,084.10</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td>£1,102.20</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td>£27,186.30</td><td></td>
</tr>
</tbody>
</table>
<h2>Funds Data</h2>
<table>
<thead><tr>
<th scope=""col"">AccountType</th><th>Company</th><th>Name</th><th>Currency</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>MeanSharePrice</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>DrawDown</th><th>MDD</th><th>Sectors</th><th>NumberOfAccounts</th><th>FirstDate</th><th>LastInvestmentDate</th><th>LastPurchaseDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Security</th><td>Prudential</td><td>China Stock</td><td>HKD</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>£1,193.94</td><td>-£19,624.80</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td>36.48</td><td>36.48</td><td></td><td>0</td><td>5/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">Security</th><td>BlackRock</td><td>UK Stock</td><td></td><td>£556.05</td><td>101.1</td><td>5.5</td><td>£100.00</td><td>£128.70</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td>74.32</td><td>83.26</td><td></td><td>0</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Security</th><td>Totals</td><td>Security</td><td></td><td>£26,084.10</td><td>0</td><td>0</td><td>0</td><td>-£19,496.10</td><td>0.95</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td>35.94</td><td>35.94</td><td></td><td>0</td><td>1/1/2010</td><td>5/5/2012</td><td>5/5/2012</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Bank Accounts Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Santander</th><td>Current</td><td>£101.10</td><td></td>
</tr>
<tr>
<th scope=""row"">Halifax</th><td>Current</td><td>£1,001.10</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>BankAccount</td><td>£1,102.20</td><td></td>
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
</body>
</html>
";

        [Test]
        public void CanCreateStatistics()
        {
            var portfolio = TestDatabase.Databases[TestDatabaseName.TwoSecTwoBank];
            var settings = PortfolioStatisticsSettings.DefaultSettings();
            var portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            var exportSettings = PortfolioStatisticsExportSettings.DefaultSettings();
            var statsString = portfolioStatistics.ExportString(true, ExportType.Html, exportSettings);
            Assert.AreEqual(test, statsString.ToString());
        }

        private static IEnumerable<TestCaseData> SortSecurityData()
        {
            yield return new TestCaseData(Statistic.Company, SortDirection.Ascending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td>£26,084.10</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td>£0.00</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td>£26,084.10</td><td></td>
</tr>
</tbody>
</table>
<h2>Funds Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>Sectors</th><th>FirstDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td>£556.05</td><td>101.1</td><td>5.5</td><td>£128.70</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>-£19,624.80</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td></td><td>5/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td>£26,084.10</td><td>0</td><td>0</td><td>-£19,496.10</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.Company, SortDirection.Descending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td>£26,084.10</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td>£0.00</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td>£26,084.10</td><td></td>
</tr>
</tbody>
</table>
<h2>Funds Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>Sectors</th><th>FirstDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>-£19,624.80</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td></td><td>5/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td>£556.05</td><td>101.1</td><td>5.5</td><td>£128.70</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td>£26,084.10</td><td>0</td><td>0</td><td>-£19,496.10</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.LatestValue, SortDirection.Ascending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td>£26,084.10</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td>£0.00</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td>£26,084.10</td><td></td>
</tr>
</tbody>
</table>
<h2>Funds Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>Sectors</th><th>FirstDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td>£556.05</td><td>101.1</td><td>5.5</td><td>£128.70</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>-£19,624.80</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td></td><td>5/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td>£26,084.10</td><td>0</td><td>0</td><td>-£19,496.10</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
</tr>
</tbody>
</table>
<h2>Portfolio Notes</h2>
");
            yield return new TestCaseData(Statistic.LatestValue, SortDirection.Descending, @"<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<td>Totals</td><td>Security</td><td>£26,084.10</td><td></td>
</tr>
<tr>
<td>Totals</td><td>BankAccount</td><td>£0.00</td><td></td>
</tr>
<tr>
<td>Totals</td><td>All</td><td>£26,084.10</td><td></td>
</tr>
</tbody>
</table>
<h2>Funds Data</h2>
<table>
<thead><tr>
<th scope=""col"">Company</th><th>Name</th><th>LatestValue</th><th>UnitPrice</th><th>NumberUnits</th><th>RecentChange</th><th>FundFraction</th><th>FundCompanyFraction</th><th>Investment</th><th>Profit</th><th>IRR3M</th><th>IRR6M</th><th>IRR1Y</th><th>IRR5Y</th><th>IRRTotal</th><th>Sectors</th><th>FirstDate</th><th>LatestDate</th><th>NumberEntries</th><th>EntryYearDensity</th><th>Notes</th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Prudential</th><td>China Stock</td><td>£25,528.05</td><td>HK$1,001.10</td><td>25.5</td><td>-£19,624.80</td><td>0.97</td><td>1</td><td>£23,042.96</td><td>£2,485.09</td><td>0</td><td>0</td><td>0</td><td>0.59</td><td>1.22</td><td></td><td>5/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6657</td><td></td>
</tr>
<tr>
<th scope=""row"">BlackRock</th><td>UK Stock</td><td>£556.05</td><td>101.1</td><td>5.5</td><td>£128.70</td><td>0.02</td><td>1</td><td>£200.00</td><td>£356.05</td><td>0</td><td>0</td><td>0</td><td>10.73</td><td>10.76</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>6</td><td>1.6675</td><td></td>
</tr>
<tr>
<th scope=""row"">Totals</th><td>Security</td><td>£26,084.10</td><td>0</td><td>0</td><td>-£19,496.10</td><td>1</td><td>0</td><td>£23,242.96</td><td>£2,841.14</td><td>0</td><td>0</td><td>0</td><td>0.76</td><td>1.51</td><td></td><td>1/1/2010</td><td>1/1/2020</td><td>11</td><td>0.9095</td><td></td>
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
                includeBenchmarks: true,
                displayValueFunds: true,
                includeSecurities: true,
                securitySortField,
                securitySortDirection,
                AccountStatisticsHelpers.DefaultSecurityStats().ToList(),
                includeBankAccounts: false,
                Statistic.Company,
                SortDirection.Ascending,
                AccountStatisticsHelpers.DefaultBankAccountStats().ToList(),
                includeSectors: false,
                Statistic.Company,
                SortDirection.Ascending,
                AccountStatisticsHelpers.DefaultSectorStats().ToList());
            var portfolioStatistics = new PortfolioStatistics(portfolio, settings, new MockFileSystem());
            var exportSettings = PortfolioStatisticsExportSettings.DefaultSettings();
            var statsString = portfolioStatistics.ExportString(false, ExportType.Html, exportSettings);

            string actualOutput = statsString.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
        }
    }
}
