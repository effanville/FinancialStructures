using System;
using System.Windows.Input;

namespace UICommon.Commands
{
    public sealed class BasicCommand<T> : ICommand
    {
        private readonly Action<T> fExecute;

        private readonly Predicate<T> fCanExecute;

        private event EventHandler fCanExecuteChangedInternal;

        public BasicCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public BasicCommand(Action<T> execute, Predicate<T> canExecute)
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
            return fCanExecute != null && fCanExecute((T)parameter);
        }

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
