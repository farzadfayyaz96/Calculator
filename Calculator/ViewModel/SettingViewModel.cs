using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;

namespace Calculator.ViewModel
{
    class SettingViewModel:NotifyProperty
    {

        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;
        private string _message;
        private bool _isError;

        public SettingViewModel()
        {
            CloseCommand = new CommandHandler(Close);
            ChangePasswordCommand = new CommandHandler(ChangePassword);
        }

        public ICommand CloseCommand { get; }

        public ICommand ChangePasswordCommand { get; }

        public Action FocusAction { get; set; }

        public Action ClearPasswordAction { get; set; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
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

        private void ChangePassword()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //check variable
                if (string.IsNullOrEmpty(OldPassword))
                {
                    ShowError("رمز عبور فعلی را وارد کنید.");
                    FocusAction();
                    return;
                }

                if (string.IsNullOrEmpty(NewPassword))
                {
                    ShowError("رمز عبور جدید را وارد کنید.");
                    FocusAction();
                    return;
                }

                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    ShowError("تایید رمز عبور را وارد کنید");
                    FocusAction();
                    return;
                }

                if (!NewPassword.Equals(ConfirmPassword))
                {
                    ShowError("رمز عبور جدید مطابقت ندارد");
                    FocusAction();
                    return;
                }
                try
                {
                    //check old password
                    var result = ProgramInfoDataAccess.Login(OldPassword);
                    if (!result)
                    {
                        ShowError("رمز عبور فعلی اشتباه است");
                        FocusAction();
                        return;
                    }
                    //update password
                    ProgramInfoDataAccess.UpdatePassword(NewPassword);
                    ShowMessage("رمز عبور با موفقیت تغییر یافت.");
                    ClearPasswordAction();
                    FocusAction();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowError("خطا در ارتباط با پیگاه داده");
                }
            });
        }

        private void ShowError(string message)
        {
            IsError = true;
            Message = message;

        }

        private void ShowMessage(string message)
        {
            IsError = false;
            Message = message;
        }
    }
}
