using System.Linq;
using Common.Structure.DataStructures;
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
        public static SecurityTrade AddNewTrade(this SelectedSecurityViewModel viewModel)
        {
            viewModel.SelectTrade(null);
            viewModel.Trades.Add(viewModel.DefaultTradeValue());
            SecurityTrade newItem = viewModel.Trades.Last();
            viewModel.SelectTrade(newItem);
            viewModel.BeginEdit();

            return newItem;
        }

        public static void SelectTrade(this SelectedSecurityViewModel viewModel, SecurityTrade valueToSelect)
        {
            viewModel.SelectionChangedCommand?.Execute(valueToSelect);
        }

        public static void BeginEditTrade(this SelectedSecurityViewModel viewModel)
        {
            viewModel.PreEditCommand?.Execute(null);
        }

        public static void CompleteEditTrade(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.AddEditDataCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }

        public static void DeleteSelectedTrade(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.DeleteTrade();
            viewModel.UpdateData(portfolio);
        }

        public static DailyValuation AddNewUnitPrice(this SelectedSecurityViewModel viewModel)
        {
            viewModel.SelectUnitPrice(null);
            viewModel.TLVM.Valuations.Add(new DailyValuation());
            DailyValuation newItem = viewModel.TLVM.Valuations.Last();
            viewModel.SelectUnitPrice(newItem);
            viewModel.BeginEdit();

            return newItem;
        }

        public static void SelectUnitPrice(this SelectedSecurityViewModel viewModel, DailyValuation valueToSelect)
        {
            viewModel.TLVM.SelectionChangedCommand?.Execute(valueToSelect);
        }

        public static void BeginEdit(this SelectedSecurityViewModel viewModel)
        {
            viewModel.TLVM.PreEditCommand?.Execute(null);
        }

        public static void CompleteEdit(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.TLVM.AddEditDataCommand?.Execute(null);
            viewModel.UpdateData(portfolio);
        }

        public static void DeleteSelected(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            viewModel.TLVM.DeleteValuation();
            viewModel.UpdateData(portfolio);
        }
    }
}
