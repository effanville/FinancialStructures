﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using StructureCommon.DisplayClasses;
using FinancialStructures.FinanceStructures;

namespace FinancePortfolioDatabase.Tests.CommonWindowTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class DataNamesViewModelTests
    {
        [Test]
        public void CanOpen()
        {
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);

            Portfolio newData = TestingGUICode.CreateBasicDataBase();

            viewModel.UpdateData(newData);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanCreateNew()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);
            NameData newName = new NameData("company", "name", "GBP", "someUrl", new HashSet<string>())
            {
                Company = "Company"
            };
            viewModel.PreEditSelectedName = newName;
            viewModel.DataNames.Add(new Selectable<NameData>(newName, false));
            viewModel.CreateCommand.Execute(viewModel.DataNames.Last());
            Assert.AreEqual(2, viewModel.DataNames.Count);
            Assert.AreEqual(2, portfolio.BankAccounts.Count);
        }

        [Test]
        [STAThread]
        public void CanEditName()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);
            viewModel.PreEditSelectedName = viewModel.DataNames[0].Instance.Copy();
            viewModel.DataNames[0].Instance.Company = "NewCompany";
            viewModel.CreateCommand.Execute(viewModel.DataNames[0]);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);

            Assert.AreEqual("NewCompany", portfolio.BankAccounts.Single().Names.Company);
        }

        [Test]
        //[Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount)
            {
                PreEditSelectedName = new NameData("Barclays", "currentAccount")
            };
            viewModel.DownloadCommand.Execute(1);

            Assert.AreEqual(1, viewModel.DataNames.Count);
            var account = portfolio.TryGetAccount(Account.BankAccount, new TwoName("Barclays", "currentAccount"), out var sec);
            Assert.AreEqual(2, sec.Values.Count());
        }

        [Test]
        public void CanDelete()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);

            Assert.AreEqual(1, viewModel.DataStore.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            viewModel.PreEditSelectedName = new NameData("Barclays", "currentAccount");
            viewModel.DeleteCommand.Execute(1);
            Assert.AreEqual(0, viewModel.DataStore.BankAccountsThreadSafe.Count);
            Assert.AreEqual(0, portfolio.BankAccounts.Count);
        }

        [Test]
        public void CanOpenSecurity()
        {
            Portfolio output = TestingGUICode.CreateBasicDataBase();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(output, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateSecurityData()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanCreateNewSecurity()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            NameData newName = new NameData("company", "name", "GBP", "someUrl", new HashSet<string>())
            {
                Company = "Company"
            };
            viewModel.PreEditSelectedName = newName;
            viewModel.DataNames.Add(new Selectable<NameData>(newName, false));
            viewModel.CreateCommand.Execute(viewModel.DataNames[1]);
            Assert.AreEqual(2, viewModel.DataNames.Count, "Bot enough in the view.");
            Assert.AreEqual(2, portfolio.Funds.Count, "Not enough in portfolio");
        }

        [Test]
        public void CanEditSecurityName()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            viewModel.PreEditSelectedName = viewModel.DataNames[0].Instance.Copy();
            viewModel.DataNames[0].Instance.Company = "NewCompany";
            viewModel.CreateCommand.Execute(viewModel.DataNames[0]);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);

            Assert.AreEqual("NewCompany", portfolio.Funds.Single().Names.Company);
        }

        [Test]
        public void CanEditSecurityNameAndUrl()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            viewModel.PreEditSelectedName = viewModel.DataNames[0].Instance.Copy();

            viewModel.DataNames[0].Instance.Company = "NewCompany";

            viewModel.DataNames[0].Instance.Url = "NewUrl";
            viewModel.CreateCommand.Execute(viewModel.DataNames[0]);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);

            Assert.AreEqual("NewCompany", portfolio.Funds.Single().Names.Company);
            Assert.AreEqual("NewUrl", portfolio.Funds.Single().Names.Url);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownloadSecurity()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security)
            {
                PreEditSelectedName = new NameData("Fidelity", "China")
            };
            viewModel.DownloadCommand.Execute(1);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }


        [Test]
        public void CanDeleteSecurity()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);

            Assert.AreEqual(1, viewModel.DataStore.FundsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);
            viewModel.PreEditSelectedName = new NameData("Fidelity", "China");
            viewModel.DeleteCommand.Execute(1);
            Assert.AreEqual(0, viewModel.DataStore.FundsThreadSafe.Count);
            Assert.AreEqual(0, portfolio.Funds.Count);
        }
    }
}
