using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Effanville.FinancialStructures.Database.Implementation;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.FinanceStructures.Implementation.Asset;

namespace Effanville.FinancialStructures.Persistence.Xml
{
    [XmlType(TypeName = "Portfolio")]
    public class XmlPortfolio
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement(ElementName = "BaseCurrency")]
        public string BaseCurrency
        {
            get;
            set;
        }

        [XmlArray(ElementName = "Funds")]
        [XmlArrayItem(ElementName = "Security")]
        public List<XmlSecurity> Funds
        {
            get;
            private set;
        } = new List<XmlSecurity>();

        /// <summary>
        /// Backing for the BankAccounts.
        /// </summary>
        [XmlArray(ElementName = "BankAccounts")]
        [XmlArrayItem(ElementName = "CashAccount")]
        public List<XmlCashAccount> BankAccounts
        {
            get;
            set;
        } = new List<XmlCashAccount>();

        /// <summary>
        /// Backing for the currencies.
        /// </summary>
        [XmlArray(ElementName = "Currencies")]
        [XmlArrayItem(ElementName = "Currency")]
        public List<XmlCurrency> Currencies
        {
            get;
            private set;
        } = new List<XmlCurrency>();

        [XmlArray(ElementName = "BenchMarks")]
        [XmlArrayItem(ElementName = "Sector")]
        public List<XmlSector> BenchMarks
        {
            get;
            set;
        } = new List<XmlSector>();

        /// <summary>
        /// The list of assets in the portfolio.
        /// </summary>
        [XmlArray(ElementName = "Assets")]
        [XmlArrayItem(ElementName = "AmortisableAsset")]
        public List<XmlAmortisableAsset> Assets
        {
            get;
            set;
        } = new List<XmlAmortisableAsset>();

        /// <summary>
        /// A list storing the actual data for all Pensions
        /// </summary>
        [XmlArray(ElementName = "Pensions")]
        [XmlArrayItem(ElementName = "Pension")]
        public List<XmlSecurity> Pensions
        {
            get;
            private set;
        } = new List<XmlSecurity>();

        /// <summary>
        /// Internal list of all notes for the portfolio.
        /// </summary>
        [XmlArray(ElementName = "Notes")]
        public List<Note> NotesInternal
        {
            get;
            set;
        } = new List<Note>();

        public void SetFrom(Portfolio portfolio)
        {
            BaseCurrency = portfolio.BaseCurrency;
            Name = portfolio.Name;
            foreach (var security in portfolio.Funds)
            {
                Funds.Add(new XmlSecurity(security.Names, security.UnitPrice, security.Shares,
                    security.Investments, security.Trades.ToList()));
            }

            foreach (var bankAcc in portfolio.BankAccounts)
            {
                BankAccounts.Add(new XmlCashAccount(bankAcc.Names, bankAcc.Values));
            }

            foreach (var currency in portfolio.Currencies)
            {
                Currencies.Add(new XmlCurrency(currency.Names, currency.Values));
            }

            foreach (var sector in portfolio.BenchMarks)
            {
                BenchMarks.Add(new XmlSector(sector.Names, sector.Values));
            }

            foreach (var asset in portfolio.Assets)
            {
                Assets.Add(new XmlAmortisableAsset(asset.Names, asset.Values, asset.Debt, asset.Payments));
            }
            
            foreach (var pension in portfolio.Pensions)
            {
                Pensions.Add(new XmlSecurity(pension.Names, pension.UnitPrice, pension.Shares, pension.Investments, pension.Trades.ToList()));
            }
            
            NotesInternal = portfolio.NotesInternal;
        }

        public void Set(Portfolio portfolio)
        {
            portfolio.BaseCurrency = BaseCurrency;
            portfolio.Name = Name;
            portfolio.NotesInternal = NotesInternal;

            foreach (var bankAcc in BankAccounts)
            {
                portfolio.AddBankAccount(new CashAccount(bankAcc.Names, bankAcc.Values));
            }

            foreach (var security in Funds)
            {
                portfolio.AddFund(new Security(security.Names,
                    security.UnitPrice,
                    security.Shares,
                    security.Investments,
                     security.SecurityTrades));
            }

            foreach (var currency in Currencies)
            {
                portfolio.AddCurrency(new Currency(currency.Names, currency.Values));
            }

            foreach (var sector in BenchMarks)
            {
                portfolio.AddBenchMark(new Sector(sector.Names, sector.Values));
            }

            foreach (var asset in Assets)
            {
                portfolio.AddAsset(new AmortisableAsset(asset.Names, asset.Values, asset.Debt, asset.Payments));
            }

            foreach (var security in Pensions)
            {
                portfolio.AddPension(new Security(security.Names,
                     security.UnitPrice,
                    security.Shares,
                    security.Investments,
                    security.SecurityTrades));
            }
        }
    }
}