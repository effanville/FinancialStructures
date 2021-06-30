using System;
using System.Windows;
using System.Windows.Threading;

namespace FinancePortfolioDatabase
{
    /// <summary>
    /// An implementation of <see cref="IDispatcher"/> using
    /// the <see cref="Application.Current.Dispatcher"/> to
    /// dispatch.
    /// </summary>
    public class DispatcherInstance : IDispatcher
    {
        Dispatcher fDispatcher;

        public DispatcherInstance()
        {
            fDispatcher = Application.Current.Dispatcher;
        }

        /// <inheritdoc/>
        public void Invoke(Action action)
        {
            fDispatcher.Invoke(action);
        }

        /// <inheritdoc/>
        public void BeginInvoke(Action action)
        {
            _ = fDispatcher.BeginInvoke(action);
        }
    }
}
