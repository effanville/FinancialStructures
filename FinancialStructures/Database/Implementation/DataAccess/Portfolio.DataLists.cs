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
        public List<ISecurity> CompanySecurities(string company)
        {
            List<ISecurity> securities = new List<ISecurity>();
            foreach (ISecurity sec in Funds)
            {
                if (sec.Names.Company == company)
                {
                    securities.Add(sec.Copy());
                }
            }
            securities.Sort();
            return securities;
        }

        /// <inheritdoc/>
        public List<ICashAccount> CompanyBankAccounts(string company)
        {
            List<ICashAccount> accounts = new List<ICashAccount>();
            foreach (ICashAccount acc in BankAccounts)
            {
                if (acc.Names.Company == company)
                {
                    accounts.Add(acc);
                }
            }

            accounts.Sort();
            return accounts;
        }

        public List<ISecurity> SectorSecurities(string sectorName)
        {
            List<ISecurity> securities = new List<ISecurity>();
            foreach (ISecurity security in Funds)
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
