﻿using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.Database.Extensions.Rates;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Export.History
{
    /// <summary>
    /// Stores all values and some IRRs of a portfolio on a given day
    /// </summary>
    public sealed class PortfolioDaySnapshot : IComparable, IComparable<PortfolioDaySnapshot>
    {
        /// <summary>
        /// The Date for the snapshot.
        /// </summary>
        public DateTime Date
        {
            get;
        }

        /// <summary>
        /// The total value held in the portfolio.
        /// </summary>
        public decimal TotalValue
        {
            get;
        }

        /// <summary>
        /// The total held in BankAccounts.
        /// </summary>
        public decimal BankAccValue
        {
            get;
        }

        /// <summary>
        /// The total held in securities.
        /// </summary>
        public decimal SecurityValue
        {
            get;
        }

        /// <summary>
        /// The Total IRR of all securities at this snapshot.
        /// </summary>
        public double TotalSecurityIRR
        {
            get;
            private set;
        }

        /// <summary>
        /// The totals held in each of the securities.
        /// </summary>
        public IDictionary<string, decimal> SecurityValues
        {
            get;
        } = new Dictionary<string, decimal>();

        /// <summary>
        /// The IRR values for all security companies over the last year.
        /// </summary>
        public IDictionary<string, double> Security1YrCar
        {
            get;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The IRR values for all security companies over all time.
        /// </summary>
        public IDictionary<string, double> SecurityTotalCar
        {
            get;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The total values for all bank account companies.
        /// </summary>
        public Dictionary<string, decimal> BankAccValues
        {
            get;
        } = new Dictionary<string, decimal>();

        /// <summary>
        /// Total values for each company.
        /// </summary>
        public IDictionary<string, decimal> CompanyValues
        {
            get;
        } = new Dictionary<string, decimal>();

        /// <summary>
        /// Total IRR for each company.
        /// </summary>
        public IDictionary<string, double> CompanyTotalIRR
        {
            get;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The total held in securities.
        /// </summary>
        public decimal AssetValue
        {
            get;
        }

        /// <summary>
        /// The total held in pensions.
        /// </summary>
        public decimal PensionValue
        {
            get;
        }

        /// <summary>
        /// The Total IRR of all pensions at this snapshot.
        /// </summary>
        public double TotalPensionIRR
        {
            get;
            private set;
        }

        /// <summary>
        /// The totals held in each of the pensions.
        /// </summary>
        public IDictionary<string, decimal> PensionValues
        {
            get;
        } = new Dictionary<string, decimal>();

        /// <summary>
        /// The IRR values for all pension companies over the last year.
        /// </summary>
        public IDictionary<string, double> Pension1YrCar
        {
            get;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The IRR values for all pension companies over all time.
        /// </summary>
        public IDictionary<string, double> PensionTotalCar
        {
            get;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The total values held in each sector.
        /// </summary>
        public Dictionary<string, decimal> SectorValues
        {
            get;
        } = new Dictionary<string, decimal>();

        /// <summary>
        /// The total CAR for each sector.
        /// </summary>
        public Dictionary<string, double> CurrentSectorTotalCar
        {
            get;
        } = new Dictionary<string, double>();

        /// <summary>
        /// Header values for this object.
        /// </summary>
        /// <returns></returns>
        public List<string> ExportHeaders()
        {
            List<string> headers = new List<string>
            {
                "Date",
                "TotalValue",
                "BankTotal",
                "SecurityTotal",
                "AssetTotal",
                "PensionTotal"
            };

            foreach (KeyValuePair<string, decimal> value in SecurityValues)
            {
                headers.Add(value.Key);
            }

            foreach (KeyValuePair<string, decimal> value in PensionValues)
            {
                headers.Add(value.Key);
            }

            foreach (KeyValuePair<string, decimal> value in BankAccValues)
            {
                headers.Add(value.Key);
            }

            foreach (KeyValuePair<string, decimal> value in SectorValues)
            {
                headers.Add(value.Key);
            }

            return headers;
        }

        /// <summary>
        /// Values to export for this.
        /// </summary>
        /// <returns></returns>
        public List<string> ExportValues(string dateFormat = "dd/MM/yyyy", bool includeSecurityValues = true, bool includePensionValues = true, bool includeBankAccValues = true, bool includeSectorValues = true)
        {
            List<string> values = new List<string>
            {
                Date.ToString(dateFormat),
                TotalValue.TruncateToString(),
                BankAccValue.TruncateToString(),
                SecurityValue.TruncateToString(),
                AssetValue.TruncateToString(),
                PensionValue.TruncateToString()
            };

            if (includeSecurityValues)
            {
                foreach (KeyValuePair<string, decimal> value in SecurityValues)
                {
                    values.Add(value.Value.TruncateToString());
                }
            }

            if (includePensionValues)
            {
                foreach (KeyValuePair<string, decimal> value in PensionValues)
                {
                    values.Add(value.Value.TruncateToString());
                }
            }

            if (includeBankAccValues)
            {
                foreach (KeyValuePair<string, decimal> value in BankAccValues)
                {
                    values.Add(value.Value.TruncateToString());
                }
            }

            if (includeSectorValues)
            {
                foreach (KeyValuePair<string, decimal> value in SectorValues)
                {
                    values.Add(value.Value.TruncateToString());
                }
            }

            return values;
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PortfolioDaySnapshot()
        {
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public PortfolioDaySnapshot(DateTime date, IPortfolio portfolio, PortfolioHistory.Settings settings)
        {
            Date = date;
            TotalValue = portfolio.TotalValue(Totals.All, date);
            BankAccValue = portfolio.TotalValue(Totals.BankAccount, date);
            SecurityValue = portfolio.TotalValue(Totals.Security, date);
            AssetValue = portfolio.TotalValue(Totals.Asset, date);
            PensionValue = portfolio.TotalValue(Totals.Pension, date);

            var companies = portfolio.Companies(Account.All);
            foreach (string company in companies)
            {
                decimal value = portfolio.TotalValue(Totals.Company, date, new TwoName(company));
                DateTime firstCompanyDate = portfolio.FirstValueDate(Totals.Company, new TwoName(company));
                double totalIRR = date < firstCompanyDate ? 0.0 : portfolio.TotalIRR(Totals.Company, firstCompanyDate, date, new TwoName(company), settings.MaxIRRIterations);
                CompanyTotalIRR.Add(company, totalIRR);
                CompanyValues.Add(company, value);
            }

            var securitySettings = settings[Account.Security];
            AddSecurityValues(date, portfolio, securitySettings.GenerateValues, securitySettings.GenerateRates, settings.MaxIRRIterations);

            var pensionSettings = settings[Account.Pension];
            AddPensionValues(date, portfolio, pensionSettings.GenerateValues, pensionSettings.GenerateRates, settings.MaxIRRIterations);

            var bankAccountSettings = settings[Account.BankAccount];
            AddBankAccountValues(date, portfolio, bankAccountSettings.GenerateValues);

            var sectorSettings = settings[Account.Benchmark];
            AddSectorTotalValues(date, portfolio, sectorSettings.GenerateValues, sectorSettings.GenerateRates, settings.MaxIRRIterations);

        }

        private void AddSecurityValues(DateTime date, IPortfolio portfolio, bool includeValues, bool generateRates, int numIterations)
        {
            if (!includeValues && !generateRates)
            {
                return;
            }

            List<string> companyNames = portfolio.Companies(Account.Security).ToList();
            companyNames.Sort();

            DateTime firstDate = portfolio.FirstValueDate(Totals.Security);
            TotalSecurityIRR = firstDate > date ? 0 : portfolio.TotalIRR(Totals.Security, firstDate, date, numIterations: numIterations);

            foreach (string companyName in companyNames)
            {
                if (includeValues)
                {
                    SecurityValues.Add(companyName, portfolio.TotalValue(Totals.SecurityCompany, date, new TwoName(companyName)));
                }

                if (generateRates)
                {
                    Security1YrCar.Add(companyName, portfolio.TotalIRR(Totals.SecurityCompany, date.AddDays(-365), date, new TwoName(companyName)));

                    DateTime firstCompanyDate = portfolio.FirstValueDate(Totals.SecurityCompany, new TwoName(companyName));
                    double totalIRR = date < firstCompanyDate ? 0.0 : portfolio.TotalIRR(Totals.SecurityCompany, firstCompanyDate, date, new TwoName(companyName), numIterations);
                    SecurityTotalCar.Add(companyName, totalIRR);
                }
            }
        }

        private void AddPensionValues(DateTime date, IPortfolio portfolio, bool includeValues, bool generateRates, int numIterations)
        {
            if (!includeValues && !generateRates)
            {
                return;
            }

            List<string> companyNames = portfolio.Companies(Account.Pension).ToList();
            companyNames.Sort();

            DateTime firstDate = portfolio.FirstValueDate(Totals.Pension);
            TotalPensionIRR = firstDate > date ? 0 : portfolio.TotalIRR(Totals.Pension, firstDate, date, numIterations: numIterations);

            foreach (string companyName in companyNames)
            {
                if (includeValues)
                {
                    PensionValues.Add(companyName, portfolio.TotalValue(Totals.PensionCompany, date, new TwoName(companyName)));
                }

                if (generateRates)
                {
                    Pension1YrCar.Add(companyName, portfolio.TotalIRR(Totals.PensionCompany, date.AddDays(-365), date, new TwoName(companyName)));

                    DateTime firstCompanyDate = portfolio.FirstValueDate(Totals.PensionCompany, new TwoName(companyName));
                    double totalIRR = date < firstCompanyDate ? 0.0 : portfolio.TotalIRR(Totals.PensionCompany, firstCompanyDate, date, new TwoName(companyName), numIterations);
                    PensionTotalCar.Add(companyName, totalIRR);
                }
            }
        }

        private void AddBankAccountValues(DateTime date, IPortfolio portfolio, bool includeValues)
        {
            if (!includeValues)
            {
                return;
            }

            List<string> companyBankNames = portfolio.Companies(Account.BankAccount).ToList();
            companyBankNames.Sort();
            foreach (string companyName in companyBankNames)
            {
                BankAccValues.Add(companyName, portfolio.TotalValue(Totals.BankAccountCompany, date, new TwoName(companyName)));
            }
        }

        private void AddSectorTotalValues(DateTime date, IPortfolio portfolio, bool includeValues, bool generateRates, int numIterations)
        {
            if (!includeValues && !generateRates)
            {
                return;
            }

            IReadOnlyList<string> sectorNames = portfolio.Sectors(Account.Security);
            foreach (string sectorName in sectorNames)
            {
                if (includeValues)
                {
                    SectorValues.Add(sectorName, portfolio.TotalValue(Totals.Sector, date, new TwoName(null, sectorName)));
                }

                if (generateRates)
                {
                    DateTime firstDate = portfolio.FirstValueDate(Totals.Sector, new TwoName(null, sectorName));
                    double sectorCAR = date < firstDate ? 0.0 : portfolio.TotalIRR(Totals.Sector, firstDate, date, new TwoName(null, sectorName), numIterations);
                    CurrentSectorTotalCar.Add(sectorName, sectorCAR);
                }
            }
        }

        /// <inheritdoc/>
        public int CompareTo(PortfolioDaySnapshot other)
        {
            return DateTime.Compare(Date, other.Date);
        }

        /// <summary>
        /// Method of comparison. Compares dates.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is PortfolioDaySnapshot val)
            {
                return CompareTo(val);
            }

            return 0;
        }

        /// <summary>
        /// Return strings with values and IRR for a company.
        /// </summary>
        public (string Date, decimal Value, double TotalIRR) GetCompanyStrings(string company)
        {
            return ($"{Date.Year}-{Date.Month}-{Date.Day}", CompanyValues[company], 100 * CompanyTotalIRR[company]);
        }
    }
}