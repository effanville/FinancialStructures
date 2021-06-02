using System.Linq;
using System.Windows.Controls;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.NamingStructures;
using StructureCommon.DisplayClasses;

namespace FinancePortfolioDatabase.Tests.ViewModelExtensions
{
    /// <summary>
    /// Contains user like interaction behaviours with the <see cref="DataNamesViewModel"/>.
    /// </summary>
    public static class DataNamesViewModelExtensions
    {
        public static void SelectItem(this DataNamesViewModel viewModel, NameData name)
        {
            viewModel.SelectionChangedCommand?.Execute(new SelectableEquatable<NameData>(name, false));
        }

        public static void BeginEdit(this DataNamesViewModel viewModel)
        {
            viewModel.PreEditCommand?.Execute(null);
        }
        public static void CompleteEdit(this DataNamesViewModel viewModel)
        {
            viewModel.CreateCommand?.Execute(null);
        }

        public static NameData AddNewItem(this DataNamesViewModel viewModel)
        {
            viewModel.SelectItem(null);
            viewModel.AddDefaultDataCommand?.Execute(new AddingNewItemEventArgs());
            viewModel.DataNames.Add(new SelectableEquatable<NameData>(new NameData(), false));
            var newItem = viewModel.DataNames.Last();
            viewModel.SelectItem(newItem.Instance);
            viewModel.BeginEdit();

            return newItem.Instance;
        }

        public static void DownloadSelected(this DataNamesViewModel viewModel)
        {
            viewModel.DownloadCommand.Execute(null);
        }

        public static void DeleteSelected(this DataNamesViewModel viewModel)
        {
            viewModel.DeleteCommand?.Execute(null);
        }
    }
}
