using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using GUISupport;
using System;
using System.Collections.Generic;

namespace FinanceCommonViewModels
{
    internal abstract class ViewModelBase : PropertyChangedBase
    {
        public abstract void UpdateData(Portfolio portfolio, List<Sector> sectors);

        public abstract Action<NameData> LoadSelectedTab { get; set; }
    }
}
