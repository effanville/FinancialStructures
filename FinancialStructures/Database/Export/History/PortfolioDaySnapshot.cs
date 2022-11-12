using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.Extensions;
using FinancialStructures.Database.Extensions.Rates;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Export.History
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
            set;
        } = new Dictionary<string, decimal>();

        /// <summary>
        /// The IRR values for all security companies over the last year.
        /// </summary>
        public IDictionary<string, double> Security1YrCar
        {
            get;
            set;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The IRR values for all security companies over all time.
        /// </summary>
        public IDictionary<string, double> SecurityTotalCar
        {
            get;
            set;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The total values for all bank account companies.
        /// </summary>
        public Dictionary<string, decimal> BankAccValues
        {
            get;
        } = new Dictionary<string, decimal>();

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
            set;
        } = new Dictionary<string, decimal>();

        /// <summary>
        /// The IRR values for all pension companies over the last year.
        /// </summary>
        public IDictionary<string, double> Pension1YrCar
        {
            get;
            set;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The IRR values for all pension companies over all time.
        /// </summary>
        public IDictionary<string, double> PensionTotalCar
        {
            get;
            set;
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
        public List<string> ExportValues()
        {
            List<string> values = new List<string>
            {
                Date.ToUkDateString(),
                TotalValue.TruncateToString(),
                BankAccValue.TruncateToString(),
                SecurityValue.TruncateToString(),
                AssetValue.TruncateToString(),
                PensionValue.TruncateToString()
            };

            foreach (KeyValuePair<string, decimal> value in SecurityValues)
            {
                values.Add(value.Value.TruncateToString());
            }

            foreach (KeyValuePair<string, decimal> value in PensionValues)
            {
                values.Add(value.Value.TruncateToString());
            }

            foreach (KeyValuePair<string, decimal> value in BankAccValues)
            {
                values.Add(value.Value.TruncateToString());
            }

            foreach (KeyValuePair<string, decimal> value in SectorValues)
            {
                values.Add(value.Value.TruncateToString());
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
        /// Constructor which generates the snapshot.
        /// </summary>
        /// <param name="date">The date to take a snapshot on.</param>
        /// <param name="portfolio">The portfolio to take a snapshot of.</param>
        /// <param name="includeSecurityValues">Should security values be calculated.</param>
        /// <param name="includeBankValues">Should bank account values be calculated.</param>
        /// <param name="includeSectorValues">Should sector values be calculated.</param>
        /// <param name="generateSecurityRates">Should security rates be calculated.</param>
        /// <param name="generateSectorRates">Should sector rates be calculated.</param>
        /// <param name="maxRateIterations">The number of iterations to use for calculating rate values.</param>
        public PortfolioDaySnapshot(DateTime date, IPortfolio portfolio, bool includeSecurityValues, bool includeBankValues, bool includeSectorValues, bool generateSecurityRates, bool generateSectorRates, int maxRateIterations = 10)
        {
            Date = date;
            TotalValue = portfolio.TotalValue(Totals.All, date);
            BankAccValue = portfolio.TotalValue(Totals.BankAccount, date);
            SecurityValue = portfolio.TotalValue(Totals.Security, date);
            AssetValue = portfolio.TotalValue(Totals.Asset, date);
            PensionValue = portfolio.TotalValue(Totals.Pension, date);

            AddSecurityValues(date, portfolio, includeSecurityValues, generateSecurityRates, maxRateIterations);
            AddPensionValues(date, portfolio, includeSecurityValues, generateSecurityRates, maxRateIterations);
            AddBankAccountValues(date, portfolio, includeBankValues);
            AddSectorTotalValues(date, portfolio, includeSectorValues, generateSectorRates, maxRateIterations);
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

    }
}