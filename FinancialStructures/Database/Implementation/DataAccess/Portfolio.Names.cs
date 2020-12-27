using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public List<string> Companies(Account elementType)
        {
            return NameData(elementType).Select(NameData => NameData.Company).Distinct().ToList();
        }

        /// <inheritdoc/>
        public List<string> Names(Account elementType)
        {
            return NameData(elementType).Select(NameData => NameData.Name).ToList();
        }

        /// <inheritdoc/>
        public List<NameCompDate> NameData(Account elementType)
        {
            List<NameCompDate> namesAndCompanies = new List<NameCompDate>();
            switch (elementType)
            {
                case (Account.Security):
                {
                    foreach (ISecurity security in Funds)
                    {
                        DateTime date = DateTime.MinValue;
                        if (security.Any())
                        {
                            date = security.LatestValue().Day;
                        }

                        namesAndCompanies.Add(new NameCompDate(security.Company, security.Name, security.Currency, security.Url, security.Sectors, date));
                    }
                    break;
                }
                case (Account.Currency):
                {
                    return SingleDataNameObtainer(Currencies);
                }
                case (Account.BankAccount):
                {
                    return SingleDataNameObtainer(BankAccounts);
                }
                case (Account.Benchmark):
                {
                    return SingleDataNameObtainer(BenchMarks);
                }
                default:
                    break;
            }

            return namesAndCompanies;
        }

        private List<NameCompDate> SingleDataNameObtainer<T>(List<T> objects) where T : ISingleValueDataList
        {
            List<NameCompDate> namesAndCompanies = new List<NameCompDate>();
            if (objects != null)
            {
                foreach (T dataList in objects)
                {
                    DateTime date = DateTime.MinValue;
                    if (dataList.Any())
                    {
                        date = dataList.LatestValue().Day;
                    }

                    namesAndCompanies.Add(new NameCompDate(dataList.Company, dataList.Name, dataList.Currency, dataList.Url, dataList.Names.Sectors, date));
                }
            }

            return namesAndCompanies;
        }
    }
}
