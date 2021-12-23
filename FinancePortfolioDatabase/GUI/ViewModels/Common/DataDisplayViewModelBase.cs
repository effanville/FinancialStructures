using Common.Structure.Reporting;
using Common.UI;
using Common.UI.ViewModelBases;
using FinancePortfolioDatabase.GUI.Configuration;
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
        /// The globals for this view model.
        /// </summary>
        protected readonly UiGlobals fUiGlobals;

        /// <summary>
        /// The user configuration for this view model.
        /// </summary>
        protected IConfiguration fUserConfiguration;

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
        /// The logging mechanism.
        /// </summary>
        public IReportLogger ReportLogger => fUiGlobals.ReportLogger;

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
        public DataDisplayViewModelBase(UiGlobals globals, UiStyles styles, IConfiguration config, IPortfolio database, string header, Account dataType)
            : base(header, database)
        {
            fUiGlobals = globals;
            Styles = styles;
            fUserConfiguration = config;
            DataType = dataType;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataDisplayViewModelBase(UiGlobals globals, UiStyles styles, IConfiguration config, IPortfolio database, string header)
            : base(header, database)
        {
            fUiGlobals = globals;
            Styles = styles;
            fUserConfiguration = config;
            DataType = Account.All;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataDisplayViewModelBase(UiGlobals globals, UiStyles styles, IPortfolio database, string header, Account dataType)
            : base(header, database)
        {
            fUiGlobals = globals;
            Styles = styles;
            DataType = dataType;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataDisplayViewModelBase(UiGlobals globals, UiStyles styles, IPortfolio database, string header)
            : base(header, database)
        {
            fUiGlobals = globals;
            Styles = styles;
            DataType = Account.All;
        }
    }
}
