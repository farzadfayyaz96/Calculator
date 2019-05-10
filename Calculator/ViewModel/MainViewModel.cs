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

        private MainViewModel()
        {
            FlyoutCommand = new CommandHandler(FlyoutAction);
            AboutUsCommand = new CommandHandler(ShowAboutUs);
            Control = new ManageContractUserControl();
        }

        public static MainViewModel Instance = new MainViewModel();

        public Action<UserControl> AddPopupAction { get; set; }


        public ICommand AboutUsCommand { get; }
        public ICommand FlyoutCommand { get; }

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

        
    }
}
