using System;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class DialogUserControl 
    {
        public DialogUserControl(string message)
        {
            InitializeComponent();
            DataContext = new DialogViewModel(message);
        }

        public DialogUserControl(string message,Action action)
        {
            InitializeComponent();
            DataContext = new DialogViewModel(message,action);
        }
    }
}
