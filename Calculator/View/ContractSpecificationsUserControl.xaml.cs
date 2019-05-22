using System.Windows;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.View
{
    
    public partial class ContractSpecificationsUserControl
    {
        private readonly ContractSpecificationsViewModel _viewModel;
        public ContractSpecificationsUserControl(Contract contract)
        {
            InitializeComponent();
            _viewModel = new ContractSpecificationsViewModel(contract);
            DataContext = _viewModel;
        }

        private void DatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.EditContract.Date = DatePicker.SelectedDate;
        }
    }
}
