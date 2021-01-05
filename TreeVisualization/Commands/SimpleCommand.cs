using System;
using System.Windows.Input;

namespace Trees
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _execute;

        public SimpleCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }
    }
}