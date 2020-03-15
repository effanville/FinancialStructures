﻿using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportLogging;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceCommonViewModels
{
    internal class DataNamesViewModel : ViewModelBase
    {
        private Portfolio Portfolio;

        private List<NameCompDate> fPreEditNames = new List<NameCompDate>();

        private List<NameCompDate> fDataNames = new List<NameCompDate>();

        /// <summary>
        /// Name and Company data of Funds in database for view.
        /// </summary>
        public List<NameCompDate> DataNames
        {
            get { return fDataNames; }
            set { fDataNames = value; OnPropertyChanged(); }
        }

        private NameCompDate fSelectedName;

        /// <summary>
        /// Name and Company data of the selected security in the list <see cref="DataNames"/>
        /// </summary>
        public NameCompDate SelectedName
        {
            get { return fSelectedName; }
            set { fSelectedName = value; OnPropertyChanged(); }
        }

        private readonly Action<Action<Portfolio>> UpdateDataCallback;
        private readonly LogReporter ReportLogger;
        private readonly EditMethods editMethods;

        public DataNamesViewModel(Portfolio portfolio, Action<Action<Portfolio>> updateDataCallback, LogReporter reportLogger, Action<NameData> loadSelectedData, EditMethods updateMethods)
            : base("Accounts", loadSelectedData)
        {
            Portfolio = portfolio;
            UpdateDataCallback = updateDataCallback;
            ReportLogger = reportLogger;
            editMethods = updateMethods;

            CreateCommand = new BasicCommand(ExecuteCreateEdit);
            DeleteCommand = new BasicCommand(ExecuteDelete);
            DownloadCommand = new BasicCommand(ExecuteDownloadCommand);
        }

        public override void UpdateData(Portfolio portfolio, Action<object> removeTab)
        {
            Portfolio = portfolio;
            var currentSelectedName = SelectedName;
            DataNames = (List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio).Result;
            DataNames.Sort();
            fPreEditNames = (List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, portfolio).Result;
            fPreEditNames.Sort();

            for (int i = 0; i < DataNames.Count; i++)
            {
                if (DataNames[i].CompareTo(currentSelectedName) == 0)
                {
                    SelectedName = DataNames[i];
                    return;
                }
            }
        }

        public override void UpdateData(Portfolio portfolio)
        {
            UpdateData(portfolio, null);
        }

        public ICommand DownloadCommand { get; }
        private void ExecuteDownloadCommand(Object obj)
        {
            if (SelectedName != null)
            {
                UpdateDataCallback(async programPortfolio => await editMethods.ExecuteFunction(FunctionType.Download, programPortfolio, SelectedName, ReportLogger).ConfigureAwait(false));
            }
        }

        public ICommand CreateCommand { get; set; }
        private void ExecuteCreateEdit(Object obj)
        {
            if (((List<NameCompDate>)editMethods.ExecuteFunction(FunctionType.NameUpdate, Portfolio).Result).Count != DataNames.Count)
            {
                bool edited = false;
                if (SelectedName.NewValue)
                {
                    UpdateDataCallback(programPortfolio => editMethods.ExecuteFunction(FunctionType.Create, programPortfolio, SelectedName, ReportLogger).Wait());
                    if (SelectedName != null)
                    {
                        SelectedName.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger.LogDetailed("Critical", "Error", "AddingData", "No Name provided on creation.");
                }
            }
            else
            {
                // maybe fired from editing stuff. Try that
                bool edited = false;
                for (int i = 0; i < DataNames.Count; i++)
                {
                    var name = DataNames[i];

                    if (name.NewValue && (!string.IsNullOrEmpty(name.Name) || !string.IsNullOrEmpty(name.Company)))
                    {
                        edited = true;
                        UpdateDataCallback(programPortfolio => editMethods.ExecuteFunction(FunctionType.Edit, programPortfolio, fPreEditNames[i], name, ReportLogger).Wait());
                        name.NewValue = false;
                    }
                }
                if (!edited)
                {
                    ReportLogger.LogDetailed("Critical", "Error", "EditingData", "Was not able to edit desired.");
                }
            }
        }

        public ICommand DeleteCommand { get; }
        private void ExecuteDelete(Object obj)
        {
            if (SelectedName.Name != null)
            {
                UpdateDataCallback(programPortfolio => editMethods.ExecuteFunction(FunctionType.Delete, programPortfolio, SelectedName, ReportLogger).Wait());
            }
            else
            {
                ReportLogger.LogDetailed("Critical", "Error", "DeletingData", "Nothing was selected when trying to delete.");
            }
        }
    }
}
