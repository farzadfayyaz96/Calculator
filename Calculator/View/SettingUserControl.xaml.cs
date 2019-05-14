
using System.Windows;
using Calculator.ViewModel;

namespace Calculator.View
{
   
    public partial class SettingUserControl
    {
        private readonly SettingViewModel _viewModel;
        public SettingUserControl()
        {
            InitializeComponent();
            _viewModel = new SettingViewModel();
            DataContext = _viewModel;
            _viewModel.FocusAction = FocusPasswordBox;
            _viewModel.ClearPasswordAction = ClearPasswords;
        }

        private void OldPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.OldPassword = OldPasswordBox.Password;
        }

        private void SettingUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            OldPasswordBox.Focus();
        }

        private void ConfirmPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }

        private void NewPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.NewPassword = NewPasswordBox.Password;
        }

        private void FocusPasswordBox()
        {
            OldPasswordBox.Focus();
        }

        private void ClearPasswords()
        {
            OldPasswordBox.Password = string.Empty;
            NewPasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
        }
    }
}
