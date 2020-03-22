using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using GUISupport;
using System;

namespace FinanceCommonViewModels
{
    /// <summary>
    /// Base for ViewModels containing display purpose objects.
    /// </summary>
    internal abstract class ViewModelBase : PropertyChangedBase
    {
        public virtual string Header { get; }
        public virtual bool Closable { get { return false; } }

        public virtual Action<NameData_ChangeLogged> LoadSelectedTab { get; set; }

        public ViewModelBase(string header)
        {
            Header = header;
        }

        public ViewModelBase(string header, Action<NameData_ChangeLogged> loadTab)
        {
            Header = header;
            LoadSelectedTab = loadTab;
        }

        public virtual void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
        }

        public abstract void UpdateData(IPortfolio portfolio);
    }
}
