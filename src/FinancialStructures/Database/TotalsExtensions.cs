using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database
{
    public static class TotalsExtensions
    {
        public static string GetIdentifier(this Totals totals, TwoName name)
        {
            switch (totals)
            {
                case Totals.Sector:
                case Totals.SecuritySector:
                case Totals.BankAccountSector:
                case Totals.AssetSector:
                case Totals.PensionSector:
                case Totals.CurrencySector:
                case Totals.Currency:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                case Totals.AssetCurrency:
                case Totals.PensionCurrency:
                    return name.Name;
                case Totals.All:
                case Totals.SecurityCompany:
                case Totals.BankAccountCompany:
                case Totals.AssetCompany:
                case Totals.PensionCompany:
                case Totals.Company:
                case Totals.Security:
                case Totals.Benchmark:
                case Totals.BankAccount:
                case Totals.Asset:
                case Totals.Pension:
                default:
                    return name.Company;
            }
        }
    }
}