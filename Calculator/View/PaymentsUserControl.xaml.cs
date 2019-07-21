
using System.Windows;
using Arash;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class PaymentsUserControl 
    {

        public PaymentsUserControl(Contract contract)
        {
            InitializeComponent();
            ViewModel = new PaymentsViewModel(contract.Id);
            DataContext = ViewModel;
            ViewModel.ItemPayment.Date = PersianDatePicker.SelectedDate;
            ViewModel.ChangeSelectedDateAction = ChangeDatePickerDate;
        }

        public PaymentsViewModel ViewModel { get;  }

        private void PersianDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ItemPayment.Date = PersianDatePicker.SelectedDate;
        }

        private void ChangeDatePickerDate(PersianDate date)
        {
            PersianDatePicker.SelectedDate = date;
        }
    }
}
