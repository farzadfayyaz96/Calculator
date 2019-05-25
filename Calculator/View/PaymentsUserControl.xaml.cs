﻿
using System.Windows;
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
        }

        public PaymentsViewModel ViewModel { get;  }

        private void PersianDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ItemPayment.Date = PersianDatePicker.SelectedDate;
        }
    }
}
