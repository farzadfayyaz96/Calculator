using System;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class DialogUserControl 
    {
        public DialogUserControl(string message,Action closeAction)
        {
            InitializeComponent();
            DataContext = new DialogViewModel(message,closeAction);
        }

        public DialogUserControl(string message,Action okAction,Action closeAction)
        {
            InitializeComponent();
            DataContext = new DialogViewModel(message,okAction,closeAction);
        }
    }
}
