using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using GUISupport;
using System;
using System.Collections.Generic;

namespace FinanceCommonViewModels
{
    /// <summary>
    /// Base for ViewModels containing display purpose objects.
    /// </summary>
    internal abstract class ViewModelBase : PropertyChangedBase
    {
        public virtual string Header { get; }
        public virtual bool Closable { get { return false; } }

        public virtual Action<NameData> LoadSelectedTab { get; set; }

        public ViewModelBase(string header)
        {
            Header = header;
        }

        public ViewModelBase(string header, Action<NameData> loadTab)
        {
            Header = header;
            LoadSelectedTab = loadTab;
        }

        public abstract void UpdateData(Portfolio portfolio, List<Sector> sectors);
    }
}
