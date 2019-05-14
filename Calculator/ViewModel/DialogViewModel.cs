
using System;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    class DialogViewModel : NotifyProperty
    {
        private string _message;
        private bool _isError;
        private string _closeButtonText;
        public DialogViewModel(string message,Action okAction)
        {
            Message = message;
            OkAction = okAction;
            CloseCommand = new CommandHandler(Close);
            OkCommand = new CommandHandler(Ok);
            CloseButtonText = "خیر";
        }

        public DialogViewModel(string message)
        {
            Message = message;
            CloseCommand = new CommandHandler(Close);
            IsError = true;
            CloseButtonText = "باشه";
        }

        public ICommand OkCommand { get; }
        public ICommand CloseCommand { get; }
        public Action OkAction { get; set; }

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

        private void Close()
        {
            MainViewModel.Instance.RemovePopupAction();
        }

        private void Ok()
        {
            OkAction();
            Close();
        }
        


    }
}
