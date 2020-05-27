using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using StructureCommon.DataStructures;
using StructureCommon.Extensions;

namespace FinancialStructures.StatisticStructures
{
    /// <summary>
    /// Stores all values and some IRRs of a portfolio on a given day
    /// </summary>
    public class PortfolioDaySnapshot
    {
        /// <summary>
        /// Outputs the headers for the snapshot for a given collection of values in CSV style.
        /// </summary>
        public string Headers()
        {
            string outputCSVStyle = string.Empty;
            outputCSVStyle += string.Concat("Date, TotalValue", ",");

            outputCSVStyle += string.Concat("BankTotal", ",");

            outputCSVStyle += string.Concat("SecurityTotal", ",");

            foreach (DayValue_Named value in SecurityValues)
            {
                outputCSVStyle += string.Concat(value.Names.Company, ",");
            }
            foreach (DayValue_Named value in BankAccValues)
            {
                outputCSVStyle += string.Concat(value.Names.Company, ",");
            }
            foreach (DayValue_Named value in SectorValues)
            {
                outputCSVStyle += string.Concat(value.Names.Company, ",");
            }

            return outputCSVStyle;
        }

        /// <summary>
        /// Outputs the values stored in CSV style.
        /// </summary>
        public override string ToString()
        {
            string outputCSVStyle = string.Empty;
            outputCSVStyle += string.Concat(TotalValue.ToString(), ",");

            outputCSVStyle += string.Concat(BankAccValue.Value, ",");

            outputCSVStyle += string.Concat(SecurityValue.Value, ",");

            foreach (DayValue_Named value in SecurityValues)
            {
                outputCSVStyle += string.Concat(value.Value, ",");
            }
            foreach (DayValue_Named value in BankAccValues)
            {
                outputCSVStyle += string.Concat(value.Value, ",");
            }
            foreach (DayValue_Named value in SectorValues)
            {
                outputCSVStyle += string.Concat(value.Value, ",");
            }

            return outputCSVStyle;
        }

        public DailyValuation TotalValue
        {
            get;
        }

        public DailyValuation BankAccValue
        {
            get;
        }

        public DailyValuation SecurityValue
        {
            get;
        }

        public List<DayValue_Named> SecurityValues { get; } = new List<DayValue_Named>();

        public List<DayValue_Named> Security1YrCar { get; } = new List<DayValue_Named>();
        public List<DayValue_Named> SecurityTotalCar { get; } = new List<DayValue_Named>();

        public List<DayValue_Named> BankAccValues { get; } = new List<DayValue_Named>();

        public List<DayValue_Named> SectorValues { get; } = new List<DayValue_Named>();

        public List<DayValue_Named> SectorCar { get; } = new List<DayValue_Named>();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PortfolioDaySnapshot()
        {
        }

        /// <summary>
        /// Constructor which generates the snapshot.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="portfolio"></param>
        public PortfolioDaySnapshot(DateTime date, IPortfolio portfolio)
        {
            TotalValue = new DailyValuation(date, portfolio.Value(date).Truncate());
            BankAccValue = new DailyValuation(date, portfolio.TotalValue(AccountType.BankAccount, date).Truncate());
            SecurityValue = new DailyValuation(date, portfolio.TotalValue(AccountType.Security, date).Truncate());

            AddSecurityValues(date, portfolio);
            AddBankAccountValues(date, portfolio);
            AddSectorTotalValues(date, portfolio);
        }

        private void AddSecurityValues(DateTime date, IPortfolio portfolio)
        {
            List<string> companyNames = portfolio.Companies(AccountType.Security);
            companyNames.Sort();
            foreach (string companyName in companyNames)
            {
                DayValue_Named companyValue = new DayValue_Named(companyName, null, date, portfolio.CompanyValue(AccountType.Security, companyName, date).Truncate());
                SecurityValues.Add(companyValue);

                DayValue_Named yearCar = new DayValue_Named(companyName, null, date, portfolio.IRRCompany(companyName, date.AddDays(-365), date).Truncate());
                Security1YrCar.Add(yearCar);

                DayValue_Named totalCar;
                if (date < portfolio.CompanyFirstDate(companyName))
                {
                    totalCar = new DayValue_Named(companyName, null, date, 0.0);
                }
                else
                {
                    totalCar = new DayValue_Named(companyName, null, date, portfolio.IRRCompany(companyName, portfolio.CompanyFirstDate(companyName), date).Truncate());
                }
                SecurityTotalCar.Add(totalCar);
            }
        }

        private void AddBankAccountValues(DateTime date, IPortfolio portfolio)
        {
            List<string> companyBankNames = portfolio.Companies(AccountType.BankAccount);
            companyBankNames.Sort();
            foreach (string companyName in companyBankNames)
            {
                DayValue_Named companyValue = new DayValue_Named(companyName, null, date, portfolio.CompanyValue(AccountType.BankAccount, companyName, date).Truncate());
                BankAccValues.Add(companyValue);
            }
        }

        private void AddSectorTotalValues(DateTime date, IPortfolio portfolio)
        {
            List<string> sectorNames = portfolio.GetSecuritiesSectors();
            foreach (string sectorName in sectorNames)
            {
                DayValue_Named sectorValue = new DayValue_Named(sectorName, null, date, portfolio.SectorValue(sectorName, date).Truncate());
                SectorValues.Add(sectorValue);

                DayValue_Named sectorCar;
                if (date < portfolio.SectorFirstDate(sectorName))
                {
                    sectorCar = new DayValue_Named(sectorName, null, date, 0.0);
                }
                else
                {
                    sectorCar = new DayValue_Named(sectorName, null, date, portfolio.IRRSector(sectorName, portfolio.SectorFirstDate(sectorName), date).Truncate());
                }
                SectorCar.Add(sectorCar);
            }
        }
    }
}
