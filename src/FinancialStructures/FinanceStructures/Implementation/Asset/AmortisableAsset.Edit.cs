using System;
using Effanville.Common.Structure.DataEdit;
using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation.Asset
{
    /// <summary>
    /// An implementation of an asset that can have a debt against it.
    /// </summary>
    public sealed partial class AmortisableAsset
    {
        /// <inheritdoc/>
        public UpdateResult<DailyValuation> SetDebt(DateTime date, decimal value)
            => Debt.SetData(date, value);

        /// <inheritdoc/>
        public UpdateResult<DailyValuation> TryDeleteDebt(DateTime date)
            => Debt.TryDeleteValue(date);

        /// <inheritdoc/>
        public UpdateResult<DailyValuation> TryEditDebt(DateTime oldDate, DateTime date, decimal value)
        {
            if (Debt.ValueExists(oldDate, out _))
            {
                return Debt.TryEditData(oldDate, date, value);
            }

            return Debt.SetData(date, value);
        }

        /// <inheritdoc/>
        public UpdateResult<DailyValuation> SetPayment(DateTime date, decimal value)
            => Payments.SetData(date, value);

        /// <inheritdoc/>
        public UpdateResult<DailyValuation> TryDeletePayment(DateTime date)
            => Payments.TryDeleteValue(date);

        /// <inheritdoc/>
        public UpdateResult<DailyValuation> TryEditPayment(DateTime oldDate, DateTime date, decimal value)
        {
            if (Payments.ValueExists(oldDate, out _))
            {
                return Payments.TryEditData(oldDate, date, value);
            }

            return Payments.SetData(date, value);
        }
    }
}
