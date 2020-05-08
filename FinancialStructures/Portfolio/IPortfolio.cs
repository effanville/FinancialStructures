using FinancialStructures.FinanceStructures;
using FinancialStructures.PortfolioAPI;
using System.Collections.Generic;

namespace FinancialStructures.FinanceInterfaces
{
    public interface IPortfolio
    {
        void SetFilePath(string path);
        string FilePath { get; }
        string Extension { get; }

        string Directory { get; }
        string DatabaseName { get; }
        string BaseCurrency { get; set; }

        List<Security> Funds { get; }
        List<CashAccount> BankAccounts { get; }
        List<Currency> Currencies { get; }
        List<Sector> BenchMarks { get; }

        void CopyData(IPortfolio portfolio);

        void SetBenchMarks(List<Sector> sectors);

        int NumberOf(AccountType accountType);
    }
}
