
using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace Calculator.ViewModel
{
    class DialogViewModel : NotifyProperty
    {
        private string _message;
        private bool _isError;
        private string _closeButtonText;
        public DialogViewModel(string message,Action okAction,Action closeAction)
        {
            Message = message;
            OkAction = okAction;
            CloseAction = closeAction;
            CloseCommand = new CommandHandler(closeAction);
            OkCommand = new CommandHandler(Ok);
            CloseButtonText = "خیر";
        }

        public DialogViewModel(string message,Action closeAction)
        {
            Message = message;
            CloseAction = closeAction;
            CloseCommand = new CommandHandler(CloseAction);
            IsError = true;
            CloseButtonText = "باشه";
        }

        public ICommand OkCommand { get; }
        public ICommand CloseCommand { get; }
        public Action OkAction { get; set; }

        public Action CloseAction { get; set; }

        public string CloseButtonText
        {
            get => _closeButtonText;
            set
            {
                _closeButtonText = value;
                OnPropertyChanged(nameof(CloseButtonText));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }

        private void Ok()
        {
            Application.Current.Invoke(() =>
            {
                OkAction();
                CloseAction();
            });
            
        }
        


    }
}
