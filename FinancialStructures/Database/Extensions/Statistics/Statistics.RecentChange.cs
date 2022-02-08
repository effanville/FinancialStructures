using System.Collections.Generic;

using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Totals totals = Totals.Security, TwoName names = null)
        {
            switch (totals)
            {
                case Totals.All:
                {
                    return portfolio.RecentChange(Totals.Security) + portfolio.RecentChange(Totals.BankAccount) + portfolio.RecentChange(Totals.Asset);
                }
                case Totals.Security:
                {
                    return RecentChangeOf(portfolio.FundsThreadSafe, portfolio, totals);
                }
                case Totals.SecurityCompany:
                case Totals.BankAccountCompany:
                case Totals.AssetCompany:
                {
                    return RecentChangeOf(portfolio.CompanyAccounts(totals.ToAccount(), names?.Company), portfolio, totals);
                }
                case Totals.BankAccount:
                {
                    return RecentChangeOf(portfolio.BankAccountsThreadSafe, portfolio, totals);
                }
                case Totals.Asset:
                {
                    return RecentChangeOf(portfolio.Assets, portfolio, totals);
                }
                case Totals.SecuritySector:
                case Totals.BankAccountSector:
                case Totals.AssetSector:
                {
                    return RecentChangeOf(portfolio.SectorAccounts(totals.ToAccount(), names), portfolio, totals);
                }
                case Totals.Sector:
                {
                    return portfolio.RecentChange(Totals.BankAccountSector, names) + portfolio.RecentChange(Totals.SecuritySector, names) + portfolio.RecentChange(Totals.AssetSector, names);
                }
                case Totals.Company:
                {
                    return portfolio.RecentChange(Totals.SecurityCompany, names) + portfolio.RecentChange(Totals.BankAccountCompany, names) + portfolio.RecentChange(Totals.AssetCompany, names);
                }
                case Totals.Benchmark:
                case Totals.Currency:
                case Totals.CurrencySector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                case Totals.AssetCurrency:
                default:
                {
                    return 0.0m;
                }
            }
        }

        private static decimal RecentChangeOf(IReadOnlyList<IValueList> accounts, IPortfolio portfolio, Totals totals)
        {
            decimal total = 0;
            foreach (IExchangableValueList valueList in accounts)
            {
                if (valueList.Any())
                {
                    ICurrency currency = portfolio.Currency(valueList);
                    total += valueList.RecentChange(currency);
                }
            }

            return total;
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Account elementType, TwoName names)
        {
            switch (elementType)
            {
                case Account.Security:
                case Account.BankAccount:
                case Account.Asset:
                {
                    if (portfolio.TryGetAccount(elementType, names, out IValueList account) && account.Any())
                    {
                        IExchangableValueList exchangeValueList = account as IExchangableValueList;
                        ICurrency currency = portfolio.Currency(exchangeValueList);
                        return exchangeValueList.RecentChange(currency);
                    }

                    break;
                }
                default:
                {
                    if (portfolio.TryGetAccount(elementType, names, out IValueList valueList) && valueList.Any())
                    {
                        return valueList.RecentChange();
                    }
                    break;
                }
            }

            return 0.0m;
        }
    }
}
