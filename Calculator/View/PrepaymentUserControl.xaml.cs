using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arash;
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
        private string _enterAmount;
        private bool _updateDateFlag;
        public PrepaymentUserControl(string prepaymentId)
        {
            _updateDateFlag = true;
            InitializeComponent();
            ViewModel = DataContext as PrepaymentViewModel;
            if (ViewModel == null) return;
            ViewModel.PaymentCollection.CollectionChanged += PaymentCollectionOnCollectionChanged;
            //load prepayment item
            try
            {
                ViewModel.PrepaymentItem = PrepaymentDataAccess.SelectById(prepaymentId);
                ViewModel.IsPrepaymentExist = true;
                ViewModel.PrepaymentItem.InitPrepaymentTasks(false, UpdateDatePickers);
            }
            catch (ItemNotFoundException e)
            {
                Logger.LogException(e);
                ViewModel.PrepaymentItem = new Prepayment(prepaymentId)
                {
                    WarrantyDate = WarrantyDatePicker.SelectedDate
                };
                ViewModel.PrepaymentItem.InitPrepaymentTasks(true, UpdateDatePickers);
            }

            _updateDateFlag = false;
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
        private void TaskOneButton_OnClick(object sender, RoutedEventArgs e)
        {
            var text = LevelOneTaskTextBox.Text;
            if (text.Equals(_enterAmount)) return;
            var task = ViewModel.PrepaymentItem.TaskLevelOne;
            if (task.IsInsertMode || task.IsUpdateMode)
            {
                ViewModel.SavePrepaymentTaskChanges(task);
                task.IsInsertMode = false;
                task.IsUpdateMode = false;
            }
            else if (task.IsExistInDatabase && !task.IsUpdateMode)
            {
                DeleteTask(task);
            }
        }

        private void LevelOneTaskTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updateDateFlag) return;
            var text = LevelOneTaskTextBox.Text;
            if (text.Equals(_enterAmount)) return;
            if (string.IsNullOrEmpty(ViewModel.PrepaymentItem.TaskLevelOne.Amount)) return;
            var task = ViewModel.PrepaymentItem.TaskLevelOne;
            if (task.IsExistInDatabase)
            {
                task.IsUpdateMode = true;
            }
            else
            {
                task.IsInsertMode = true;
            }
        }
        
        private void LevelOneDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            if(_updateDateFlag)return;
            ViewModel.PrepaymentItem.TaskLevelOne.Date = LevelOneDatePicker.SelectedDate;
            if(string.IsNullOrEmpty(ViewModel.PrepaymentItem.TaskLevelOne.Amount))return;
            var task = ViewModel.PrepaymentItem.TaskLevelOne;
            if (task.IsExistInDatabase)
            {
                task.IsUpdateMode = true;
            }
            else task.IsInsertMode = true;
        }

        private void TaskTwoButton_OnClick(object sender, RoutedEventArgs e)
        {
            var text = LevelTwoTextBox.Text;
            if (text.Equals(_enterAmount)) return;
            var task = ViewModel.PrepaymentItem.TaskLevelTwo;
            if (task.IsInsertMode || task.IsUpdateMode)
            {
                ViewModel.SavePrepaymentTaskChanges(task);
                task.IsInsertMode = false;
                task.IsUpdateMode = false;
            }
            else if (task.IsExistInDatabase && !task.IsUpdateMode)
            {
                DeleteTask(task);
            }
        }
        private void LevelTwoTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updateDateFlag) return;
            var text = LevelTwoTextBox.Text;
            if (text.Equals(_enterAmount)) return;
            var task = ViewModel.PrepaymentItem.TaskLevelTwo;
            if (string.IsNullOrEmpty(task.Amount)) return;
            if (task.IsExistInDatabase)
            {
                task.IsUpdateMode = true;
            }
            else
            {
                task.IsInsertMode = true;
            }
        }

        private void LevelTwoDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            if (_updateDateFlag) return;
            ViewModel.PrepaymentItem.TaskLevelTwo.Date = LevelTwoDatePicker.SelectedDate;
            var task = ViewModel.PrepaymentItem.TaskLevelTwo;
            if (string.IsNullOrEmpty(task.Amount)) return;
            if (task.IsExistInDatabase)
            {
                task.IsUpdateMode = true;
            }
            else task.IsInsertMode = true;
        }
        private void TaskThreeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var text = LevelThreeTextBox.Text;
            if (text.Equals(_enterAmount)) return;
            var task = ViewModel.PrepaymentItem.TaskLevelThree;
            if (task.IsInsertMode || task.IsUpdateMode)
            {
                ViewModel.SavePrepaymentTaskChanges(task);
                task.IsInsertMode = false;
                task.IsUpdateMode = false;
            }
            else if (task.IsExistInDatabase && !task.IsUpdateMode)
            {
                DeleteTask(task);
            }
        }
        private void LevelThreeTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updateDateFlag) return;
            var text = LevelThreeTextBox.Text;
            if (text.Equals(_enterAmount)) return;
            var task = ViewModel.PrepaymentItem.TaskLevelThree;
            if (string.IsNullOrEmpty(task.Amount)) return;
            if (task.IsExistInDatabase)
            {
                task.IsUpdateMode = true;
            }
            else
            {
                task.IsInsertMode = true;
            }
        }


        private void LevelThreeDatePicker_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            if (_updateDateFlag) return;
            ViewModel.PrepaymentItem.TaskLevelThree.Date = LevelThreeDatePicker.SelectedDate;
            var task = ViewModel.PrepaymentItem.TaskLevelThree;
            if (string.IsNullOrEmpty(task.Amount)) return;
            if (task.IsExistInDatabase)
            {
                task.IsUpdateMode = true;
            }
            else task.IsInsertMode = true;

        }

        private void UpdateDatePickers(PersianDate levelOneDate, PersianDate levelTwoDate, PersianDate levelThreeDate)
        {
            LevelOneDatePicker.SelectedDate = levelOneDate;
            LevelTwoDatePicker.SelectedDate = levelTwoDate;
            LevelThreeDatePicker.SelectedDate = levelThreeDate;
        }

        
        private void LevelOneTaskTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _enterAmount = LevelOneTaskTextBox.Text;
        }

        private void LevelThreeTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _enterAmount = LevelThreeTextBox.Text;
        }

        private void LevelTwoTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _enterAmount = LevelTwoTextBox.Text;
        }

        public void DeleteTask(PrepaymentTask task)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var level = string.Empty;
                PersianDatePicker datePicker = LevelOneDatePicker;
                if (task.Level.Equals("1")) level = "اول";
                else if (task.Level.Equals("2"))
                {
                    level = "دوم";
                    datePicker = LevelTwoDatePicker;
                }
                else if (task.Level.Equals("3"))
                {
                    level = "سوم";
                    datePicker = LevelThreeDatePicker;
                }
                
                try
                {
                    PrepaymentTasksDataAccess.Delete(task.PrepaymentId, task.Level);
                    task.Amount = string.Empty;
                    task.Date = PersianDate.Today;
                    task.IsExistInDatabase = false;
                    datePicker.SelectedDate = PersianDate.Today;
                    var message = "مرحله " + level + " با موفقیت حذف شد.";
                    ViewModel.ShowPrePaymentTaskMessage(message, false);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    var message = "خطا در حین حذف مرحله " + level;
                    ViewModel.ShowPrePaymentTaskMessage(message, true);
                }
                
            });
        }

    }
    
}
