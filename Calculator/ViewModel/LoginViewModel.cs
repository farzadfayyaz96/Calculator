using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.View;

namespace Calculator.ViewModel
{
    class LoginViewModel : NotifyProperty
    {
        private string _password;
        private string _message;

        public LoginViewModel()
        {
            LoginCommand = new CommandHandler(Login);
        }

        public ICommand LoginCommand { get; }
        public Action FocusAction { get; set; }

        public string Message
        {
            get => _message;
            set {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private void Login()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (string.IsNullOrEmpty(Password))
                {
                    Message = "رمز عبور را وارد کنید!";
                    return;
                }
                try
                {
                    var result = ProgramInfoDataAccess.Login(Password);
                    if (result)
                    {
                        MainViewModel.Instance.ChangeContent(new ManageContractUserControl());
                        return;
                    }
                    Message = "رمز عبور معتبر نمی باشد.";
                    FocusAction();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    Message = "خطا در ارتباط با پایگاه داده";
                }
                
            });
        }
    }
}
