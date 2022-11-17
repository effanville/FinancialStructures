using System;
using System.Threading.Tasks;

namespace FinancialStructures.Database
{
    public sealed class BackgroundPortfolioUpdater : IPortfolioUpdater
    {
        public void PerformPortfolioAction(Action<IPortfolio> action, IPortfolio portfolio)
        {
            new Task(() => action(portfolio)).Start();
        }
    }

    public sealed class SynchronousPortfolioUpdater : IPortfolioUpdater
    {
        public void PerformPortfolioAction(Action<IPortfolio> action, IPortfolio portfolio)
        {
            action(portfolio);
        }
    }

    public interface IPortfolioUpdater
    {
        void PerformPortfolioAction(Action<IPortfolio> action, IPortfolio portfolio);
    }
}
