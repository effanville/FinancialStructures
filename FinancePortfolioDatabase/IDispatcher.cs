using System;

namespace FinancePortfolioDatabase
{
    /// <summary>
    /// A thin interface around the Threading.Dispatcher class
    /// to abstract dependencies.
    /// </summary>
    public interface IDispatcher
    {
        void Invoke(Action action);
        void BeginInvoke(Action action);
    }
}
