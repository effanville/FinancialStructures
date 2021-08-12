using System;
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
    public sealed class TimeListViewModel : PropertyChangedBase
    {
        private readonly UiGlobals fUiGlobals;
        private TimeList fDisplayList;
        private readonly Action<DailyValuation> DeleteValueAction;
        private readonly Action<DailyValuation, DailyValuation> AddEditValueAction;

        internal DailyValuation fOldSelectedValuation;
        internal DailyValuation SelectedValuation;

        private List<DailyValuation> fUnitPrices = new List<DailyValuation>();
        public List<DailyValuation> Valuations
        {
            get
            {
                return fUnitPrices;
            }
            set
            {
                SetAndNotify(ref fUnitPrices, value, nameof(Valuations));
            }
        }

        public TimeListViewModel(TimeList timeList, UiGlobals globals, Action<DailyValuation> deleteValueAction, Action<DailyValuation, DailyValuation> addEditValueAction)
        {
            DeleteValueAction = deleteValueAction;
            AddEditValueAction = addEditValueAction;
            fDisplayList = timeList;
            fUiGlobals = globals;
            PreEditCommand = new RelayCommand(ExecutePreEdit);
            AddEditDataCommand = new RelayCommand(ExecuteAddEditData);
            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            AddDefaultDataCommand = new RelayCommand<AddingNewItemEventArgs>(e => DataGrid_AddingNewItem(null, e));

            DeleteValuationCommand = new RelayCommand<KeyEventArgs>(ExecuteDeleteValuation);
        }

        public void UpdateData(TimeList timeList)
        {
            fDisplayList = timeList;
            Valuations = null;
            Valuations = timeList.Values();
        }

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
