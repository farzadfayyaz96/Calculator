using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calculator.View;

namespace Calculator.ViewModel
{
    class MainViewModel : NotifyProperty
    {
        private bool _flyoutIsOpen;
        private UserControl _control;
        private bool _isLogin;

        private MainViewModel()
        {
            FlyoutCommand = new CommandHandler(FlyoutAction);
            AboutUsCommand = new CommandHandler(ShowAboutUs);
            Control = new LoginUserControl();
            NewContractCommand = new CommandHandler(ShowNewContract);
            ManageContractCommand = new CommandHandler(ShowManageContract);
            ManageProfitCommand = new CommandHandler(ShowManageProfit);
            SettingCommand = new CommandHandler(ShowSetting);
            IsLogin = true;
        }

        public static MainViewModel Instance = new MainViewModel();

        public Action<UserControl> AddPopupAction { get; set; }
        public Action RemovePopupAction { get; set; }

        public ICommand NewContractCommand { get; }
        public ICommand ManageProfitCommand { get; }
        public ICommand ManageContractCommand { get; }
        public ICommand SettingCommand { get; }
        public ICommand AboutUsCommand { get; }
        public ICommand FlyoutCommand { get; }
        public bool IsLogin
        {
            get => _isLogin;
            set
            {
                _isLogin = value;
                OnPropertyChanged(nameof(IsLogin));
            }
        }
        public bool FlyoutIsOpen
        {
            get => _flyoutIsOpen;
            set
            {
                _flyoutIsOpen = value;
                OnPropertyChanged(nameof(FlyoutIsOpen));
            }
        }
        public UserControl Control
        {
            get => _control;
            set
            {
                _control = value;
                OnPropertyChanged(nameof(Control));
            }
        }

        private void FlyoutAction()
        {
            FlyoutIsOpen = !FlyoutIsOpen;

        }
        private void ShowAboutUs()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var aboutUsControl = new AboutUsUserControl();
                AddPopupAction(aboutUsControl);
                FlyoutAction();

            });
            
        }
        public void ChangeContent(UserControl control)
        {
            Control = control;
        }
        private void ShowManageProfit()
        {
            FlyoutAction();
            Control = new ManageProfitUserControl();
            
        }
        private void ShowManageContract()
        {
            FlyoutAction();
            Control = new ManageContractUserControl();
            
        }
        private void ShowNewContract()
        {
            FlyoutAction();
            Control = new NewContractUserControl();
            
        }
        private void ShowSetting()
        {
            FlyoutAction();
            AddPopupAction(new SettingUserControl());
            
        }
        
    }
}
