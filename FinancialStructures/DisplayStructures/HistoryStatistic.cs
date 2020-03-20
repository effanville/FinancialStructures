using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.Mathematics;
using FinancialStructures.PortfolioAPI;
using System;
using System.Collections.Generic;

namespace FinancialStructures.DisplayStructures
{
    public class HistoryStatistic
    {
        public string Headers()
        {
            string outputCSVStyle = string.Empty;
            outputCSVStyle += string.Concat("Date, TotalValue", ",");

            outputCSVStyle += string.Concat("BankTotal", ",");

            outputCSVStyle += string.Concat("SecurityTotal", ",");
            foreach (var value in SecurityValues)
            {
                outputCSVStyle += string.Concat(value.Company, ",");
            }
            foreach (var value in BankAccValues)
            {
                outputCSVStyle += string.Concat(value.Company, ",");
            }
            foreach (var value in SectorValues)
            {
                outputCSVStyle += string.Concat(value.Company, ",");
            }

            return outputCSVStyle;
        }
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
        public DailyValuation TotalValue { get; set; }

        public DailyValuation BankAccValue { get; set; }

        public DailyValuation SecurityValue { get; set; }

        public List<DayValue_Named> SecurityValues { get; set; } = new List<DayValue_Named>();

        public string SecurityValuesString
        {
            get
            {
                var stringRepresentation = string.Empty;
                foreach (var data in SecurityValues)
                {
                    stringRepresentation = stringRepresentation + "  " + data.ToString();
                }
                return stringRepresentation;
            }
        }
        public List<DayValue_Named> Security1YrCar { get; set; } = new List<DayValue_Named>();
        public List<DayValue_Named> SecurityTotalCar { get; set; } = new List<DayValue_Named>();

        public List<DayValue_Named> BankAccValues { get; set; } = new List<DayValue_Named>();

        public string BankAccValuesString
        {
            get
            {
                var stringRepresentation = string.Empty;
                foreach (var data in BankAccValues)
                {
                    stringRepresentation = stringRepresentation + "  " + data.ToString();
                }
                return stringRepresentation;
            }
        }

        public List<DayValue_Named> SectorValues { get; set; } = new List<DayValue_Named>();

        public string SectorValuesString
        {
            get
            {
                var stringRepresentation = string.Empty;
                foreach (var data in SectorValues)
                {
                    stringRepresentation = stringRepresentation + "  " + data.ToString();
                }
                return stringRepresentation;
            }
        }

        public List<DayValue_Named> SectorCar { get; set; } = new List<DayValue_Named>();

        public HistoryStatistic()
        { }

        public HistoryStatistic(DailyValuation value, List<DayValue_Named> securityValues, List<DayValue_Named> bankValues)
        {
            TotalValue = value;
            SecurityValues = securityValues;
            BankAccValues = bankValues;
        }

        public HistoryStatistic(Portfolio portfolio, DateTime date)
        {
            TotalValue = new DailyValuation(date, MathSupport.Truncate(portfolio.Value(date)));
            BankAccValue = new DailyValuation(date, MathSupport.Truncate(portfolio.TotalValue(AccountType.BankAccount, date)));
            SecurityValue = new DailyValuation(date, MathSupport.Truncate(portfolio.TotalValue(AccountType.Security, date)));
            var companyNames = portfolio.Companies(AccountType.Security);
            companyNames.Sort();
            foreach (var companyName in companyNames)
            {
                var companyValue = new DayValue_Named(null, companyName, date, MathSupport.Truncate(portfolio.CompanyValue(AccountType.Security, companyName, date)));
                SecurityValues.Add(companyValue);
                var yearCar = new DayValue_Named(null, companyName, date, MathSupport.Truncate(portfolio.IRRCompany(companyName, date.AddDays(-365), date)));
                Security1YrCar.Add(yearCar);
                DayValue_Named totalCar;
                if (date < portfolio.CompanyFirstDate(companyName))
                {
                    totalCar = new DayValue_Named(null, companyName, date, 0.0);
                }
                else
                {
                    totalCar = new DayValue_Named(null, companyName, date, MathSupport.Truncate(portfolio.IRRCompany(companyName, portfolio.CompanyFirstDate(companyName), date)));
                }
                SecurityTotalCar.Add(totalCar);
            }

            var companyBankNames = portfolio.Companies(AccountType.BankAccount);
            companyBankNames.Sort();
            foreach (var companyName in companyBankNames)
            {
                var companyValue = new DayValue_Named(null, companyName, date, MathSupport.Truncate(portfolio.CompanyValue(AccountType.BankAccount, companyName, date)));
                BankAccValues.Add(companyValue);
            }

            var sectorNames = portfolio.GetSecuritiesSectors();
            foreach (var sectorName in sectorNames)
            {
                var sectorValue = new DayValue_Named(null, sectorName, date, MathSupport.Truncate(portfolio.SectorValue(sectorName, date)));
                SectorValues.Add(sectorValue);
                DayValue_Named sectorCar;
                if (date < portfolio.SectorFirstDate(sectorName))
                {
                    sectorCar = new DayValue_Named(null, sectorName, date, 0.0);
                }
                else
                {
                    sectorCar = new DayValue_Named(null, sectorName, date, MathSupport.Truncate(portfolio.IRRSector(sectorName, portfolio.SectorFirstDate(sectorName), date)));
                }
                SectorCar.Add(sectorCar);
            }
        }
    }
}
