using System;
using System.Collections.Generic;
using System.IO;
using FinancialStructures.DataStructures;
using Common.Structure.DataStructures;
using Common.Structure.Reporting;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// Contains data editing of a security class.
    /// </summary>
    public partial class Security
    {
        /// <inheritdoc/>
        public override bool TryEditData(DateTime oldDate, DateTime date, double unitPrice, IReportLogger reportLogger = null)
        {
            return AddOrEditUnitPriceData(oldDate, date, unitPrice, reportLogger);
        }

        /// <inheritdoc/>
        public override void SetData(DateTime date, double unitPrice, IReportLogger reportLogger = null)
        {
            _ = AddOrEditUnitPriceData(date, date, unitPrice, reportLogger);
        }

        /// <inheritdoc/>
        public bool TryAddOrEditData(DateTime oldDate, DateTime date, double unitPrice, double shares, double investment = 0, IReportLogger reportLogger = null)
        {
            bool editUnitPrice = AddOrEditUnitPriceData(oldDate, date, unitPrice, reportLogger);
            bool editShares = AddOrEditSharesData(oldDate, date, shares, reportLogger);
            bool editInvestments = AddOrEditInvestmentData(oldDate, date, investment, reportLogger);

            return editUnitPrice & editShares & editInvestments && ComputeInvestments(reportLogger);
        }

        private bool AddOrEditUnitPriceData(DateTime oldDate, DateTime date, double shares, IReportLogger reportLogger = null)
        {
            if (DoesDateUnitPriceDataExist(oldDate, out int _))
            {
                return fUnitPrice.TryEditData(oldDate, date, shares, reportLogger);
            }

            fUnitPrice.SetData(date, shares, reportLogger);
            return true;
        }

        private bool AddOrEditSharesData(DateTime oldDate, DateTime date, double shares, IReportLogger reportLogger = null)
        {
            if (DoesDateSharesDataExist(oldDate, out int _))
            {
                return fShares.TryEditData(oldDate, date, shares, reportLogger);
            }

            fShares.SetData(date, shares, reportLogger);
            return true;
        }

        private bool AddOrEditInvestmentData(DateTime oldDate, DateTime date, double shares, IReportLogger reportLogger = null)
        {
            if (DoesDateInvestmentDataExist(oldDate, out int _))
            {
                return fInvestments.TryEditData(oldDate, date, shares, reportLogger);
            }

            fInvestments.SetData(date, shares, reportLogger);
            return true;
        }

        /// <inheritdoc/>
        public override List<object> CreateDataFromCsv(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            List<object> dailyValuations = new List<object>();
            foreach (string[] dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 4)
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Line in Csv file has incomplete data.");
                    break;
                }

                SecurityDayData line = new SecurityDayData(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]), double.Parse(dayValuation[2]), double.Parse(dayValuation[3]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        /// <inheritdoc/>
        public override void WriteDataToCsv(TextWriter writer, IReportLogger reportLogger)
        {
            foreach (SecurityDayData value in GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        /// <summary>
        /// Tries to delete the data. If it can, it deletes all data specified, then returns true only if all data has been successfully deleted.
        /// </summary>
        public override bool TryDeleteData(DateTime date, IReportLogger reportLogger = null)
        {
            return fUnitPrice.TryDeleteValue(date, reportLogger)
                & fShares.TryDeleteValue(date, reportLogger)
                & fInvestments.TryDeleteValue(date, reportLogger)
                && ComputeInvestments(reportLogger);
        }

        /// <summary>
        /// Removes unnecessary investment and Share number values to reduce size.
        /// </summary>
        public void CleanData()
        {
            fShares.CleanValues();
            fInvestments.CleanValues();
        }

        /// <summary>
        /// Upon a new/edit investment, one needs to recompute the values of the investments
        /// One should not change Inv = 0 or Inv > 0  to ensure that dividend reivestments are not accidentally included in a new investment.
        /// This though causes a problem if a value is deleted.
        /// </summary>
        /// <remarks>
        /// This should be called throughout, whenever one updates the data stored in the Security.
        /// </remarks>
        private bool ComputeInvestments(IReportLogger reportLogger = null)
        {
            CleanData();

            for (int index = 0; index < fInvestments.Count(); index++)
            {
                DailyValuation investmentValue = fInvestments[index];
                if (investmentValue.Value != 0)
                {
                    DailyValuation sharesCurrentValue = fShares.NearestEarlierValue(investmentValue.Day);
                    DailyValuation sharesPreviousValue = fShares.RecentPreviousValue(investmentValue.Day) ?? new DailyValuation(DateTime.Today, 0);
                    if (sharesCurrentValue != null)
                    {
                        fInvestments.SetData(investmentValue.Day, (sharesCurrentValue.Value - sharesPreviousValue.Value) * fUnitPrice.NearestEarlierValue(investmentValue.Day).Value, reportLogger);
                    }
                }
                if (investmentValue.Value == 0)
                {
                    if (fInvestments.TryDeleteValue(investmentValue.Day, reportLogger))
                    {
                        index--;
                    }
                }
            }

            return true;
        }
    }
}
