﻿using System;

using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Tests.TestDatabaseConstructor
{
    public class SecurityConstructor
    {
        public const string DefaultName = "UK Stock";
        public const string DefaultCompany = "BlackRock";
        public static readonly DateTime[] DefaultDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] DefaultShareValues = new decimal[] { 2.0m, 1.5m, 17.3m, 4, 5.7m, 5.5m };
        public static readonly decimal[] DefaultUnitPrices = new decimal[] { 100.0m, 100.0m, 125.2m, 90.6m, 77.7m, 101.1m };
        public static readonly decimal[] DefaultInvestments = new decimal[] { 100.0m, 0.0m, 0.0m, 0.0m, 0.0m, 0.0m };
        public static readonly SecurityTrade[] DefaultTrades = new SecurityTrade[]
            {
                new SecurityTrade(TradeType.Buy, new TwoName(DefaultCompany, DefaultName), new DateTime(2010, 1, 1), 2.0m, 100.0m, 0.0m),
                new SecurityTrade(TradeType.ShareReprice, new TwoName(DefaultCompany, DefaultName), new DateTime(2011, 1, 1), -0.5m, 100.0m, 0.0m),
                new SecurityTrade(TradeType.DividendReinvestment, new TwoName(DefaultCompany, DefaultName), new DateTime(2012, 5, 1), 15.8m, 125.2m, 0.0m)
            };

        public const string SecondaryName = "China Stock";
        public const string SecondaryCompany = "Prudential";
        public static readonly DateTime[] SecondaryDates = new DateTime[] { new DateTime(2010, 1, 5), new DateTime(2011, 2, 1), new DateTime(2012, 5, 5), new DateTime(2016, 4, 3), new DateTime(2019, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] SecondaryShareValues = new decimal[] { 2.0m, 2.5m, 17.3m, 22.5m, 22.7m, 25.5m };
        public static readonly decimal[] SecondaryUnitPrices = new decimal[] { 1010.0m, 1110.0m, 1215.2m, 900.6m, 1770.7m, 1001.1m };
        public static readonly decimal[] SecondaryInvestments = new decimal[] { 2020.0m, 0.0m, 21022.96m, 0.0m, 0.0m, 0.0m };
        
        
        public static readonly DateTime[] TertiaryDates = new DateTime[] { new DateTime(2010, 1, 5), new DateTime(2011, 2, 1), new DateTime(2012, 5, 5), new DateTime(2016, 4, 3), new DateTime(2019, 5, 6), new DateTime(2020, 1, 1),new DateTime(2020, 4, 1) };
        public static readonly decimal[] TertiaryUnitPrices = new decimal[] { 1010.0m, 1110.0m, 1215.2m, 900.6m, 1770.7m, 1001.1m, 1002.3m };
        public static readonly decimal[] TertiaryInvestments = new decimal[] { 2020.0m, 0.0m, 21022.96m, 0.0m, 0.0m,  -1000m };
        public static readonly decimal[] TertiaryShareValues = new decimal[] { 2.0m, 2.5m, 17.3m, 22.5m, 22.7m, 0m };

        private Security Item;

        private SecurityConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            NameData names = new NameData(company, name, currency, url)
            {
                SectorsFlat = sectors
            };
            Item = new Security(names);
        }

        public SecurityConstructor WithData(DateTime date, decimal sharePrice)
        {
            Item.UnitPrice.SetData(date, sharePrice);

            Item.EnsureDataConsistency();
            return this;
        }

        public SecurityConstructor WithTrades(SecurityTrade[] trades)
        {
            foreach (var trade in trades)
            {
                if (trade != null)
                {
                    Item.SecurityTrades.Add(trade);
                }
            }
            Item.EnsureDataConsistency();

            return this;
        }

        private SecurityConstructor WithData(DateTime date, decimal sharePrice, decimal numberUnits, decimal investment = 0)
        {
            Item.Shares.SetData(date, numberUnits);
            Item.UnitPrice.SetData(date, sharePrice);
            if (investment != 0)
            {
                Item.Investments.SetData(date, investment);
                Item.SecurityTrades.Add(new SecurityTrade(investment > 0 ? TradeType.Buy : TradeType.Sell, Item.Names.ToTwoName(), date, numberUnits, sharePrice, 0.0m));
            }

            Item.EnsureOnLoadDataConsistency();
            return this;
        }
        
        private SecurityConstructor WithDataFromTrades(DateTime date, decimal? sharePrice, decimal numberUnits, decimal investment = 0.0m)
        {
            decimal previousNumUnits = Item.Shares.LatestValue() ?? 0.0m;
            Item.Shares.SetData(date, numberUnits);
            if (sharePrice.HasValue)
            {
                Item.UnitPrice.SetData(date, sharePrice.Value);
            }

            if (investment != 0.0m)
            {
                Item.Investments.SetData(date, investment);
                decimal sharePriceValue = sharePrice.HasValue ? sharePrice.Value : Item.UnitPrice.LatestValue().Value;
                Item.SecurityTrades.Add(new SecurityTrade(investment > 0 ? TradeType.Buy : TradeType.Sell, Item.Names.ToTwoName(), date, numberUnits- previousNumUnits, sharePriceValue, 0.0m));
            }

            Item.EnsureOnLoadDataConsistency();
            return this;
        }

        public Security GetItem()
        {
            return Item;
        }

        public SecurityConstructor Clear()
        {
            Item = null;
            Item = new Security();
            return this;
        }

        public static Security NameLess()
        {
            return new SecurityConstructor(null, null).GetItem();
        }

        public static Security Empty()
        {
            return new SecurityConstructor(DefaultCompany, DefaultName).GetItem();
        }

        public static Security Default()
        {
            return WithNameAndData(DefaultCompany, DefaultName, dates: DefaultDates, sharePrice: DefaultUnitPrices, numberUnits: DefaultShareValues, investment: DefaultInvestments)
                .GetItem();
        }

        public static Security DefaultFromTrades()
        {
            return FromNameAndTradeData(
                DefaultCompany,
                DefaultName,
                dates: DefaultDates,
                sharePrice: DefaultUnitPrices,
                trades: DefaultTrades)
                .GetItem();
        }

        public static Security Secondary()
        {
            return WithNameAndData(
                SecondaryCompany,
                SecondaryName,
                currency: CurrencyConstructor.DefaultCompany,
                dates: SecondaryDates,
                sharePrice: SecondaryUnitPrices,
                numberUnits: SecondaryShareValues,
                investment: SecondaryInvestments)
                .GetItem();
        }
        
        public static Security NowWithNoShares()
            => WithNameAndDataFromTrades(
                    SecondaryCompany,
                    SecondaryName,
                    currency: CurrencyConstructor.DefaultCompany,
                    dates: TertiaryDates,
                    sharePrice: TertiaryUnitPrices,
                    numberUnits: TertiaryShareValues,
                    investment: TertiaryInvestments)
                .GetItem();

        public static SecurityConstructor WithName(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            return new SecurityConstructor(company, name, currency, url, sectors);
        }

        public static SecurityConstructor FromNameAndTradeData(
            string company,
            string name,
            string currency = null,
            string url = null,
            string sectors = null,
            DateTime[] dates = null,
            decimal[] sharePrice = null,
            SecurityTrade[] trades = null)
        {
            SecurityConstructor securityConstructor = new SecurityConstructor(company, name, currency, url, sectors);
            if (dates != null)
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    if (dates[i] != default(DateTime))
                    {
                        _ = securityConstructor.WithData(dates[i], sharePrice[i]);
                    }
                }
            }

            _ = securityConstructor.WithTrades(trades);
            return securityConstructor;
        }
        
        public static SecurityConstructor WithNameAndDataFromTrades(
            string company,
            string name,
            string currency = null,
            string url = null,
            string sectors = null,
            DateTime[] dates = null,
            decimal[] sharePrice = null,
            decimal[] numberUnits = null,
            decimal[] investment = null)
        {
            SecurityConstructor securityConstructor = new SecurityConstructor(company, name, currency, url, sectors);
            if (dates != null)
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    if (dates[i] != default(DateTime))
                    {
                        decimal numUnits = i < numberUnits?.Length ? numberUnits[i] : 0.0m;
                        decimal numInvestment = i < investment?.Length ? investment[i] : 0.0m;
                        _ = securityConstructor.WithDataFromTrades(dates[i], sharePrice?[i] , numUnits, numInvestment);
                    }
                }
            }
            return securityConstructor;
        }
        public static SecurityConstructor WithNameAndData(
            string company,
            string name,
            string currency = null,
            string url = null,
            string sectors = null,
            DateTime[] dates = null,
            decimal[] sharePrice = null,
            decimal[] numberUnits = null,
            decimal[] investment = null)
        {
            SecurityConstructor securityConstructor = new SecurityConstructor(company, name, currency, url, sectors);
            if (dates != null)
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    if (dates[i] != default(DateTime))
                    {
                        _ = securityConstructor.WithData(dates[i], sharePrice[i] , numberUnits?[i] ?? 0.0m, investment?[i] ?? 0.0m);
                    }
                }
            }
            return securityConstructor;
        }
    }
}
