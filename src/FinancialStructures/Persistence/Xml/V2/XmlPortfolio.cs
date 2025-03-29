using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation.Asset;

namespace Effanville.FinancialStructures.Persistence.Xml.V2
{
    [XmlType(TypeName = "Portfolio")]
    public class XmlPortfolio
    {
        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "BaseCurrency")]
        public string BaseCurrency { get; set; }

        [XmlArray(ElementName = "Funds")]
        [XmlArrayItem(ElementName = "Security")]
        public List<XmlSecurity> Funds { get; private set; } = new List<XmlSecurity>();

        /// <summary>
        /// Backing for the BankAccounts.
        /// </summary>
        [XmlArray(ElementName = "BankAccounts")]
        [XmlArrayItem(ElementName = "CashAccount")]
        public List<XmlCashAccount> BankAccounts { get; set; } = new List<XmlCashAccount>();

        /// <summary>
        /// Backing for the currencies.
        /// </summary>
        [XmlArray(ElementName = "Currencies")]
        [XmlArrayItem(ElementName = "Currency")]
        public List<XmlCurrency> Currencies { get; private set; } = new List<XmlCurrency>();

        [XmlArray(ElementName = "BenchMarks")]
        [XmlArrayItem(ElementName = "Sector")]
        public List<XmlSector> BenchMarks { get; set; } = new List<XmlSector>();

        /// <summary>
        /// The list of assets in the portfolio.
        /// </summary>
        [XmlArray(ElementName = "Assets")]
        [XmlArrayItem(ElementName = "AmortisableAsset")]
        public List<XmlAmortisableAsset> Assets { get; set; } = new List<XmlAmortisableAsset>();

        /// <summary>
        /// A list storing the actual data for all Pensions
        /// </summary>
        [XmlArray(ElementName = "Pensions")]
        [XmlArrayItem(ElementName = "Pension")]
        public List<XmlSecurity> Pensions { get; private set; } = new List<XmlSecurity>();

        /// <summary>
        /// Internal list of all notes for the portfolio.
        /// </summary>
        [XmlArray(ElementName = "Notes")]
        public List<Note> NotesInternal { get; set; } = new List<Note>();

        public XmlPortfolio()
        {
            Version = "2.0.0.0";
        }

        public XmlPortfolio(Portfolio portfolio)
        {
            Version = "2.0.0.0";
            BaseCurrency = portfolio.BaseCurrency;
            Name = portfolio.Name;
            foreach (ISecurity security in portfolio.Funds)
            {
                Funds.Add(new XmlSecurity(security.Names, new XmlTimeList(security.UnitPrice), security.Trades.Select(x => new XmlTrade(x)).ToList()));
            }

            foreach (IExchangeableValueList bankAcc in portfolio.BankAccounts)
            {
                BankAccounts.Add(new XmlCashAccount(bankAcc.Names, new XmlTimeList(bankAcc.Values)));
            }

            foreach (ICurrency currency in portfolio.Currencies)
            {
                Currencies.Add(new XmlCurrency(currency.Names, new XmlTimeList(currency.Values)));
            }

            foreach (IValueList sector in portfolio.BenchMarks)
            {
                BenchMarks.Add(new XmlSector(sector.Names, new XmlTimeList(sector.Values)));
            }

            foreach (IAmortisableAsset asset in portfolio.Assets)
            {
                Assets.Add(new XmlAmortisableAsset(asset.Names, new XmlTimeList(asset.Values), new XmlTimeList(asset.Debt), new XmlTimeList(asset.Payments)));
            }

            foreach (ISecurity pension in portfolio.Pensions)
            {
                Pensions.Add(new XmlSecurity(pension.Names, new XmlTimeList(pension.UnitPrice), pension.Trades.Select(x => new XmlTrade(x)).ToList()));
            }

            NotesInternal = portfolio.NotesInternal;
        }

        public void Set(Portfolio portfolio)
        {
            portfolio.BaseCurrency = BaseCurrency;
            portfolio.Name = Name;
            portfolio.NotesInternal = NotesInternal;

            foreach (XmlCashAccount bankAcc in BankAccounts)
            {
                portfolio.AddBankAccount(new CashAccount(bankAcc.Names, bankAcc.Values.ToTimeList()));
            }

            foreach (XmlSecurity security in Funds)
            {
                portfolio.AddFund(new Security(
                    Account.Security,
                    security.Names,
                    security.UnitPrice.ToTimeList(),
                     security.SecurityTrades.Select(x => x.ToTrade()).ToList()));
            }

            foreach (XmlCurrency currency in Currencies)
            {
                portfolio.AddCurrency(new Currency(currency.Names, currency.Values.ToTimeList()));
            }

            foreach (XmlSector sector in BenchMarks)
            {
                portfolio.AddBenchMark(new Sector(sector.Names, sector.Values.ToTimeList()));
            }

            foreach (XmlAmortisableAsset asset in Assets)
            {
                portfolio.AddAsset(new AmortisableAsset(asset.Names, asset.Values.ToTimeList(), asset.Debt.ToTimeList(), asset.Payments.ToTimeList()));
            }

            foreach (XmlSecurity security in Pensions)
            {
                portfolio.AddPension(new Security(
                    Account.Pension,
                    security.Names,
                     security.UnitPrice.ToTimeList(),
                    security.SecurityTrades.Select(x => x.ToTrade()).ToList()));
            }
        }
    }
}