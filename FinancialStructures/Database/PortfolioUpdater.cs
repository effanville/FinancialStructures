using System;
using Common.Structure.DataStructures;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Implementation of a <see cref="IPortfolioUpdater"/> that performs the action asyncronously.
    /// </summary>
    public sealed class BackgroundPortfolioUpdater : IPortfolioUpdater
    {
        private TaskQueue _taskQueue = new TaskQueue();

        /// <inheritdoc/>
        public void PerformPortfolioAction(Action<IPortfolio> action, IPortfolio portfolio)
        {
            _taskQueue.Enqueue(action, portfolio);
        }
    }

    /// <summary>
    /// Implementation of a <see cref="IPortfolioUpdater"/> that performs the action syncronously.
    /// </summary>
    public sealed class SynchronousPortfolioUpdater : IPortfolioUpdater
    {
        /// <inheritdoc/>
        public void PerformPortfolioAction(Action<IPortfolio> action, IPortfolio portfolio)
        {
            action(portfolio);
        }
    }

    /// <summary>
    /// Contains methods for updating a portfolio.
    /// </summary>
    public interface IPortfolioUpdater
    {
        /// <summary>
        /// Update the portfolio with the given action.
        /// </summary>
        void PerformPortfolioAction(Action<IPortfolio> action, IPortfolio portfolio);
    }
}
