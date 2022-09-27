using System;

using Common.Structure.Reporting;

namespace FinancialStructures.FinanceStructures.Implementation.Asset
{
    /// <summary>
    /// An implementation of an asset that can have a debt against it.
    /// </summary>
    public sealed partial class AmortisableAsset : ValueList, IAmortisableAsset
    {
        /// <inheritdoc/>
        public void SetDebt(DateTime date, decimal value, IReportLogger reportLogger = null)
        {
            Debt.SetData(date, value, reportLogger);
        }

        /// <inheritdoc/>
        public bool TryDeleteDebt(DateTime date, IReportLogger reportLogger = null)
        {
            return Debt.TryDeleteValue(date, reportLogger);
        }

        /// <inheritdoc/>
        public bool TryEditDebt(DateTime oldDate, DateTime date, decimal value, IReportLogger reportLogger = null)
        {
            if (Debt.ValueExists(oldDate, out _))
            {
                return Debt.TryEditData(oldDate, date, value, reportLogger);
            }

            Debt.SetData(date, value, reportLogger);
            return true;
        }

        /// <inheritdoc/>
        public void SetPayment(DateTime date, decimal value, IReportLogger reportLogger = null)
        {
            Payments.SetData(date, value, reportLogger);
        }

        /// <inheritdoc/>
        public bool TryDeletePayment(DateTime date, IReportLogger reportLogger = null)
        {
            return Payments.TryDeleteValue(date, reportLogger);
        }

        /// <inheritdoc/>
        public bool TryEditPayment(DateTime oldDate, DateTime date, decimal value, IReportLogger reportLogger = null)
        {
            if (Payments.ValueExists(oldDate, out _))
            {
                return Payments.TryEditData(oldDate, date, value, reportLogger);
            }

            Payments.SetData(date, value, reportLogger);
            return true;
        }
    }
}
