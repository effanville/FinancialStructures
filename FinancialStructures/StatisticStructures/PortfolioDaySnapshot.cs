using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using StructureCommon.MathLibrary;
using FinancialStructures.PortfolioAPI;
using System;
using System.Collections.Generic;

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

            foreach (var value in SecurityValues)
            {
                outputCSVStyle += string.Concat(value.Names.Company, ",");
            }
            foreach (var value in BankAccValues)
            {
                outputCSVStyle += string.Concat(value.Names.Company, ",");
            }
            foreach (var value in SectorValues)
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

            foreach (var value in SecurityValues)
            {
                outputCSVStyle += string.Concat(value.Value, ",");
            }
            foreach (var value in BankAccValues)
            {
                outputCSVStyle += string.Concat(value.Value, ",");
            }
            foreach (var value in SectorValues)
            {
                outputCSVStyle += string.Concat(value.Value, ",");
            }

            return outputCSVStyle;
        }

        public DailyValuation TotalValue { get; }

        public DailyValuation BankAccValue { get; }

        public DailyValuation SecurityValue { get; }

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
        { }

        /// <summary>
        /// Constructor which generates the snapshot.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="portfolio"></param>
        public PortfolioDaySnapshot(DateTime date, IPortfolio portfolio)
        {
            TotalValue = new DailyValuation(date, MathSupport.Truncate(portfolio.Value(date)));
            BankAccValue = new DailyValuation(date, MathSupport.Truncate(portfolio.TotalValue(AccountType.BankAccount, date)));
            SecurityValue = new DailyValuation(date, MathSupport.Truncate(portfolio.TotalValue(AccountType.Security, date)));

            AddSecurityValues(date, portfolio);
            AddBankAccountValues(date, portfolio);
            AddSectorTotalValues(date, portfolio);
        }

        private void AddSecurityValues(DateTime date, IPortfolio portfolio)
        {
            var companyNames = portfolio.Companies(AccountType.Security);
            companyNames.Sort();
            foreach (var companyName in companyNames)
            {
                var companyValue = new DayValue_Named(companyName, null, date, MathSupport.Truncate(portfolio.CompanyValue(AccountType.Security, companyName, date)));
                SecurityValues.Add(companyValue);

                var yearCar = new DayValue_Named(companyName, null, date, MathSupport.Truncate(portfolio.IRRCompany(companyName, date.AddDays(-365), date)));
                Security1YrCar.Add(yearCar);

                DayValue_Named totalCar;
                if (date < portfolio.CompanyFirstDate(companyName))
                {
                    totalCar = new DayValue_Named(companyName, null, date, 0.0);
                }
                else
                {
                    totalCar = new DayValue_Named(companyName, null, date, MathSupport.Truncate(portfolio.IRRCompany(companyName, portfolio.CompanyFirstDate(companyName), date)));
                }
                SecurityTotalCar.Add(totalCar);
            }
        }

        private void AddBankAccountValues(DateTime date, IPortfolio portfolio)
        {
            var companyBankNames = portfolio.Companies(AccountType.BankAccount);
            companyBankNames.Sort();
            foreach (var companyName in companyBankNames)
            {
                var companyValue = new DayValue_Named(companyName, null, date, MathSupport.Truncate(portfolio.CompanyValue(AccountType.BankAccount, companyName, date)));
                BankAccValues.Add(companyValue);
            }
        }

        private void AddSectorTotalValues(DateTime date, IPortfolio portfolio)
        {
            var sectorNames = portfolio.GetSecuritiesSectors();
            foreach (var sectorName in sectorNames)
            {
                var sectorValue = new DayValue_Named(sectorName, null, date, MathSupport.Truncate(portfolio.SectorValue(sectorName, date)));
                SectorValues.Add(sectorValue);

                DayValue_Named sectorCar;
                if (date < portfolio.SectorFirstDate(sectorName))
                {
                    sectorCar = new DayValue_Named(sectorName, null, date, 0.0);
                }
                else
                {
                    sectorCar = new DayValue_Named(sectorName, null, date, MathSupport.Truncate(portfolio.IRRSector(sectorName, portfolio.SectorFirstDate(sectorName), date)));
                }
                SectorCar.Add(sectorCar);
            }
        }
    }
}
