using System.Windows.Controls;
using Calculator.ViewModel;

namespace Calculator
{

    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.Instance;
            MainViewModel.Instance.AddPopupAction = AddPopup;
        }


        private void AddPopup(UserControl control)
        {
            Grid.SetRow(control, 0);
            Grid.SetRowSpan(control, 2);
            MainGrid.Children.Add(control);
        }

    }
}
