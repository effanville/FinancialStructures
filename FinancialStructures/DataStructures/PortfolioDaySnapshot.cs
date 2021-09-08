using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;
using Common.Structure.Extensions;
using System.Linq;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Stores all values and some IRRs of a portfolio on a given day
    /// </summary>
    public class PortfolioDaySnapshot
    {
        /// <summary>
        /// The total value held in the portfolio.
        /// </summary>
        public DailyValuation TotalValue
        {
            get;
        }

        /// <summary>
        /// The total held in BankAccounts.
        /// </summary>
        public DailyValuation BankAccValue
        {
            get;
        }

        /// <summary>
        /// The total held in securities.
        /// </summary>
        public DailyValuation SecurityValue
        {
            get;
        }

        /// <summary>
        /// The totals held in each of the securities.
        /// </summary>
        public Dictionary<string, DailyValuation> SecurityValues
        {
            get;
        } = new Dictionary<string, DailyValuation>();

        /// <summary>
        /// The IRR values for all security companies over the last year.
        /// </summary>
        public Dictionary<string, DailyValuation> Security1YrCar
        {
            get;
        } = new Dictionary<string, DailyValuation>();

        /// <summary>
        /// The IRR values for all security companies over all time.
        /// </summary>
        public Dictionary<string, DailyValuation> SecurityTotalCar
        {
            get;
        } = new Dictionary<string, DailyValuation>();

        /// <summary>
        /// The total values for all bank account companies.
        /// </summary>
        public Dictionary<string, DailyValuation> BankAccValues
        {
            get;
        } = new Dictionary<string, DailyValuation>();

        /// <summary>
        /// The total values held in each sector.
        /// </summary>
        public Dictionary<string, DailyValuation> SectorValues
        {
            get;
        } = new Dictionary<string, DailyValuation>();

        /// <summary>
        /// The total CAR for each sector.
        /// </summary>
        public Dictionary<string, DailyValuation> SectorCar
        {
            get;
        } = new Dictionary<string, DailyValuation>();

        public List<string> ExportHeaders()
        {
            List<string> headers = new List<string>();
            headers.Add("Date");
            headers.Add("TotalValue");
            headers.Add("BankTotal");

            headers.Add("SecurityTotal");

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

        public List<string> ExportValues()
        {
            List<string> values = new List<string>();
            values.Add(TotalValue.Day.ToUkDateString());
            values.Add(TotalValue.Value.ToString());
            values.Add(BankAccValue.Value.ToString());
            values.Add(SecurityValue.Value.ToString());

            foreach (var value in SecurityValues)
            {
                values.Add(value.Value.Value.ToString());
            }

            foreach (var value in BankAccValues)
            {
                values.Add(value.Value.Value.ToString());
            }

            foreach (var value in SectorValues)
            {
                values.Add(value.Value.Value.ToString());
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
        /// <param name="date"></param>
        /// <param name="portfolio"></param>
        public PortfolioDaySnapshot(DateTime date, IPortfolio portfolio)
        {
            TotalValue = new DailyValuation(date, portfolio.TotalValue(Totals.All, date).Truncate());
            BankAccValue = new DailyValuation(date, portfolio.TotalValue(Totals.BankAccount, date).Truncate());
            SecurityValue = new DailyValuation(date, portfolio.TotalValue(Totals.Security, date).Truncate());

            AddSecurityValues(date, portfolio);
            AddBankAccountValues(date, portfolio);
            AddSectorTotalValues(date, portfolio);
        }

        private void AddSecurityValues(DateTime date, IPortfolio portfolio)
        {
            List<string> companyNames = portfolio.Companies(Account.Security).ToList();
            companyNames.Sort();
            foreach (string companyName in companyNames)
            {
                var companyValue = new DailyValuation(date, portfolio.TotalValue(Totals.SecurityCompany, date, new TwoName(companyName)).Truncate());
                SecurityValues.Add(companyName, companyValue);

                var yearCar = new DailyValuation(date, portfolio.IRRTotal(Totals.SecurityCompany, date.AddDays(-365), date, new TwoName(companyName)).Truncate());
                Security1YrCar.Add(companyName, yearCar);

                double totalIRR = date < portfolio.FirstValueDate(Totals.SecurityCompany, new TwoName(companyName)) ? 0.0 : portfolio.IRRTotal(Totals.SecurityCompany, portfolio.FirstValueDate(Totals.SecurityCompany, new TwoName(companyName)), date, new TwoName(companyName)).Truncate();

                SecurityTotalCar.Add(companyName, new DailyValuation(date, totalIRR));
            }
        }

        private void AddBankAccountValues(DateTime date, IPortfolio portfolio)
        {
            List<string> companyBankNames = portfolio.Companies(Account.BankAccount).ToList();
            companyBankNames.Sort();
            foreach (string companyName in companyBankNames)
            {
                var companyValue = new DailyValuation(date, portfolio.TotalValue(Totals.BankAccountCompany, date, new TwoName(companyName)).Truncate());
                BankAccValues.Add(companyName, companyValue);
            }
        }

        private void AddSectorTotalValues(DateTime date, IPortfolio portfolio)
        {
            IReadOnlyList<string> sectorNames = portfolio.Sectors(Account.Security);
            foreach (string sectorName in sectorNames)
            {
                var sectorValue = new DailyValuation(date, portfolio.TotalValue(Totals.Sector, date, new TwoName(null, sectorName)).Truncate());
                SectorValues.Add(sectorName, sectorValue);

                double sectorCAR = date < portfolio.FirstValueDate(Totals.Sector, new TwoName(null, sectorName)) ? 0.0 : portfolio.IRRTotal(Totals.Sector, portfolio.FirstValueDate(Totals.Sector, new TwoName(null, sectorName)), date, new TwoName(null, sectorName)).Truncate();

                SectorCar.Add(sectorName, new DailyValuation(date, sectorCAR));
            }
        }
    }
}
