using System.Linq;
using System.Windows.Controls;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using Common.Structure.DataStructures;

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
            viewModel.TLVM.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            viewModel.TLVM.Valuations.Add(new DailyValuation());
            var newItem = viewModel.TLVM.Valuations.Last();
            viewModel.SelectItem(newItem);
            viewModel.BeginEdit();

            return newItem;
        }

        public static void SelectItem(this SelectedSingleDataViewModel viewModel, DailyValuation valueToSelect)
        {
            viewModel.TLVM.SelectionChangedCommand?.Execute(valueToSelect);
        }

        public static void BeginEdit(this SelectedSingleDataViewModel viewModel)
        {
            viewModel.TLVM.PreEditCommand?.Execute(null);
        }

        public static void CompleteEdit(this SelectedSingleDataViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.TLVM.AddEditDataCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }

        public static void DeleteSelected(this SelectedSingleDataViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.DeleteValuationCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }
    }
}
