using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using System.Xml;
using Effanville.FinancialStructures.Persistence.Xml.V2;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Tests.Persistence;

internal class XmlTradeTests
{
    private static string enl = Environment.NewLine;
    private static IEnumerable<TestCaseData> WriteXmlTestCases()
    {
        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Comp", "Name"),
            DateTime.SpecifyKind(new DateTime(2018, 1, 31), DateTimeKind.Utc),
            278.671m,
            123m,
            456m,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-31T00:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");

        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Comp", "Name"),
            new DateTime(2018, 1, 31),
            278.671m,
            123m,
            456m,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-30T16:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");
    }

    [TestCaseSource(nameof(WriteXmlTestCases))]
    public void WriteXmlTests(TradeType type, TwoName names, DateTime day, decimal numShares, decimal price, decimal costs, string expectedXml)
    {
        XmlTrade val = new XmlTrade(new SecurityTrade(type, names, day, numShares, price, costs));
        using (StringWriter fs = new StringWriter())
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = false;
            xmlWriterSettings.Encoding = Encoding.UTF8;
            xmlWriterSettings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
            {
                val.WriteXml(writer);
            }

            string output = fs.ToString();

            Assert.That(output, Is.EqualTo(expectedXml));
        }
    }
    private static IEnumerable<TestCaseData> ReadXmlTestCases()
    {
        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Comp", "Name"),
            new DateTime(2018, 1, 30, 16, 0, 0),
            278.671m,
            123m,
            456m,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-30T16:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");
        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Comp", "Name"),
            new DateTime(2018, 1, 31),
            278.671m,
            123m,
            456m,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-31T00:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");
        yield return new TestCaseData(
            TradeType.Unknown,
            null,
            default,
            0m,
            0m,
            0m,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade />");
        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Principal", "Global"),
            new DateTime(2023, 09, 17),
            124.434m,
            24.109166m,
            0m,
            $"<SecurityTrade TradeType=\"Buy\" Company=\"Principal\" Name=\"Global\" Day=\"2023-09-17T00:00:00+08:00\" NumberShares=\"124.434\" UnitPrice=\"24.109166\" TradeCosts=\"0\" />");
        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Principal", "Global"),
            new DateTime(2023, 09, 17, 2, 0, 0),
            124.434m,
            24.109166m,
            0m,
            $"<SecurityTrade TradeType=\"Buy\" Company=\"Principal\" Name=\"Global\" Day=\"2023-09-17T00:00:00+06:00\" NumberShares=\"124.434\" UnitPrice=\"24.109166\" TradeCosts=\"0\" />");
    }

    [TestCaseSource(nameof(ReadXmlTestCases))]
    public void ReadXmlTests(TradeType type, TwoName names, DateTime day, decimal numShares, decimal price, decimal costs, string actualXml)
    {
        var valuation = new XmlTrade();
        using (StringReader fs = new StringReader(actualXml))
        {
            XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
            using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
            {
                valuation.ReadXml(reader);
            }

            Assert.That(valuation,
                Is.EqualTo(
                    new XmlTrade(new SecurityTrade(type, names, day, numShares, price, costs))));
        }
    }

    private static IEnumerable<TestCaseData> XmlRoundTripViaFileTestCases()
    {
        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Comp", "Name"),
            DateTime.SpecifyKind(new DateTime(2018, 1, 30, 16, 0, 0), DateTimeKind.Utc),
            278.671m,
            123m,
            456m,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-30T08:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");
        yield return new TestCaseData(
            TradeType.Buy,
            new TwoName("Comp", "Name"),
            DateTime.SpecifyKind(new DateTime(2018, 1, 31), DateTimeKind.Utc),
            278.671m,
            123m,
            456m,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-31T00:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");
    }

    [TestCaseSource(nameof(XmlRoundTripViaFileTestCases))]
    public void XmlRoundTripViaFileTests(TradeType type, TwoName names, DateTime day, decimal numShares, decimal price, decimal costs, string actualXml)
    {
        string output;
        XmlTrade val = new XmlTrade(new SecurityTrade(type, names, day, numShares, price, costs));
        using (StringWriter fs = new StringWriter())
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = false;
            xmlWriterSettings.Encoding = Encoding.UTF8;
            xmlWriterSettings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
            {
                val.WriteXml(writer);
            }

            output = fs.ToString();
        }

        var valuation = new XmlTrade();
        using (StringReader fs = new StringReader(output))
        {
            XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
            using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
            {
                valuation.ReadXml(reader);
            }
        }

        Assert.That(val, Is.EqualTo(valuation));
    }

    private static IEnumerable<TestCaseData> XmlRoundTripTestCases()
    {
        yield return new TestCaseData(
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-30T16:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");
        yield return new TestCaseData(
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Buy\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-31T00:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456\" />");
        yield return new TestCaseData(
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>{enl}<SecurityTrade TradeType=\"Dividend\" Company=\"Comp\" Name=\"Name\" Day=\"2018-01-31T00:00:00\" NumberShares=\"278.671\" UnitPrice=\"123\" TradeCosts=\"456.12345\" />");
    }

    [TestCaseSource(nameof(XmlRoundTripTestCases))]
    public void XmlRoundTripTests(string serializedData)
    {
        var valuation = new XmlTrade();
        using (StringReader fs = new StringReader(serializedData))
        {
            XmlReaderSettings xmlWriterSettings = new XmlReaderSettings();
            using (XmlReader reader = XmlReader.Create(fs, xmlWriterSettings))
            {
                valuation.ReadXml(reader);
            }
        }

        string output;
        using (StringWriter fs = new StringWriter())
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = false;
            xmlWriterSettings.Encoding = Encoding.UTF8;
            xmlWriterSettings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(fs, xmlWriterSettings))
            {
                valuation.WriteXml(writer);
            }

            output = fs.ToString();
            Assert.That(output, Is.EqualTo(serializedData));
        }
    }
}
