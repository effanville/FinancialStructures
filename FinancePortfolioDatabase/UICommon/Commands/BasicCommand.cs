using System;
using System.Windows.Input;

namespace UICommon.Commands
{
    public sealed class BasicCommand : ICommand
    {
        private readonly Func<bool> fCanExecute;
        private readonly Action fExecute;

        public BasicCommand(Action execute)
           : this(execute, null)
        {
            fExecute = execute;
        }

        public BasicCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            fExecute = execute;
            fCanExecute = canExecute;
        }

        public bool CanExecute(object parameter = null)
        {
            return fCanExecute == null || fCanExecute();
        }

        public void Execute(Object obj = null)
        {
            fExecute();
        }

        // Ensures WPF commanding infrastructure asks all RelayCommand objects whether their
        // associated views should be enabled whenever a command is invoked 
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

        public void RaiseCanExecuteChanged()
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
