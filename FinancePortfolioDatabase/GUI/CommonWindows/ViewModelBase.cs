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
        public ViewModelBase(string header)
        {
            Header = header;
        }

        public ViewModelBase(string header, Action<NameData> loadTab)
        {
            Header = header;
            LoadSelectedTab = loadTab;
        }

        public virtual string Header { get; }
        public virtual bool Closable { get; }

        public abstract void UpdateData(Portfolio portfolio, List<Sector> sectors);

        public virtual Action<NameData> LoadSelectedTab { get; set; }
    }
}
