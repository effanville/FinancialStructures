using System;
using Common.Structure.DataStructures;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Implementation of a <see cref="IDatabaseUpdater{IPortfolio}"/> that performs the action asyncronously.
    /// </summary>
    public sealed class BackgroundPortfolioUpdater : IDatabaseUpdater<IPortfolio>
    {
        private TaskQueue _taskQueue = new TaskQueue();

        /// <inheritdoc/>
        public void PerformUpdateAction(Action<IPortfolio> action, IPortfolio portfolio)
        {
            _taskQueue.Enqueue(action, portfolio);
        }
    }

    /// <summary>
    /// Implementation of a <see cref="IDatabaseUpdater{IPortfolio}"/> that performs the action syncronously.
    /// </summary>
    public sealed class SynchronousPortfolioUpdater : IDatabaseUpdater<IPortfolio>
    {
        /// <inheritdoc/>
        public void PerformUpdateAction(Action<IPortfolio> action, IPortfolio portfolio)
        {
            action(portfolio);
        }
    }

    /// <summary>
    /// Contains methods for updating an object of type <see cref="T"/> using callbacks.
    /// </summary>
    public interface IDatabaseUpdater<T> where T : class
    {
        /// <summary>
        /// Update the portfolio with the given action.
        /// </summary>
        void PerformUpdateAction(Action<T> action, T portfolio);
    }
}
