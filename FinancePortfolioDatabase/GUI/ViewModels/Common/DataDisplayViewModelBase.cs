using FinancialStructures.Database;
using Common.UI.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    /// <summary>
    /// Wraps a base view model with a account type record.
    /// </summary>
    public abstract class DataDisplayViewModelBase : ViewModelBase<IPortfolio>
    {
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
        public DataDisplayViewModelBase(string header, Account dataType, IPortfolio database)
            : base(header, database)
        {
            DataType = dataType;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataDisplayViewModelBase(string header, IPortfolio database)
            : base(header, database)
        {
            DataType = Account.All;
        }
    }
}
