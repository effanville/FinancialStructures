using System.Linq;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.Tests.ViewModelExtensions
{
    /// <summary>
    /// Contains user like interaction behaviours with the <see cref="SecurityEditWindowViewModel"/>.
    /// </summary>
    public static class SecurityEditWindowViewModelExtensions
    {
        public static DataNamesViewModel DataNames(this SecurityEditWindowViewModel viewModel)
        {
            var desiredViewModel = viewModel.Tabs.Where(vm => vm is DataNamesViewModel svm);
            return desiredViewModel.First() as DataNamesViewModel;
        }

        public static SelectedSecurityViewModel SelectedTab(this SecurityEditWindowViewModel viewModel, NameData name)
        {
            var desiredViewModel = viewModel.Tabs.Where(vm => vm is SelectedSecurityViewModel svm && svm.SelectedName.Equals(name));
            return desiredViewModel.First() as SelectedSecurityViewModel;
        }
    }
}
