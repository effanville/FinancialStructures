using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Common.Structure.DataStructures;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.Database;

namespace FinancePortfolioDatabase.Tests.ViewModelExtensions
{
    /// <summary>
    /// Contains user like interaction behaviours with the <see cref="SelectedSingleDataViewModel"/>.
    /// </summary>
    public static class SelectedSecurityViewModelExtensions
    {
        public static DailyValuation AddNewItem(this SelectedSecurityViewModel viewModel)
        {
            viewModel.SelectItem(null);
            viewModel.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            viewModel.TLVM.Valuations.Add(new DailyValuation());
            var newItem = viewModel.TLVM.Valuations.Last();
            viewModel.SelectItem(newItem);
            viewModel.BeginEdit();

            return newItem;
        }

        public static void SelectItem(this SelectedSecurityViewModel viewModel, DailyValuation valueToSelect)
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

        [STAThread]
        public static void DeleteSelected(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            KeyEventArgs eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, Key.Delete);
            viewModel.TLVM.DeleteValuationCommand?.Execute(eventArgs);
            viewModel.UpdateData(portfolio);
        }
    }
}
