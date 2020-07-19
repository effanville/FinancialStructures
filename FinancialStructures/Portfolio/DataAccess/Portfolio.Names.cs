﻿using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public List<string> Companies(AccountType elementType)
        {
            return NameData(elementType).Select(NameData => NameData.Company).Distinct().ToList();
        }

        /// <inheritdoc/>
        public List<string> Names(AccountType elementType)
        {
            return NameData(elementType).Select(NameData => NameData.Name).ToList();
        }

        /// <inheritdoc/>
        public List<NameCompDate> NameData(AccountType elementType)
        {
            List<NameCompDate> namesAndCompanies = new List<NameCompDate>();
            switch (elementType)
            {
                case (AccountType.Security):
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
                case (AccountType.Currency):
                {
                    return SingleDataNameObtainer(Currencies);
                }
                case (AccountType.BankAccount):
                {
                    return SingleDataNameObtainer(BankAccounts);
                }
                case (AccountType.Sector):
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
