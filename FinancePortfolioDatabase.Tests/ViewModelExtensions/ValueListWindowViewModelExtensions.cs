using System.Collections.Generic;
using System.Linq;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.Tests.ViewModelExtensions
{
    /// <summary>
    /// Contains user like interaction behaviours with the <see cref="ValueListWindowViewModel"/>.
    /// </summary>
    public static class ValueListWindowViewModelExtensions
    {
        public static DataNamesViewModel DataNames(this ValueListWindowViewModel viewModel)
        {
            IEnumerable<object> desiredViewModel = viewModel.Tabs.Where(vm => vm is DataNamesViewModel svm);
            return desiredViewModel.First() as DataNamesViewModel;
        }

        public static SelectedSingleDataViewModel SelectedTab(this ValueListWindowViewModel viewModel, NameData name)
        {
            IEnumerable<object> desiredViewModel = viewModel.Tabs.Where(vm => vm is SelectedSingleDataViewModel svm && svm.SelectedName.Equals(name));
            return desiredViewModel.First() as SelectedSingleDataViewModel;
        }
    }
}
