using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.FinanceStructures
{
    public class CashAccount : SingleValueDataList
    {
        public new CashAccount Copy()
        {
            return new CashAccount(Names, Values);
        }

        public TimeList Amounts
        {
            get { return Values; }
            set { Values = value; }
        }

        /// <summary>
        /// Default constructor where no data is known.
        /// </summary>
        internal CashAccount(NameData names)
            : base(names)
        {
        }

        /// <summary>
        /// Constructor used when data is known.
        /// </summary>
        private CashAccount(NameData names, TimeList amounts)
            : base(names, amounts)
        {
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
            return Names.Currency;
        }

        /// <summary>
        /// Returns the sectors associated to this OldCashAccount.
        /// </summary>
        /// <returns></returns>
        public List<string> GetSectors()
        {
            return Names.Sectors;
        }

        public bool EditNameData(string company, string name, string url, string currency, List<string> sectors)
        {
            if (Names.Currency != currency)
            {
                Names.Currency = currency;
            }

            Names.Sectors = sectors;

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
                Names.Sectors.Remove(sectorName);
                return true;
            }

            return false;
        }

        public bool TryAddSector(string sectorName)
        {
            if (!IsSectorLinked(sectorName))
            {
                Names.Sectors.Add(sectorName);
                return true;
            }

            return false;
        }

        internal bool IsSectorLinked(string sectorName)
        {
            if (Names.Sectors != null && Names.Sectors.Any())
            {
                foreach (var name in Names.Sectors)
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
