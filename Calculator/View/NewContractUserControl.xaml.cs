using System.Windows;
using Calculator.ViewModel;

namespace Calculator.View
{

    public partial class NewContractUserControl
    {
        private readonly NewContractViewModel _viewModel;
        public NewContractUserControl()
        {
            InitializeComponent();
            _viewModel = new NewContractViewModel();
            DataContext = _viewModel;
            _viewModel.NewContract.Date = DatePicker.SelectedDate;
        }

        private void PersianDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.NewContract.Date = DatePicker.SelectedDate;
        }
    }
}
