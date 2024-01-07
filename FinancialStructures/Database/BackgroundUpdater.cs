using System;
using Common.Structure.DataEdit;
using Common.Structure.DataStructures;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Implementation of a <see cref="IUpdater{T}"/> that performs the action asyncronously.
    /// </summary>
    public sealed class BackgroundUpdater<T> : IUpdater<T> where T : class
    {
        private readonly TaskQueue _taskQueue = new TaskQueue();

        /// <inheritdoc/>
        public T Database
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public void PerformUpdateAction(Action<T> action, T portfolio)
        {
            _taskQueue.Enqueue(action, portfolio);
        }

        /// <inheritdoc/>
        public void PerformUpdate(object obj, UpdateRequestArgs<T> requestArgs)
        {
            if (!requestArgs.IsHandled)
            {
                _taskQueue.Enqueue(requestArgs.UpdateAction, Database);
                requestArgs.IsHandled = true;
            }
        }
    }
}
