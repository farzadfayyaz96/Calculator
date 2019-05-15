
using System.Windows;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.View
{

    public partial class EditContractWindow
    {
        private readonly EditContractViewModel _viewModel;
        public EditContractWindow(Contract contract)
        {
            InitializeComponent();
            _viewModel = new EditContractViewModel(contract);
            DataContext = _viewModel;
            DatePicker.SelectedDate = contract.Date;
        }

        private void DatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.ItemContract.Date = DatePicker.SelectedDate;
        }
    }
}
