﻿using System.Windows.Input;

namespace Calculator.ViewModel
{
    class AboutUsViewModel : NotifyProperty
    {

        public AboutUsViewModel()
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
