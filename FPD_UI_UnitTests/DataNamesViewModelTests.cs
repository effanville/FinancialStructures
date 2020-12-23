using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using FPD_UI_UnitTests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;

namespace FPD_UI_UnitTests.CommonWindowTests
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
            NameCompDate newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today)
            {
                Company = "Company"
            };
            viewModel.fPreEditSelectedName = newName;
            viewModel.DataNames.Add(newName);
            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames.Last());
            viewModel.CreateCommand.Execute(dataGridArgs);
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
            viewModel.fPreEditSelectedName = viewModel.DataNames[0].Copy();
            viewModel.DataNames[0].Company = "NewCompany";
            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames[0]);
            viewModel.CreateCommand.Execute(dataGridArgs);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);

            Assert.AreEqual("NewCompany", portfolio.BankAccounts.Single().Company);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount)
            {
                fPreEditSelectedName = new NameCompDate("Barclays", "currentAccount")
            };
            viewModel.DownloadCommand.Execute(1);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanDelete()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);

            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            viewModel.fPreEditSelectedName = new NameCompDate("Barclays", "currentAccount");
            viewModel.DeleteCommand.Execute(1);
            Assert.AreEqual(0, viewModel.DataStore.BankAccounts.Count);
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
            NameCompDate newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today)
            {
                Company = "Company"
            };
            viewModel.fPreEditSelectedName = newName;
            viewModel.DataNames.Add(newName);
            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames[1]);
            viewModel.CreateCommand.Execute(dataGridArgs);
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
            viewModel.fPreEditSelectedName = viewModel.DataNames[0].Copy();

            viewModel.DataNames[0].Company = "NewCompany";
            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames[0]);
            viewModel.CreateCommand.Execute(dataGridArgs);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);

            Assert.AreEqual("NewCompany", portfolio.Funds.Single().Company);
        }

        [Test]
        public void CanEditSecurityNameAndUrl()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            viewModel.fPreEditSelectedName = viewModel.DataNames[0].Copy();

            viewModel.DataNames[0].Company = "NewCompany";
            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames[0]);

            viewModel.DataNames[0].Url = "NewUrl";
            dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames[0]);
            viewModel.CreateCommand.Execute(dataGridArgs);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);

            Assert.AreEqual("NewCompany", portfolio.Funds.Single().Company);
            Assert.AreEqual("NewUrl", portfolio.Funds.Single().Url);
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
                fPreEditSelectedName = new NameCompDate("Fidelity", "China")
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

            Assert.AreEqual(1, viewModel.DataStore.Funds.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);
            viewModel.fPreEditSelectedName = new NameCompDate("Fidelity", "China");
            viewModel.DeleteCommand.Execute(1);
            Assert.AreEqual(0, viewModel.DataStore.Funds.Count);
            Assert.AreEqual(0, portfolio.Funds.Count);
        }
    }
}
