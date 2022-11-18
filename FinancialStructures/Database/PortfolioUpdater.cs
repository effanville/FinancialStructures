using System;
using System.Threading.Tasks;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Implementation of a <see cref="IPortfolioUpdater"/> that performs the action asyncronously.
    /// </summary>
    public sealed class BackgroundPortfolioUpdater : IPortfolioUpdater
    {
        /// <inheritdoc/>
        public void PerformPortfolioAction(Action<IPortfolio> action, IPortfolio portfolio)
        {
            new Task(() => action(portfolio)).Start();
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
