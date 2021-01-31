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
        public List<NameData> NameData(Account elementType)
        {
            List<NameData> namesAndCompanies = new List<NameData>();
            switch (elementType)
            {
                case (Account.Security):
                {
                    return SingleDataNameObtainer(Funds);
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

        private List<NameData> SingleDataNameObtainer<T>(List<T> objects) where T : IValueList
        {
            List<NameData> namesAndCompanies = new List<NameData>();
            if (objects != null)
            {
                foreach (T dataList in objects)
                {
                    namesAndCompanies.Add(new NameData(dataList.Names.Company, dataList.Names.Name, dataList.Names.Currency, dataList.Names.Url, dataList.Names.Sectors));
                }
            }

            return namesAndCompanies;
        }
    }
}
