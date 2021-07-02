using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <summary>
        /// Returns a copy of the currently held portfolio.
        /// Note one cannot use this portfolio to edit as it makes a copy.
        /// </summary>
        /// <remarks>
        /// This is in theory dangerous. I know thought that a security copied
        /// returns a genuine security, so I can case without trouble.
        /// </remarks>
        public IPortfolio Copy()
        {
            Portfolio PortfoCopy = new Portfolio();
            PortfoCopy.BaseCurrency = BaseCurrency;
            PortfoCopy.FilePath = FilePath;
            foreach (Security security in Funds)
            {
                PortfoCopy.Funds.Add((Security)security.Copy());
            }
            foreach (CashAccount bankAcc in BankAccounts)
            {
                PortfoCopy.BankAccounts.Add((CashAccount)bankAcc.Copy());
            }
            foreach (Currency currency in Currencies)
            {
                PortfoCopy.Currencies.Add((Currency)currency.Copy());
            }
            foreach (Sector sector in BenchMarks)
            {
                PortfoCopy.BenchMarks.Add((Sector)sector.Copy());
            }

            return PortfoCopy;
        }
        /// <inheritdoc/>
        public IReadOnlyList<IValueList> CompanyAccounts(string company)
        {
            return CompanyAccounts(Account.All, company);
        }

        /// <inheritdoc/>
        public IReadOnlyList<IValueList> CompanyAccounts(Account account, string company)
        {
            List<IValueList> accountList = new List<IValueList>();
            switch (account)
            {
                case Account.All:
                {
                    accountList.AddRange(CompanyAccounts(Account.Security, company));
                    accountList.AddRange(CompanyAccounts(Account.BankAccount, company));
                    accountList.Sort();
                    break;
                }
                case Account.Security:
                {
                    foreach (ISecurity sec in FundsThreadSafe)
                    {
                        if (sec.Names.Company == company)
                        {
                            accountList.Add(sec.Copy());
                        }
                    }
                    accountList.Sort();
                    break;
                }
                case Account.BankAccount:
                {
                    foreach (ICashAccount acc in BankAccounts)
                    {
                        if (acc.Names.Company == company)
                        {
                            accountList.Add(acc);
                        }
                    }

                    accountList.Sort();
                    break;
                }
                case Account.Benchmark:
                case Account.Currency:
                default:
                    break;
            }

            return accountList;
        }

        /// <inheritdoc/>
        public IReadOnlyList<ISecurity> SectorSecurities(string sectorName)
        {
            List<ISecurity> securities = new List<ISecurity>();
            foreach (ISecurity security in FundsThreadSafe)
            {
                if (security.IsSectorLinked(sectorName))
                {
                    securities.Add(security);
                }
            }
            securities.Sort();
            return securities;
        }
    }
}
