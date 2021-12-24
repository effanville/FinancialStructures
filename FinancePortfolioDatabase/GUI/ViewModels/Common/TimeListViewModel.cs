using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Common.Structure.DataStructures;
using Common.UI.Commands;
using Common.UI.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    /// <summary>
    /// View model for displaying a <see cref="TimeList"/>
    /// </summary>
    public sealed class TimeListViewModel : PropertyChangedBase
    {
        private readonly Action<DailyValuation> DeleteValueAction;
        private readonly Action<DailyValuation, DailyValuation> AddEditValueAction;

        internal DailyValuation fOldSelectedValuation;
        internal DailyValuation SelectedValuation;

        private List<DailyValuation> fValuations = new List<DailyValuation>();

        /// <summary>
        /// The list of values to display.
        /// </summary>
        public List<DailyValuation> Valuations
        {
            get => fValuations;
            set => SetAndNotify(ref fValuations, value, nameof(Valuations));
        }

        private string fValueName;

        /// <summary>
        /// The name of the type of value displayed.
        /// </summary>
        public string ValueName
        {
            get => fValueName;
            set => SetAndNotify(ref fValueName, value, nameof(ValueName));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimeListViewModel(TimeList timeList, string valueName, Action<DailyValuation> deleteValueAction, Action<DailyValuation, DailyValuation> addEditValueAction)
        {
            DeleteValueAction = deleteValueAction;
            AddEditValueAction = addEditValueAction;
            ValueName = valueName;
            PreEditCommand = new RelayCommand(ExecutePreEdit);
            AddEditDataCommand = new RelayCommand(ExecuteAddEditData);
            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            UpdateData(timeList);
        }

        /// <summary>
        /// Routine to update the data in the display.
        /// </summary>
        public void UpdateData(TimeList timeList)
        {
            Valuations = null;
            Valuations = timeList?.Values() ?? new List<DailyValuation>();
        }

        internal DailyValuation DefaultNewItem()
        {
            DailyValuation latest = null;
            if (Valuations != null && Valuations.Any())
            {
                latest = Valuations.Last();
            }

            return new DailyValuation()
            {
                Day = DateTime.Today,
                Value = latest?.Value ?? 0.0m
            };
        }

        /// <summary>
        /// Command to update the selected item.
        /// </summary>
        public ICommand SelectionChangedCommand
        {
            get;
            set;
        }
        private void ExecuteSelectionChanged(object obj)
        {
            if (Valuations != null && obj is DailyValuation data)
            {
                SelectedValuation = data;
            }
        }

        /// <summary>
        /// Called prior to an edit occurring in a row. This is used
        /// to record the state of the row before editing.
        /// </summary>
        public ICommand PreEditCommand
        {
            get;
            set;
        }

        private void ExecutePreEdit()
        {
            fOldSelectedValuation = SelectedValuation?.Copy();
        }

        /// <summary>
        /// Command to add or edit data to the <see cref="TimeList"/>
        /// </summary>
        public ICommand AddEditDataCommand
        {
            get;
            set;
        }

        private void ExecuteAddEditData()
        {
            if (SelectedValuation != null)
            {
                AddEditValueAction(fOldSelectedValuation, SelectedValuation);
            }
        }

        /// <summary>
        /// Command to delete values from the <see cref="TimeList"/>
        /// </summary>
        internal void DeleteValuation()
        {
            if (SelectedValuation != null)
            {
                DeleteValueAction(SelectedValuation);
            }
        }
    }
}
