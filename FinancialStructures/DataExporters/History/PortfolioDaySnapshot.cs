using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using Common.Structure.Extensions;
using System.Linq;

namespace FinancialStructures.DataExporters.History
{
    /// <summary>
    /// Stores all values and some IRRs of a portfolio on a given day
    /// </summary>
    public sealed class PortfolioDaySnapshot
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
        public double TotalValue
        {
            get;
        }

        /// <summary>
        /// The total held in BankAccounts.
        /// </summary>
        public double BankAccValue
        {
            get;
        }

        /// <summary>
        /// The total held in securities.
        /// </summary>
        public double SecurityValue
        {
            get;
        }

        /// <summary>
        /// The totals held in each of the securities.
        /// </summary>
        public IDictionary<string, double> SecurityValues
        {
            get;
            set;
        } = new Dictionary<string, double>();

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
        public Dictionary<string, double> BankAccValues
        {
            get;
        } = new Dictionary<string, double>();

        /// <summary>
        /// The total values held in each sector.
        /// </summary>
        public Dictionary<string, double> SectorValues
        {
            get;
        } = new Dictionary<string, double>();

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
                "SecurityTotal"
            };

            foreach (var value in SecurityValues)
            {
                headers.Add(value.Key);
            }

            foreach (var value in BankAccValues)
            {
                headers.Add(value.Key);
            }

            foreach (var value in SectorValues)
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
                SecurityValue.TruncateToString()
            };

            foreach (var value in SecurityValues)
            {
                values.Add(value.Value.TruncateToString());
            }

            foreach (var value in BankAccValues)
            {
                values.Add(value.Value.TruncateToString());
            }

            foreach (var value in SectorValues)
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
        /// <param name="generateSecurityRates">Should security rates be calculated.</param>
        /// <param name="generateSectorRates">Should sector rates be calculated.</param>
        public PortfolioDaySnapshot(DateTime date, IPortfolio portfolio, bool generateSecurityRates, bool generateSectorRates)
        {
            Date = date;
            TotalValue = portfolio.TotalValue(Totals.All, date);
            BankAccValue = portfolio.TotalValue(Totals.BankAccount, date);
            SecurityValue = portfolio.TotalValue(Totals.Security, date);

            AddSecurityValues(date, portfolio, generateSecurityRates);
            AddBankAccountValues(date, portfolio);
            AddSectorTotalValues(date, portfolio, generateSectorRates);
        }

        private void AddSecurityValues(DateTime date, IPortfolio portfolio, bool generateRates)
        {
            List<string> companyNames = portfolio.Companies(Account.Security).ToList();
            companyNames.Sort();

            foreach (string companyName in companyNames)
            {
                SecurityValues.Add(companyName, portfolio.TotalValue(Totals.SecurityCompany, date, new TwoName(companyName)));

                if (generateRates)
                {
                    Security1YrCar.Add(companyName, portfolio.TotalIRR(Totals.SecurityCompany, date.AddDays(-365), date, new TwoName(companyName)));

                    var firstDate = portfolio.FirstValueDate(Totals.SecurityCompany, new TwoName(companyName));
                    double totalIRR = date < firstDate ? 0.0 : portfolio.TotalIRR(Totals.SecurityCompany, firstDate, date, new TwoName(companyName));
                    SecurityTotalCar.Add(companyName, totalIRR);
                }
            }
        }

        private void AddBankAccountValues(DateTime date, IPortfolio portfolio)
        {
            List<string> companyBankNames = portfolio.Companies(Account.BankAccount).ToList();
            companyBankNames.Sort();
            foreach (string companyName in companyBankNames)
            {
                BankAccValues.Add(companyName, portfolio.TotalValue(Totals.BankAccountCompany, date, new TwoName(companyName)));
            }
        }

        private void AddSectorTotalValues(DateTime date, IPortfolio portfolio, bool generateRates)
        {
            IReadOnlyList<string> sectorNames = portfolio.Sectors(Account.Security);
            foreach (string sectorName in sectorNames)
            {
                SectorValues.Add(sectorName, portfolio.TotalValue(Totals.Sector, date, new TwoName(null, sectorName)));

                if (generateRates)
                {
                    var firstDate = portfolio.FirstValueDate(Totals.Sector, new TwoName(null, sectorName));
                    double sectorCAR = date < firstDate ? 0.0 : portfolio.TotalIRR(Totals.Sector, firstDate, date, new TwoName(null, sectorName));
                    CurrentSectorTotalCar.Add(sectorName, sectorCAR);
                }
            }
        }
    }
}
