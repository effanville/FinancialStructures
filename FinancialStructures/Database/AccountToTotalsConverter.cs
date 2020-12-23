namespace FinancialStructures.Database
{
    public static class AccountToTotalsConverter
    {
        /// <summary>
        /// Converts a Account enum to a Totals enum.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Totals ConvertAccountToTotal(Account element)
        {
            switch (element)
            {
                default:
                case Account.All:
                {
                    return Totals.All;
                }
                case Account.Security:
                {
                    return Totals.Security;
                }
                case Account.BankAccount:
                {
                    return Totals.BankAccount;
                }
                case Account.Benchmark:
                {
                    return Totals.Benchmark;
                }
                case Account.Currency:
                {
                    return Totals.Currency;
                }
            }
        }

        /// <summary>
        /// Converts a Account enum to a Totals enum.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Account ConvertTotalToAccount(Totals element)
        {
            switch (element)
            {
                case Totals.All:
                case Totals.Sector:
                case Totals.Company:
                case Totals.CurrencySector:
                default:
                {
                    return Account.All;
                }
                case Totals.Security:
                case Totals.SecurityCompany:
                case Totals.SecuritySector:
                case Totals.SecurityCurrency:
                {
                    return Account.Security;
                }
                case Totals.BankAccount:
                case Totals.BankAccountCompany:
                case Totals.BankAccountSector:
                case Totals.BankAccountCurrency:
                {
                    return Account.BankAccount;
                }
                case Totals.Benchmark:
                {
                    return Account.Benchmark;
                }
                case Totals.Currency:
                {
                    return Account.Currency;
                }
            }
        }


        /// <summary>
        /// Converts a Account enum to a Totals enum.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Totals ConvertTotalToCompanyTotal(Totals element)
        {
            switch (element)
            {
                case Totals.All:
                case Totals.Sector:
                case Totals.Company:
                case Totals.CurrencySector:
                default:
                {
                    return Totals.All;
                }
                case Totals.Security:
                case Totals.SecurityCompany:
                case Totals.SecuritySector:
                case Totals.SecurityCurrency:
                {
                    return Totals.SecurityCompany;
                }
                case Totals.BankAccount:
                case Totals.BankAccountCompany:
                case Totals.BankAccountSector:
                case Totals.BankAccountCurrency:
                {
                    return Totals.BankAccount;
                }
                case Totals.Benchmark:
                {
                    return Totals.Benchmark;
                }
                case Totals.Currency:
                {
                    return Totals.Currency;
                }
            }
        }
    }
}
