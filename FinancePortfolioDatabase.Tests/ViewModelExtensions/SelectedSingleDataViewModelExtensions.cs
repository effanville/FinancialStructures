using System.Linq;
using Common.Structure.DataStructures;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;

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
            viewModel.TLVM.Valuations.Add(viewModel.TLVM.DefaultNewItem());
            DailyValuation newItem = viewModel.TLVM.Valuations.Last();
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
