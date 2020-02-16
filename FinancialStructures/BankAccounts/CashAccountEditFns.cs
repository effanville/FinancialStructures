using FinancialStructures.DataStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Here contains various editors for the CashAccount class.
    /// </summary>
    public partial class CashAccount
    {
        /// <summary>
        /// Compares another security and determines if has same name and company.
        /// </summary>
        internal bool IsEqualTo(CashAccount otherAccount)
        {
            if (otherAccount.GetName() != fName)
            {
                return false;
            }

            if (otherAccount.GetCompany() != fCompany)
            {
                return false;
            }

            return true;
        }

        public int Count()
        {
            return fAmounts.Count();
        }
        /// <summary>
        /// Adds <param name="value"/> to amounts on <param name="date"/> if data doesnt exist.
        /// </summary>
        internal bool TryAddValue(DateTime date, double value)
        {
            if (fAmounts.ValueExists(date, out int _))
            {
                return false;
            }

            return fAmounts.TryAddValue(date, value);
        }

        /// <summary>
        /// Trys to get latest data earlier than date requested. Only returns true if all data present.
        /// </summary>
        public bool TryGetEarlierData(DateTime date, out DailyValuation Value)
        {
            return fAmounts.TryGetNearestEarlierValue(date, out Value);
        }

        /// <summary>
        /// Produces a list of data for GUI visualisation.
        /// </summary>
        internal List<AccountDayDataView> GetDataForDisplay()
        {
            var output = new List<AccountDayDataView>();
            foreach (var datevalue in fAmounts.GetValuesBetween(fAmounts.FirstDate(), fAmounts.LatestDate()))
            {
                fAmounts.TryGetValue(datevalue.Day, out double UnitPrice);
                var thisday = new AccountDayDataView(datevalue.Day, UnitPrice, false);
                output.Add(thisday);
            }

            return output;
        }

        /// <summary>
        /// Edits value if value exists. Does nothing if it doesn't exist.
        /// </summary>
        internal bool TryEditValue(DateTime oldDate, DateTime date, double value, ErrorReports reports)
        {
            return fAmounts.TryEditData(oldDate, date, value, reports);
        }

        /// <summary>
        /// Edits the name and company of the CashAccount.
        /// </summary>
        internal bool EditNameCompany(string name, string company, string currency, List<string> sectors)
        {
            if (fCompany != company)
            {
                fCompany = company;
            }
            if (fName != name)
            {
                fName = name;
            }
            if (fCurrency != currency)
            {
                fCurrency = currency;
            }

            fSectors = sectors;

            return true;
        }

        /// <summary>
        /// Removes data on <paramref name="date"/> if it exists.
        /// </summary>
        internal bool TryDeleteData(DateTime date, ErrorReports reports)
        {
            return fAmounts.TryDeleteValue(date, reports);
        }

        /// <summary>
        /// Removes a sector associated to this CashAccount.
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

        /// <summary>
        /// Produces a copy of the specified CashAccount.
        /// </summary>
        public CashAccount Copy()
        {
            return new CashAccount(fName, fCompany, fCurrency, fAmounts);
        }

        /// <summary>
        /// Returns the name of the CashAccount.
        /// </summary>
        public string GetName()
        {
            return fName;
        }

        /// <summary>
        /// Returns the company of the CashAccount.
        /// </summary>
        public string GetCompany()
        {
            return fCompany;
        }

        /// <summary>
        /// Returns the Url of the CashAccount.
        /// </summary>
        public string GetUrl()
        {
            return fUrl;
        }

        /// <summary>
        /// Returns the currency of the CashAccount.
        /// </summary>
        public string GetCurrency()
        {
            return fCurrency;
        }

        /// <summary>
        /// Returns the sectors associated to this CashAccount.
        /// </summary>
        /// <returns></returns>
        public List<string> GetSectors()
        {
            return fSectors;
        }
    }
}
