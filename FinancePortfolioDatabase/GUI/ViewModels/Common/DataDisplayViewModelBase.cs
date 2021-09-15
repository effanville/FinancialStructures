using Common.UI.ViewModelBases;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancialStructures.Database;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    /// <summary>
    /// Wraps a base view model with a account type record.
    /// </summary>
    public abstract class DataDisplayViewModelBase : ViewModelBase<IPortfolio>
    {
        private UiStyles fStyles;

        /// <summary>
        /// The style object containing the style for the ui.
        /// </summary>
        public UiStyles Styles
        {
            get => fStyles;
            set => SetAndNotify(ref fStyles, value, nameof(Styles));
        }

        /// <summary>
        /// Whether the display can be closed or not.
        /// </summary>
        public virtual bool Closable => false;

        /// <summary>
        /// The Account type the view model stores data pertaining to.
        /// </summary>
        public Account DataType
        {
            get;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataDisplayViewModelBase(UiStyles styles, string header, Account dataType, IPortfolio database)
            : base(header, database)
        {
            Styles = styles;
            DataType = dataType;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataDisplayViewModelBase(UiStyles styles, string header, IPortfolio database)
            : base(header, database)
        {
            Styles = styles;
            DataType = Account.All;
        }
    }
}
