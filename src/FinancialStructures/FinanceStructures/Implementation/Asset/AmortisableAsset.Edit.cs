using System;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation.Asset
{
    /// <summary>
    /// An implementation of an asset that can have a debt against it.
    /// </summary>
    public sealed partial class AmortisableAsset
    {
        /// <inheritdoc/>
        public void SetDebt(DateTime date, decimal value)
        {
            Debt.SetData(date, value);
        }

        /// <inheritdoc/>
        public bool TryDeleteDebt(DateTime date)
        {
            return Debt.TryDeleteValue(date);
        }

        /// <inheritdoc/>
        public bool TryEditDebt(DateTime oldDate, DateTime date, decimal value)
        {
            if (Debt.ValueExists(oldDate, out _))
            {
                return Debt.TryEditData(oldDate, date, value);
            }

            Debt.SetData(date, value);
            return true;
        }

        /// <inheritdoc/>
        public void SetPayment(DateTime date, decimal value)
        {
            Payments.SetData(date, value);
        }

        /// <inheritdoc/>
        public bool TryDeletePayment(DateTime date)
        {
            return Payments.TryDeleteValue(date);
        }

        /// <inheritdoc/>
        public bool TryEditPayment(DateTime oldDate, DateTime date, decimal value)
        {
            if (Payments.ValueExists(oldDate, out _))
            {
                return Payments.TryEditData(oldDate, date, value);
            }

            Payments.SetData(date, value);
            return true;
        }
    }
}
