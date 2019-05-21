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
        private string _changePasswordMessage;
        private bool _isError;

        public SettingViewModel()
        {
            CloseCommand = new CommandHandler(Close);
            ChangePasswordCommand = new CommandHandler(ChangePassword);
            SaveProfitContractTypeCommand = new CommandHandler(SaveProfitContractType);
        }

        public ICommand CloseCommand { get; }

        public ICommand ChangePasswordCommand { get; }

        public ICommand SaveProfitContractTypeCommand { get; }

        public Action FocusAction { get; set; }

        public Action ClearPasswordAction { get; set; }

        public string ChangePasswordMessage
        {
            get => _changePasswordMessage;
            set
            {
                _changePasswordMessage = value;
                OnPropertyChanged(nameof(ChangePasswordMessage));
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
                    ShowChangePasswordError("رمز عبور فعلی را وارد کنید.");
                    FocusAction();
                    return;
                }

                if (string.IsNullOrEmpty(NewPassword))
                {
                    ShowChangePasswordError("رمز عبور جدید را وارد کنید.");
                    FocusAction();
                    return;
                }

                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    ShowChangePasswordError("تایید رمز عبور را وارد کنید");
                    FocusAction();
                    return;
                }

                if (!NewPassword.Equals(ConfirmPassword))
                {
                    ShowChangePasswordError("رمز عبور جدید مطابقت ندارد");
                    FocusAction();
                    return;
                }
                try
                {
                    //check old password
                    var result = ProgramInfoDataAccess.Login(OldPassword);
                    if (!result)
                    {
                        ShowChangePasswordError("رمز عبور فعلی اشتباه است");
                        FocusAction();
                        return;
                    }
                    //update password
                    ProgramInfoDataAccess.UpdatePassword(NewPassword);
                    ShowChangePasswordMessage("رمز عبور با موفقیت تغییر یافت.");
                    ClearPasswordAction();
                    FocusAction();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowChangePasswordError("خطا در ارتباط با پیگاه داده");
                }
            });
        }

        private void ShowChangePasswordError(string message)
        {
            IsError = true;
            ChangePasswordMessage = message;

        }

        private void ShowChangePasswordMessage(string message)
        {
            IsError = false;
            ChangePasswordMessage = message;
        }

        private void SaveProfitContractType()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //do something here!!!
            });
        }
    }
}
