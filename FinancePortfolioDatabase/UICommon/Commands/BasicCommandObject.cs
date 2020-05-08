using System;
using System.Windows.Input;

namespace UICommon.Commands
{
    public sealed class BasicCommandObject : ICommand
    {
        private readonly Action<object> fExecute;

        private readonly Predicate<object> fCanExecute;

        private event EventHandler fCanExecuteChangedInternal;

        public BasicCommandObject(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public BasicCommandObject(Action<object> execute, Predicate<object> canExecute)
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

        public bool CanExecute(object parameter)
        {
            return fCanExecute != null && fCanExecute((object)parameter);
        }

        public void Execute(object parameter)
        {
            fExecute((object)parameter);
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

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
