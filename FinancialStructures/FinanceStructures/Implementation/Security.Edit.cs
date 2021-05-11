using System;
using System.Collections.Generic;
using System.IO;
using FinancialStructures.DataStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// Contains data editing of a security class.
    /// </summary>
    public partial class Security
    {
        /// <inheritdoc/>
        public override bool TryAddOrEditData(DateTime oldDate, DateTime date, double unitPrice, IReportLogger reportLogger = null)
        {
            double shares = Shares.Value(date)?.Value ?? 0.0;
            double investment = Investments.Value(date)?.Value ?? 0.0;
            return TryAddOrEditData(oldDate, date, unitPrice, shares, investment, reportLogger: reportLogger);
        }

        /// <inheritdoc/>
        public bool TryAddOrEditData(DateTime oldDate, DateTime date, double unitPrice, double shares = 0, double investment = 0, IReportLogger reportLogger = null)
        {
            if (DoesDateSharesDataExist(oldDate, out int _) || DoesDateInvestmentDataExist(oldDate, out int _) || DoesDateUnitPriceDataExist(oldDate, out int _))
            {
                _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Security {Names} data on {date.ToString("d")} edited.");
                return TryEditData(oldDate, date, shares, unitPrice, investment, reportLogger);
            }

            return fShares.TryAddValue(date, shares, reportLogger) & fUnitPrice.TryAddValue(date, unitPrice, reportLogger) & fInvestments.TryAddValue(date, investment, reportLogger) && ComputeInvestments(reportLogger);
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
        /// Trys to get latest data earlier than date requested. Only returns true if all data present.
        /// </summary>
        internal bool TryGetEarlierData(DateTime date, out DailyValuation price, out DailyValuation units, out DailyValuation investment)
        {
            return fUnitPrice.TryGetNearestEarlierValue(date, out price)
                & fShares.TryGetNearestEarlierValue(date, out units)
                & fInvestments.TryGetNearestEarlierValue(date, out investment);
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
        /// Try to edit data. If any dont have any relevant values, then do not edit
        /// If do have relevant values, then edit that value
        /// If investment value doesnt exist, then add that value.
        /// </summary>
        private bool TryEditData(DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null)
        {
            bool editShares = false;
            bool editUnitPrice = false;
            if (DoesDateSharesDataExist(oldDate, out int _))
            {
                editShares = fShares.TryEditData(oldDate, newDate, shares, reportLogger);
            }

            if (DoesDateUnitPriceDataExist(oldDate, out int _))
            {
                editUnitPrice = fUnitPrice.TryEditData(oldDate, newDate, unitPrice, reportLogger);
            }

            fInvestments.TryEditDataOtherwiseAdd(oldDate, newDate, Investment, reportLogger);

            return editShares & editUnitPrice && ComputeInvestments(reportLogger);
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
                        _ = fInvestments.TryEditData(investmentValue.Day, (sharesCurrentValue.Value - sharesPreviousValue.Value) * fUnitPrice.NearestEarlierValue(investmentValue.Day).Value, reportLogger);
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
