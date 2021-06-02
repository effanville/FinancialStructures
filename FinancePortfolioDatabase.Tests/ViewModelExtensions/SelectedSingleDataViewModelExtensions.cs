using System.Linq;
using System.Windows.Controls;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using StructureCommon.DataStructures;

namespace FinancePortfolioDatabase.Tests.ViewModelExtensions
{
    /// <summary>
    /// Contains user like interaction behaviours with the <see cref="SelectedSingleDataViewModel"/>.
    /// </summary>
    public static class SelectedSingleDataViewModelExtensions
    {
        public static DailyValuation AddNewItem(this SelectedSingleDataViewModel viewModel)
        {
            viewModel.SelectItem(null);
            viewModel.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            viewModel.SelectedData.Add(new DailyValuation());
            var newItem = viewModel.SelectedData.Last();
            viewModel.SelectItem(newItem);
            viewModel.BeginEdit();

            return newItem;
        }

        public static void SelectItem(this SelectedSingleDataViewModel viewModel, DailyValuation valueToSelect)
        {
            viewModel.SelectionChangedCommand?.Execute(valueToSelect);
        }

        public static void BeginEdit(this SelectedSingleDataViewModel viewModel)
        {
            viewModel.PreEditCommand?.Execute(null);
        }

        public static void CompleteEdit(this SelectedSingleDataViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.EditDataCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }

        public static void DeleteSelected(this SelectedSingleDataViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.DeleteValuationCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }
    }
}
