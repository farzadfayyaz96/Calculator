

using System.Windows;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class LoginUserControl
    {
        private LoginViewModel _viewModel;
        public LoginUserControl()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;
            _viewModel.FocusAction = FocusPasswordBox;
            FocusPasswordBox();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        private void FocusPasswordBox()
        {
            PasswordBox.Focus();
        }
    }
}
