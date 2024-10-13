using Effanville.FinancialStructures.Database;

namespace Effanville.FinancialStructures.FinanceStructures.Extensions
{
    public static class AccountExtensions
    {
        public static decimal DefaultValue(this Account accountType)
        {
            if (accountType == Account.Currency || accountType == Account.Benchmark)
            {
                return 1.0m;
            }

            return 0.0m;
        }
    }
}