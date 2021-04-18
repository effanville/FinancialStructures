using System.Linq;
using System.Windows.Controls;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;

namespace FinancePortfolioDatabase.Tests.ViewModelExtensions
{
    /// <summary>
    /// Contains user like interaction behaviours with the <see cref="SelectedSingleDataViewModel"/>.
    /// </summary>
    public static class SelectedSecurityViewModelExtensions
    {
        public static SecurityDayData AddNewItem(this SelectedSecurityViewModel viewModel)
        {
            viewModel.SelectItem(null);
            viewModel.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            viewModel.SelectedSecurityData.Add(new SecurityDayData());
            var newItem = viewModel.SelectedSecurityData.Last();
            viewModel.SelectItem(newItem);
            viewModel.BeginEdit();

            return newItem;
        }

        public static void SelectItem(this SelectedSecurityViewModel viewModel, SecurityDayData valueToSelect)
        {
            viewModel.SelectionChangedCommand?.Execute(valueToSelect);
        }

        public static void BeginEdit(this SelectedSecurityViewModel viewModel)
        {
            viewModel.PreEditCommand?.Execute(null);
        }

        public static void CompleteEdit(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.AddEditSecurityDataCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }

        public static void DeleteSelected(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.DeleteValuationCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }
    }
}
