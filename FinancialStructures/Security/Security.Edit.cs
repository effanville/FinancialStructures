using FinancialStructures.DataStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains data editing of a security class.
    /// </summary>
    public partial class Security
    {
        /// <summary>
        /// Compares another security and determines if has same name and company.
        /// </summary>
        internal bool IsEqualTo(Security otherSecurity)
        {
            if (otherSecurity.GetName() != fName)
            {
                return false;
            }

            if (otherSecurity.GetCompany() != fCompany)
            {
                return false;
            }

            return true;
        }

        public int Count()
        {
            return fUnitPrice.Count();
        }
        /// <summary>
        /// Returns true if shares and unit prices have an item or are not null.
        /// </summary>
        internal bool Any()
        {
            if (fUnitPrice.Any() && fShares.Any())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Makes a copy of the security.
        /// </summary>
        internal Security Copy()
        {
            return new Security(fCompany, fName, fCurrency, fUrl, fShares, fUnitPrice, fInvestments);
        }

        /// <summary>
        /// Returns the name of the security.
        /// </summary>
        public string GetName()
        {
            return fName;
        }

        /// <summary>
        /// Returns the company field of the security
        /// </summary>
        public string GetCompany()
        {
            return fCompany;
        }

        /// <summary>
        /// Returns the currency field of the security
        /// </summary>
        public string GetCurrency()
        {
            return fCurrency;
        }

        /// <summary>
        /// Returns the Uri field of the security
        /// </summary>
        public string GetUrl()
        {
            return fUrl;
        }

        /// <summary>
        /// Returns the sectors associated to this security.
        /// </summary>
        /// <returns></returns>
        public List<string> GetSectors()
        {
            return fSectors;
        }

        public DayDataView DayData(DateTime day)
        {
            fUnitPrice.TryGetValue(day, out double UnitPrice);
            fShares.TryGetValue(day, out double shares);
            fInvestments.TryGetValue(day, out double invest);
            return new DayDataView(day, UnitPrice, shares, invest);
        }

        /// <summary>
        /// Produces a list of data for visual display purposes. Display in the base currency of the fund ( so this does not modify values due to currency)
        /// </summary>
        internal List<DayDataView> GetDataForDisplay()
        {
            var output = new List<DayDataView>();
            if (fUnitPrice.Any())
            {
                foreach (var datevalue in fUnitPrice.GetValuesBetween(fUnitPrice.FirstDate(), fUnitPrice.LatestDate()))
                {
                    fUnitPrice.TryGetValue(datevalue.Day, out double UnitPrice);
                    fShares.TryGetValue(datevalue.Day, out double shares);
                    fInvestments.TryGetValue(datevalue.Day, out double invest);
                    var thisday = new DayDataView(datevalue.Day, UnitPrice, shares, invest);
                    output.Add(thisday);
                }
            }
            if (fShares.Any())
            {
                foreach (var datevalue in fShares.GetValuesBetween(fShares.FirstDate(), fShares.LatestDate()))
                {
                    if (!fUnitPrice.TryGetValue(datevalue.Day, out double _))
                    {
                        fShares.TryGetValue(datevalue.Day, out double shares);
                        fInvestments.TryGetValue(datevalue.Day, out double invest);
                        var thisday = new DayDataView(datevalue.Day, double.NaN, shares, invest);
                        output.Add(thisday);
                    }
                }
            }
            if (fInvestments.Any())
            {
                foreach (var datevalue in fInvestments.GetValuesBetween(fInvestments.FirstDate(), fInvestments.LatestDate()))
                {
                    if (!fUnitPrice.TryGetValue(datevalue.Day, out double _) && !fShares.TryGetValue(datevalue.Day, out double _))
                    {
                        fInvestments.TryGetValue(datevalue.Day, out double invest);
                        var thisday = new DayDataView(datevalue.Day, double.NaN, double.NaN, invest);

                        output.Add(thisday);
                    }
                }
            }
            output.Sort();
            return output;
        }

        /// <summary>
        /// Checks if SharePrice data for the date specified exists. if so outputs index value
        /// </summary>
        internal bool DoesDateSharesDataExist(DateTime date, out int index)
        {
            return fShares.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        internal bool DoesDateUnitPriceDataExist(DateTime date, out int index)
        {
            return fUnitPrice.ValueExists(date, out index);
        }

        /// <summary>
        /// Checks if UnitPrice data for the date specified exists. if so outputs index value
        /// </summary>
        internal bool DoesDateInvestmentDataExist(DateTime date, out int index)
        {
            return fInvestments.ValueExists(date, out index);
        }

        /// <summary>
        /// Attempts to add data for the date specified.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        internal bool TryAddData(Action<string, string, string> reportLogger, DateTime date, double unitPrice, double shares = 0, double investment = 0)
        {
            if (DoesDateSharesDataExist(date, out int _) || DoesDateInvestmentDataExist(date, out int _) || DoesDateUnitPriceDataExist(date, out int _))
            {
                reportLogger("Error", "AddingData", $"Security `{fCompany}'-`{fName}' already has NumShares or UnitPrice or Investment data on {date.ToString("d")}.");
                return false;
            }

            return fShares.TryAddValue(date, shares) & fUnitPrice.TryAddValue(date, unitPrice) & fInvestments.TryAddValue(date, investment) && ComputeInvestments(reportLogger);
        }

        /// <summary>
        /// Adds the value to the data with todays date and with the latest number of shares.
        /// </summary>
        public void UpdateSecurityData(double value, Action<string, string, string> reportLogger, DateTime day)
        {
            // best approximation for number of units is last known number of units.
            TryGetEarlierData(day, out DailyValuation _, out DailyValuation units, out DailyValuation _);
            if (units == null)
            {
                units = new DailyValuation(day, 0);
            }

            TryAddData(reportLogger, day, value, units.Value);
        }


        /// <summary>
        /// Try to edit data. If any dont have any relevant values, then do not edit
        /// If do have relevant values, then edit that value
        /// If investment value doesnt exist, then add that value.
        /// </summary>
        internal bool TryEditData(Action<string, string, string> reportLogger, DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment)
        {
            bool editShares = false;
            bool editUnitPrice = false;
            if (DoesDateSharesDataExist(oldDate, out int _))
            {
                editShares = fShares.TryEditData(oldDate, newDate, shares, reportLogger);
            }

            if (DoesDateUnitPriceDataExist(oldDate, out int _))
            {
                editUnitPrice = fUnitPrice.TryEditData(oldDate, newDate, unitPrice, reportLogger);
            }

            fInvestments.TryEditDataOtherwiseAdd(oldDate, newDate, Investment, reportLogger);

            return editShares & editUnitPrice && ComputeInvestments(reportLogger);
        }

        /// <summary>
        /// Edits name and company data of security.
        /// </summary>
        internal bool EditNameData(string company, string name, string currency, string url, List<string> sectors, Action<string, string, string> reportLogger)
        {
            if (name != fName)
            {
                fName = name;
            }
            if (company != fCompany)
            {
                fCompany = company;
            }
            if (url != fUrl)
            {
                fUrl = url;
            }
            if (currency != fCurrency)
            {
                fCurrency = currency;
            }
            if (sectors != fSectors)
            {
                fSectors = sectors;
            }

            return true;
        }

        internal bool TryRemoveSector(string sectorName)
        {
            if (IsSectorLinked(sectorName))
            {
                fSectors.Remove(sectorName);
                return true;
            }

            return false;
        }

        internal bool TryAddSector(string sectorName)
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

        internal int NumberSectors()
        {
            return Sectors.Count;
        }

        /// <summary>
        /// Trys to get data on specific date. Only returns true if all data present.
        /// </summary>
        internal bool TryGetData(DateTime date, out double price, out double units, out double investment)
        {
            return fUnitPrice.TryGetValue(date, out price) & fShares.TryGetValue(date, out units) & fInvestments.TryGetValue(date, out investment);
        }

        /// <summary>
        /// Trys to get latest data earlier than date requested. Only returns true if all data present.
        /// </summary>
        internal bool TryGetEarlierData(DateTime date, out DailyValuation price, out DailyValuation units, out DailyValuation investment)
        {
            return fUnitPrice.TryGetNearestEarlierValue(date, out price) & fShares.TryGetNearestEarlierValue(date, out units) & fInvestments.TryGetNearestEarlierValue(date, out investment);
        }

        /// <summary>
        /// Tries to delete the data. If it can, it deletes all data specified, then returns true only if all data has been successfully deleted.
        /// </summary>
        internal bool TryDeleteData(Action<string, string, string> reportLogger, DateTime date)
        {
            return fUnitPrice.TryDeleteValue(date, reportLogger) & fShares.TryDeleteValue(date, reportLogger) & fInvestments.TryDeleteValue(date, reportLogger) && ComputeInvestments(reportLogger);
        }

        /// <summary>
        /// Upon a new/edit investment, one needs to recompute the values of the investments
        /// One should not change Inv = 0 or Inv > 0  to ensure that dividend reivestments are not accidentally included in a new investment.
        /// This though causes a problem if a value is deleted.
        /// </summary>
        /// <remarks>
        /// This should be called throughout, whenever one updates the data stored in the Security.
        /// </remarks>
        private bool ComputeInvestments(Action<string, string, string> reportLogger)
        {
            // return true;
            for (int index = 0; index < fInvestments.Count(); index++)
            {
                var investmentValue = fInvestments[index];
                if (investmentValue.Value != 0)
                {
                    DailyValuation sharesCurrentValue = fShares.NearestEarlierValue(investmentValue.Day);
                    DailyValuation sharesPreviousValue = fShares.RecentPreviousValue(investmentValue.Day) ?? new DailyValuation(DateTime.Today, 0);
                    if (sharesCurrentValue != null)
                    {
                        fInvestments.TryEditData(investmentValue.Day, (sharesCurrentValue.Value - sharesPreviousValue.Value) * fUnitPrice.NearestEarlierValue(investmentValue.Day).Value, reportLogger);
                    }
                }
                if (investmentValue.Value == 0)
                {
                    if (fInvestments.TryDeleteValue(investmentValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }

            return true;
        }
    }
}
