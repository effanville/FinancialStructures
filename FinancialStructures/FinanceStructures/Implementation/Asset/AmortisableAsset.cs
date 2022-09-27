using System;
using Common.Structure.DataStructures;

using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures.Implementation.Asset
{
    /// <summary>
    /// An implementation of an asset that can have a debt against it.
    /// </summary>
    public sealed partial class AmortisableAsset : ValueList, IAmortisableAsset
    {
        /// <inheritdoc/>
        public TimeList Debt { get; set; } = new TimeList();

        /// <inheritdoc/>
        public TimeList Payments { get; set; } = new TimeList();

        /// <summary>
        /// Empty constructor.
        /// </summary>
        internal AmortisableAsset()
            : base()
        {
        }

        internal AmortisableAsset(NameData name)
            : base(name)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        private AmortisableAsset(NameData names, TimeList values, TimeList debt, TimeList payments)
            : base(names, values)
        {
            Debt = debt;
            Payments = payments;
        }

        /// <inheritdoc/>
        protected override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.Asset));
        }

        /// <summary>
        /// Ensures that events for data edit are subscribed to.
        /// </summary>
        public override void SetupEventListening()
        {
            base.SetupEventListening();
            Debt.DataEdit += OnDataEdit;
        }

        /// <inheritdoc/>
        public override IValueList Copy()
        {
            return new AmortisableAsset(Names.Copy(), Values.Copy(), Debt.Copy(), Payments.Copy());
        }
    }
}
