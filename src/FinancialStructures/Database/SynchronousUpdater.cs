using System;

using Effanville.Common.Structure.DataEdit;

namespace Effanville.FinancialStructures.Database
{
    /// <summary>
    /// Implementation of a <see cref="IUpdater{IPortfolio}"/> that performs the action syncronously.
    /// </summary>
    public sealed class SynchronousUpdater<T> : IUpdater<T> where T : class
    {
        /// <inheritdoc/>
        public T Database
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public void PerformUpdateAction(Action<T> action, T portfolio)
        {
            action(portfolio);
        }

        /// <inheritdoc/>
        public void PerformUpdate(object obj, UpdateRequestArgs<T> requestArgs)
        {
            if (!requestArgs.IsHandled)
            {
                requestArgs.UpdateAction(Database);
                requestArgs.IsHandled = true;
            }
        }
    }
}
