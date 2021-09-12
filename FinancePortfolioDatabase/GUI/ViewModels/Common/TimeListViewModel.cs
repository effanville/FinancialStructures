﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Structure.DataStructures;
using Common.UI;
using Common.UI.Commands;
using Common.UI.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    /// <summary>
    /// View model for displaying a <see cref="TimeList"/>
    /// </summary>
    public sealed class TimeListViewModel : PropertyChangedBase
    {
        private readonly UiGlobals fUiGlobals;
        private TimeList fDisplayList;
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
        public TimeListViewModel(TimeList timeList, string valueName, UiGlobals globals, Action<DailyValuation> deleteValueAction, Action<DailyValuation, DailyValuation> addEditValueAction)
        {
            DeleteValueAction = deleteValueAction;
            AddEditValueAction = addEditValueAction;
            fDisplayList = timeList;
            fUiGlobals = globals;
            ValueName = valueName;
            PreEditCommand = new RelayCommand(ExecutePreEdit);
            AddEditDataCommand = new RelayCommand(ExecuteAddEditData);
            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            AddDefaultDataCommand = new RelayCommand<AddingNewItemEventArgs>(e => DataGrid_AddingNewItem(null, e));

            DeleteValuationCommand = new RelayCommand<KeyEventArgs>(ExecuteDeleteValuation);
        }

        /// <summary>
        /// Routine to update the data in the display.
        /// </summary>
        public void UpdateData(TimeList timeList)
        {
            fDisplayList = timeList;
            Valuations = null;
            Valuations = timeList.Values();
        }

        /// <summary>
        /// Command called to add default values.
        /// </summary>
        public ICommand AddDefaultDataCommand
        {
            get;
            set;
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            DailyValuation latest = null;
            if (Valuations != null && Valuations.Any())
            {
                latest = Valuations.Last();
            }
            e.NewItem = new DailyValuation()
            {
                Day = DateTime.Today,
                Value = latest?.Value ?? 0.0
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
        public ICommand DeleteValuationCommand
        {
            get;
            set;
        }

        private void ExecuteDeleteValuation(KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (SelectedValuation != null)
                {
                    DeleteValueAction(SelectedValuation);
                }
            }
        }
    }
}
