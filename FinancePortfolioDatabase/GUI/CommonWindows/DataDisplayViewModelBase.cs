using FinancialStructures.FinanceInterfaces;
using UICommon.ViewModelBases;

namespace FinanceCommonViewModels
{
    /// <summary>
    /// Wraps a base view model with a account type record.
    /// </summary>
    public abstract class DataDisplayViewModelBase : ViewModelBase<IPortfolio>
    {
        /// <summary>
        /// The Account type the view model stores data pertaining to.
        /// </summary>
        public Account DataType
        {
            get;
        }

        public DataDisplayViewModelBase(string header, Account dataType, IPortfolio database)
            : base(header, database)
        {
            DataType = dataType;
        }
    }
}
