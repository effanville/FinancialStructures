using FinancialStructures.FinanceStructures;
using System;

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

        /// <summary>
        /// Adds the desired data to the security if it can.
        /// </summary>
        public static bool TryAddDataToSecurity(this Portfolio portfolio, Action<string, string, string> reportLogger, string company, string name, DateTime date, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    return portfolio.Funds[fundIndex].TryAddData(reportLogger, date, unitPrice, shares, Investment);
                }
            }
            reportLogger("Error", "AddingData", $"Security `{company}'-`{name}' could not be found in the database.");
            return false;
        }

        /// <summary>
        /// Edits the data of the security, if possible.
        /// </summary>
        public static bool TryEditSecurityData(this Portfolio portfolio, Action<string, string, string> reportLogger, string company, string name, DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].TryEditData(reportLogger, oldDate, newDate, shares, unitPrice, Investment);
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the data if it exists.
        /// </summary>
        public static bool TryRemoveSecurityData(this Portfolio portfolio, Action<string, string, string> reportLogger, string company, string name, DateTime date, double shares, double unitPrice, double Investment = 0)
        {
            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].TryDeleteData(reportLogger, date, shares, unitPrice, Investment);
                }
            }

            return false;
        }
    }
}
