using System;
using System.Windows;
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
            MainViewModel.Instance.RemovePopupAction = RemovePopup;
        }


        private void AddPopup(UserControl control)
        {
            Grid.SetRow(control, 0);
            Grid.SetRowSpan(control, 2);
            MainGrid.Children.Add(control);
        }

        private void RemovePopup()
        {
            var index = MainGrid.Children.Count - 1 ;
            MainGrid.Children.RemoveAt(index);
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
