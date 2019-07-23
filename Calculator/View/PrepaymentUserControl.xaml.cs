using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arash.PersianDateControls;
using Calculator.CustomException;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class PrepaymentUserControl
    {
        
        public PrepaymentUserControl(string prepaymentId)
        {
            InitializeComponent();
            ViewModel = DataContext as PrepaymentViewModel;
            if (ViewModel == null) return;
            ViewModel.PaymentCollection.CollectionChanged += PaymentCollectionOnCollectionChanged;
            //load prepayment item
            try
            {
                ViewModel.PrepaymentItem = PrepaymentDataAccess.SelectById(prepaymentId);
                ViewModel.IsPrepaymentExist = true;
            }
            catch (ItemNotFoundException e)
            {
                Logger.LogException(e);
                ViewModel.PrepaymentItem = new Prepayment(prepaymentId)
                {
                    WarrantyDate = WarrantyDatePicker.SelectedDate
                };
            }
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

        private void WarrantyDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.PrepaymentItem.WarrantyDate = WarrantyDatePicker.SelectedDate;
        }


        private void LevelOneTaskTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                
                var task = ViewModel.PrepaymentItem.TaskLevelOne;
                Console.WriteLine("fffffffffffffff   " + task.Amount);
                if (task.IsExistInDatabase)
                {
                    //exist so update it
                    try
                    {
                        PrepaymentTasksDataAccess.Update(task);
                        var message = $"مبلغ مراحله اول با مقدار {LevelOneTaskTextBox.Text} ویرایش یافت";
                        ViewModel.ShowPrePaymentTaskMessage(message, false);
                    }
                    catch (Exception exception)
                    {
                        Logger.LogException(exception);
                        ViewModel.ShowPrePaymentTaskMessage("خطا در حین ویرایش مبلغ مرحله اول رخ داده است", true);
                    }

                    return;
                }

                //not exist so insert it
                try
                {
                    PrepaymentTasksDataAccess.Insert(task);
                    var message = $"مقدار مبلغ مرحله اول با مقدار {LevelOneTaskTextBox.Text} ذخیره شد";
                    ViewModel.ShowPrePaymentTaskMessage(message, false);
                    task.IsExistInDatabase = true;
                }
                catch (Exception exception)
                {
                    Logger.LogException(exception);
                    Logger.LogException(exception);
                    ViewModel.ShowPrePaymentTaskMessage("خطا در حین ذخیره مبلغ مرحله اول رخ داده است", true);
                }

            });


        }
    }
    
}
