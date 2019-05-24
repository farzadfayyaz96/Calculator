
using System.Windows;
using System.Windows.Controls;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class FunctionsUserControl
    {
        
        public FunctionsUserControl(string contractId)
        {
            InitializeComponent();
            ViewModel = new FunctionsViewModel(contractId);
            DataContext = ViewModel;
            ViewModel.ItemFunction.Date = PersianDatePicker.SelectedDate;
        }

        public FunctionsViewModel ViewModel { get; }

        private void DataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            FunctionsDataGrid.UnselectAll();
        }

        private void PersianDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ItemFunction.Date = PersianDatePicker.SelectedDate;
        }
    }
}
