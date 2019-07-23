using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Arash.PersianDateControls;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class PrepaymentUserControl
    {
        
        public PrepaymentUserControl()
        {
            InitializeComponent();
            ViewModel = DataContext as PrepaymentViewModel;
            ViewModel.PaymentCollection.CollectionChanged+=PaymentCollectionOnCollectionChanged;
        }

        private void PaymentCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset + 120);
            
        }

        public PrepaymentViewModel ViewModel { get; set; }



        /**
         * get date picker payment list view item
         * set selected date to payment date
         */
        private void PersianDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            //convert sender to date picker
            if (!(sender is PersianDatePicker datePicker)) return;
            //convert date picker parent to grid for get payment id
            if (!(datePicker.Parent is Grid parent)) return;
            //find payment by id and set selected date to his date
            foreach (var item in ViewModel.PaymentCollection)
            {
                if(!item.PaymentItem.Id.Equals(parent.Tag))continue;
                item.PaymentItem.Date = datePicker.SelectedDate;
                break;
            }

        }

        private void PaymentListView_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Console.WriteLine(e.Delta);
            MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset + (e.Delta * -1));
        }

        private void AmountTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
           
        }

        private void AmountTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            
        }
    }
    
}
