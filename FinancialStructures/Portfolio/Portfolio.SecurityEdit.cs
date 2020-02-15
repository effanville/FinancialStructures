using FinancialStructures.FinanceStructures;
using FinancialStructures.ReportingStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static partial class PortfolioSecurity
    {
        /// <summary>
        /// Queries whether a security with the specified company and name exists.
        /// </summary>
        public static bool DoesSecurityExist(this Portfolio portfolio, string company, string name)
        {
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetCompany() == company && sec.GetName() == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryRemoveSecurity(this Portfolio portfolio, ErrorReports reports, string company, string name)
        {
            foreach (Security sec in portfolio.Funds)
            {
                if (sec.GetCompany() == company && sec.GetName() == name)
                {
                    portfolio.Funds.Remove(sec);
                    reports.AddReport( $"Security `{company}'-`{name}' removed from the database.", Location.DeletingData);
                    return true;
                }
            }
            reports.AddError( $"Security `{company}'-`{name}' could not be found in the database.", Location.DeletingData);
            return false;
        }

        /// <summary>
        /// Adds the desired data to the security if it can.
        /// </summary>
        public static bool TryAddDataToSecurity(this Portfolio portfolio, ErrorReports reports, string company, string name, DateTime date, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    return portfolio.Funds[fundIndex].TryAddData(reports, date, unitPrice, shares, Investment);
                }
            }
            reports.AddError($"Security `{company}'-`{name}' could not be found in the database.", Location.AddingData);
            return false;
        }

        /// <summary>
        /// Tries to add a security to the underlying global database
        /// </summary>
        public static bool TryAddSecurity(this Portfolio portfolio, ErrorReports reports, string company, string name, string currency, string url, string sectors)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(company))
            {
                reports.AddError( $"Company `{company}' or name `{name}' is not suitable.", Location.AddingData);
                return false;
            }

            if (portfolio.DoesSecurityExist(company, name))
            {
                reports.AddError( $"Security `{company}'-`{name}' already exists.", Location.AddingData);
                return false;
            }

            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(sectors))
            {
                var sectorsSplit = sectors.Split(',');

                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }

            Security newSecurity = new Security(name, company, currency, url, sectorList);
            portfolio.Funds.Add(newSecurity);
            reports.AddReport($"Security `{company}'-`{name}' added to database.", Location.AddingData);
            return true;
        }

        /// <summary>
        /// Renames the security if this exists.
        /// </summary>
        public static bool TryEditSecurityName(this Portfolio portfolio, ErrorReports reports, string company, string name, string newCompany, string newName, string currency, string url, string sectors)
        {
            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(sectors))
            {
                var sectorsSplit = sectors.Split(',');
                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }

            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].TryEditNameCompany(newName, newCompany, currency, url, sectorList, reports);
                }
            }

            return false;
        }

        /// <summary>
        /// Edits the data of the security, if possible.
        /// </summary>
        public static bool TryEditSecurityData(this Portfolio portfolio, ErrorReports reports, string company, string name, DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].TryEditData(reports, oldDate, newDate, shares, unitPrice, Investment);
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the data if it exists.
        /// </summary>
        public static bool TryRemoveSecurityData(this Portfolio portfolio, ErrorReports reports, string company, string name, DateTime date, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].TryDeleteData(reports, date, shares, unitPrice, Investment);
                }
            }

            return false;
        }
    }
}
