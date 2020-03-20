using FinancialStructures.DataStructures;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    internal interface INameDataObtainer
    {
        bool Any();

        DailyValuation LatestValue(Currency currency = null);

        string GetCompany();
        string GetName();
        string GetCurrency();
        string GetUrl();
        List<string> GetSectors();
    }
}
