using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
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
            viewModel.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            viewModel.Trades.Add(new SecurityTrade());
            var newItem = viewModel.Trades.Last();
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

        [STAThread]
        public static void DeleteSelectedTrade(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            KeyEventArgs eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, Key.Delete);
            eventArgs.RoutedEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridCell));
            eventArgs.Source = new DataGridCell();
            viewModel.DeleteTradeKeyDownCommand?.Execute(eventArgs);
            viewModel.UpdateData(portfolio);
        }

        public static DailyValuation AddNewUnitPrice(this SelectedSecurityViewModel viewModel)
        {
            viewModel.SelectUnitPrice(null);
            viewModel.TLVM.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            viewModel.TLVM.Valuations.Add(new DailyValuation());
            var newItem = viewModel.TLVM.Valuations.Last();
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

        [STAThread]
        public static void DeleteSelected(this SelectedSecurityViewModel viewModel, IPortfolio portfolio)
        {
            KeyEventArgs eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, Key.Delete);
            eventArgs.RoutedEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler),typeof(DataGridCell));
            eventArgs.Source = new DataGridCell();
            viewModel.TLVM.DeleteValuationCommand?.Execute(eventArgs);
            viewModel.UpdateData(portfolio);
        }
    }
}
