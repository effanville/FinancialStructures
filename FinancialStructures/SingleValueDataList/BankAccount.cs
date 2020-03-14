using FinancialStructures.DataStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    public class CashAccount : SingleValueDataList
    {
        public new CashAccount Copy()
        {
            return new CashAccount(Company, Name, fCurrency, Values);
        }

        private List<string> fSectors = new List<string>();

        /// <summary>
        /// The Sectors associated with this CashAccount
        /// </summary>
        public List<string> Sectors
        {
            get { return fSectors; }
            set { fSectors = value; }
        }

        /// <summary>
        /// The company name associated to the account.
        /// </summary>
        private string fCurrency;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Currency
        {
            get { return fCurrency; }
            set { fCurrency = value; }
        }

        public TimeList Amounts
        {
            get { return Values; }
            set { Values = value; }
        }

        /// <summary>
        /// Default constructor where no data is known.
        /// </summary>
        internal CashAccount(string company, string name, string currency)
            : base(company, name, string.Empty)
        {
            fCurrency = currency;
        }

        /// <summary>
        /// Constructor used when data is known.
        /// </summary>
        private CashAccount(string company, string name, string currency, TimeList amounts)
            : base(company, name, string.Empty, amounts)
        {
            fCurrency = currency;
        }

        /// <summary>
        /// Parameterless constructor for serialisation.
        /// </summary>
        private CashAccount()
            : base()
        {
        }

        /// <summary>
        /// Returns the interpolated value of the security on the date provided.
        /// </summary>
        internal DailyValuation Value(DateTime date, SingleValueDataList currency = null)
        {
            DailyValuation perSharePrice = Values.ValueZeroBefore(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(date).Value;
            double value = perSharePrice.Value * currencyValue;
            return new DailyValuation(date, value);
        }

        /// <summary>
        /// Returns the currency of the OldCashAccount.
        /// </summary>
        public string GetCurrency()
        {
            return fCurrency;
        }

        /// <summary>
        /// Returns the sectors associated to this OldCashAccount.
        /// </summary>
        /// <returns></returns>
        public List<string> GetSectors()
        {
            return fSectors;
        }

        public bool EditNameData(string company, string name, string url, string currency, List<string> sectors)
        {
            if (fCurrency != currency)
            {
                fCurrency = currency;
            }

            fSectors = sectors;

            return base.EditNameData(company, name, url);
        }

        /// <summary>
        /// Returns the latest valuation of the OldCashAccount.
        /// </summary>
        internal DailyValuation LatestValue(SingleValueDataList currency = null)
        {
            DailyValuation latestDate = Values.LatestValuation();
            if (latestDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0); ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(latestDate.Day).Value;
            double latestValue = latestDate.Value * currencyValue;

            return new DailyValuation(latestDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the first valuation of the OldCashAccount.
        /// </summary>
        internal DailyValuation FirstValue(SingleValueDataList currency = null)
        {
            DailyValuation firstDate = Values.FirstValuation();
            if (firstDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0); ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(firstDate.Day).Value;
            double latestValue = firstDate.Value * currencyValue;

            return new DailyValuation(firstDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the latest earlier valuation of the OldCashAccount to <paramref name="date"/>.
        /// </summary>
        internal DailyValuation NearestEarlierValuation(DateTime date, SingleValueDataList currency = null)
        {
            var value = Values.NearestEarlierValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
            value.SetValue(value.Value * currencyValue);
            return value;
        }

        /// <summary>
        /// Returns earliest valuation after the date specified. 
        /// </summary>
        internal DailyValuation NearestLaterValuation(DateTime date, SingleValueDataList currency = null)
        {
            var value = Values.NearestLaterValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
            value.SetValue(value.Value * currencyValue);
            return value;
        }

        /// <summary>
        /// Removes a sector associated to this OldCashAccount.
        /// </summary>
        /// <param name="sectorName"></param>
        /// <returns></returns>
        public bool TryRemoveSector(string sectorName)
        {
            if (IsSectorLinked(sectorName))
            {
                fSectors.Remove(sectorName);
                return true;
            }

            return false;
        }

        public bool TryAddSector(string sectorName)
        {
            if (!IsSectorLinked(sectorName))
            {
                fSectors.Add(sectorName);
                return true;
            }

            return false;
        }

        internal bool IsSectorLinked(string sectorName)
        {
            if (fSectors != null && fSectors.Count > 0)
            {
                foreach (var name in fSectors)
                {
                    if (name == sectorName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
