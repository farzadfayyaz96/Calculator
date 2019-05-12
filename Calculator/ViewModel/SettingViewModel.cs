using System.Windows.Input;

namespace Calculator.ViewModel
{
    class SettingViewModel
    {


        public SettingViewModel()
        {
            CloseCommand = new CommandHandler(Close);
        }

        public ICommand CloseCommand { get; }


        private void Close()
        {
            MainViewModel.Instance.RemovePopupAction();
        }
    }
}
