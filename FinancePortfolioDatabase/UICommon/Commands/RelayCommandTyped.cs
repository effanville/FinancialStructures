using System;
using System.Windows.Input;

namespace UICommon.Commands
{
    /// <summary>
    /// Command instance that executes with an argument of the specified type T.
    /// </summary>
    public sealed class RelayCommand<T> : ICommand
    {
        private readonly Action<T> fExecute;

        private readonly Predicate<T> fCanExecute;

        private event EventHandler fCanExecuteChangedInternal;

        /// <summary>
        /// Constructor that takes an execution method, and can always execute
        /// </summary>
        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Constructor that takes a execution method and whether one can execute.
        /// </summary>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            fExecute = execute;
            fCanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                fCanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                fCanExecuteChangedInternal -= value;
            }
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return fCanExecute != null && fCanExecute((T)parameter);
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            fExecute((T)parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = fCanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        private static bool DefaultCanExecute(T parameter)
        {
            return true;
        }
    }
}
