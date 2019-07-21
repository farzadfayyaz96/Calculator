using System;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    class CommandHandler : ICommand 
    {
        private readonly Action _executeAction;
        public CommandHandler(Action action)
        {
            _executeAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeAction();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
