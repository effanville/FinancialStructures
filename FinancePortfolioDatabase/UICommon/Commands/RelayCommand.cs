using System;
using System.Windows.Input;

namespace UICommon.Commands
{
    /// <summary>
    /// Command instance that executes without an argument required.
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        private readonly Func<bool> fCanExecute;
        private readonly Action fExecute;

        /// <summary>
        /// Constructor that takes an execution method, and can always execute
        /// </summary>
        public RelayCommand(Action execute)
           : this(execute, () => true)
        {
            fExecute = execute;
        }

        /// <summary>
        /// Constructor that takes a execution method and whether one can execute.
        /// </summary>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            fExecute = execute;
            fCanExecute = canExecute;
        }

        /// <inheritdoc/>
        public bool CanExecute(object parameter = null)
        {
            return fCanExecute == null || fCanExecute();
        }

        /// <inheritdoc/>
        public void Execute(Object obj = null)
        {
            fExecute();
        }

        // Ensures WPF commanding infrastructure asks all RelayCommand objects whether their
        // associated views should be enabled whenever a command is invoked.
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

        private event EventHandler fCanExecuteChangedInternal;

        public void OnCanExecuteChanged()
        {
            EventHandler handler = fCanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
