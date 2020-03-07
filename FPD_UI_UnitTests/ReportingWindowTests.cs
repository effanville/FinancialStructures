using NUnit.Framework;
using FinanceWindowsViewModels;
using FinancialStructures.Reporting;
using System.Linq;

namespace FPD_UI_UnitTests
{
    public class ReportingWindowTests
    {
        [Test]
        public void ReportsSync()
        {
            var viewModel = new ReportingWindowViewModel();
            viewModel.Reports.AddReport("Error", "Unknown", "Is this added?");
            viewModel.SyncReports();
            
            Assert.AreEqual(1, viewModel.ReportsToView.Count, "Viewable reports should have one report added.");
            Assert.AreEqual(ReportType.Error, viewModel.ReportsToView.Single().ErrorType);
            Assert.AreEqual(Location.Unknown, viewModel.ReportsToView.Single().ErrorLocation);
            Assert.AreEqual("Is this added?", viewModel.ReportsToView.Single().Message);
        }

        [Test]
        public void CanAddReport()
        {
            var viewModel = new ReportingWindowViewModel();
            viewModel.UpdateReport("Error", "Unknown", "Is this added?");

            Assert.AreEqual(1, viewModel.Reports.GetReports().Count, "Reports should have a report added.");
            Assert.AreEqual(1, viewModel.ReportsToView.Count, "Viewable reports should have one report added.");
            Assert.AreEqual(ReportType.Error, viewModel.ReportsToView.Single().ErrorType);
            Assert.AreEqual(Location.Unknown, viewModel.ReportsToView.Single().ErrorLocation);
            Assert.AreEqual("Is this added?", viewModel.ReportsToView.Single().Message);
        }

        [Test]
        public void CanClearReports()
        {
            var viewModel = new ReportingWindowViewModel();
            viewModel.UpdateReport("Error", "Unknown", "Is this added?");
            viewModel.UpdateReport("Error", "Unknown", "Is this also added?");

            Assert.AreEqual(2, viewModel.Reports.GetReports().Count, "Reports should have a report added.");
            Assert.AreEqual(2, viewModel.ReportsToView.Count, "Viewable reports should have one report added.");

            viewModel.ClearReportsCommand.Execute(3);
            Assert.AreEqual(0, viewModel.Reports.GetReports().Count, "Reports should have been cleared.");
            Assert.AreEqual(0, viewModel.ReportsToView.Count, "Viewable reports should have been cleared.");
        }

        [Test]
        public void CanClearSingleReport()
        {
            var viewModel = new ReportingWindowViewModel();
            viewModel.UpdateReport("Error", "Unknown", "Is this added?");
            viewModel.UpdateReport("Error", "Unknown", "Is this also added?");

            Assert.AreEqual(2, viewModel.Reports.GetReports().Count, "Reports should have a report added.");
            Assert.AreEqual(2, viewModel.ReportsToView.Count, "Viewable reports should have one report added.");

            viewModel.IndexToDelete = 1;
            viewModel.ClearSingleReportCommand.Execute(3);
            Assert.AreEqual(1, viewModel.Reports.GetReports().Count, "Reports should have been cleared.");
            Assert.AreEqual(1, viewModel.ReportsToView.Count, "Viewable reports should have been cleared.");
            Assert.AreEqual(ReportType.Error, viewModel.ReportsToView.Single().ErrorType);
            Assert.AreEqual(Location.Unknown, viewModel.ReportsToView.Single().ErrorLocation);
            Assert.AreEqual("Is this added?", viewModel.ReportsToView.Single().Message);
        }
    }
}