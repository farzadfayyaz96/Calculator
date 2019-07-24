using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Arash;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;
using Calculator.View;

namespace Calculator.ViewModel
{
    public class PrepaymentViewModel : NotifyProperty
    {
        private string _paymentMessage;
        private bool _isMessageError;
        private bool _isPrepaymentExist;
        private string _prepaymentTaskMessage;
        private bool _prepaymentTaskIsError;
        private Prepayment _prepaymentItem;
        public PrepaymentViewModel()
        {
            PaymentCollection = new ObservableCollection<PaymentListViewItem>
            {
                new PaymentListViewItem(AddPayment, DeletePayment),
            };
            DeleteAllCommand = new CommandHandler(DeleteAllPayment);
            AddPrepaymentCommand = new CommandHandler(AddPrepayment);
        }

        public ObservableCollection<PaymentListViewItem> PaymentCollection { get; }
        public ICommand DeleteAllCommand { get; }
        public ICommand AddPrepaymentCommand { get; }

        public string ContractId { get; set; }

        public string PaymentMessage
        {
            get => _paymentMessage;
            set
            {
                _paymentMessage = value;
                OnPropertyChanged(nameof(PaymentMessage));
            }
        }

        public bool IsMessageError
        {
            get => _isMessageError;
            set
            {
                _isMessageError = value;
                OnPropertyChanged(nameof(IsMessageError));
            }
        }

        public string PrepaymentTaskMessage
        {
            get => _prepaymentTaskMessage;
            set
            {
                _prepaymentTaskMessage = value;
                OnPropertyChanged(nameof(PrepaymentTaskMessage));
            }
        }
        public bool IsPrepaymentExist
        {
            get => _isPrepaymentExist;
            set
            {
                _isPrepaymentExist = value;
                OnPropertyChanged(nameof(IsPrepaymentExist));
            }
        }

        public bool PrepaymentTaskOneIsError
        {
            get => _prepaymentTaskIsError;
            set
            {
                _prepaymentTaskIsError = value;
                OnPropertyChanged(nameof(PrepaymentTaskOneIsError));
            }
        }

        public Prepayment PrepaymentItem
        {
            get=>_prepaymentItem;
            set
            {
                _prepaymentItem = value;
                OnPropertyChanged(nameof(PrepaymentItem));
            }
        }

        private void AddPayment(PaymentListViewItem item)
        {
            if (string.IsNullOrEmpty(item.PaymentItem.Amount))
            {
                ShowPaymentMessage("مبلغ پرداخت را وارد کنید", true);
                return;
            }

            item.IsSavedItem = true;
            var newItem = new PaymentListViewItem(AddPayment, DeletePayment);
            PaymentCollection.Add(newItem);
            ShowPaymentMessage($"پرداخت با مبلغ  {item.PaymentItem.Amount} با موفقیت ذخیره شد", false);
        }

        private void DeletePayment(PaymentListViewItem item)
        {
            for (var i = 0; i < PaymentCollection.Count; i++)
            {
                var paymentListViewItem = PaymentCollection[i];
                if (!paymentListViewItem.PaymentItem.Id.Equals(item.PaymentItem.Id)) continue;


                //get contract windows
                var contractWindowList = ManageContractViewModel.Instance.EditContractWindowList;
                EditContractWindow editWindow = null;
                foreach (var window in contractWindowList)
                {
                    if (!window.ViewModel.ContractId.Equals(ContractId))
                    {
                        //not found yet!!!
                        continue;
                    }

                    //found it and add new popup dialog to windows
                    editWindow = window;
                    break;
                }

                //show dialog
                var message =
                    $"آیا مایل به حذف پرداخت به مبلغ '{item.PaymentItem.Amount}' و در تاریخ '{item.PaymentItem.Date}' می باشید ؟";
                var dialog = new DialogUserControl(message, () => { PaymentCollection.RemoveAt(i); },
                    editWindow?.ViewModel.RemovePopupAction);
                editWindow?.ViewModel.AddPopupAction(dialog);
                break;
            }
        }

        private void DeleteAllPayment()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                //get contract windows
                var contractWindowList = ManageContractViewModel.Instance.EditContractWindowList;
                EditContractWindow editWindow = null;
                foreach (var window in contractWindowList)
                {
                    if (!window.ViewModel.ContractId.Equals(ContractId))
                    {
                        //not found yet!!!
                        continue;
                    }

                    //found it and add new popup dialog to windows
                    editWindow = window;
                    break;
                }

                //show dialog
                var message = $"آیا مایل به حذف همه ی پرداخت ها هستید؟";
                var dialog = new DialogUserControl(message, () =>
                {
                    PaymentCollection.Clear();
                    PaymentCollection.Add(new PaymentListViewItem(AddPayment, DeletePayment));
                }, editWindow?.ViewModel.RemovePopupAction);
                editWindow?.ViewModel.AddPopupAction(dialog);
                ShowPaymentMessage("همه پرداخت ها با موفقیت حذف شدند",false);
            });
            
        }

        private void ShowPaymentMessage(string message, bool isError)
        {
            PaymentMessage = message;
            IsMessageError = isError;
        }

        public void ShowPrePaymentTaskMessage(string message,bool isError)
        {
            PrepaymentTaskOneIsError = isError;
            PrepaymentTaskMessage = message;
        }

        private void AddPrepayment()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    PrepaymentDataAccess.Insert(PrepaymentItem);
                    IsPrepaymentExist = true;
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }

            });
        }


        public void SavePrepaymentTaskChanges(PrepaymentTask task)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                
                var level = string.Empty;
                if (task.Level.Equals("1")) level = "اول";
                else if (task.Level.Equals("2")) level = "دوم";
                else if (task.Level.Equals("3")) level = "سوم";
                if (task.IsExistInDatabase){
                    //exist so update it
                    try
                    {
                        PrepaymentTasksDataAccess.Update(task);
                        var message = "مرحله " + level + " با موفقیت ویرایش یافت";
                        ShowPrePaymentTaskMessage(message, false);
                    }
                    catch (Exception exception)
                    {
                        Logger.LogException(exception);
                        var message = "خطا در حین ویرایش مرحله" + level;
                        ShowPrePaymentTaskMessage(message, true);
                    }

                    return;
                }

                //not exist so insert it
                try
                {
                    PrepaymentTasksDataAccess.Insert(task);
                    var message = "مرحله " + level + " با موفقیت ذخیره شد";
                    ShowPrePaymentTaskMessage(message, false);
                    task.IsExistInDatabase = true;
                }
                catch (Exception exception)
                {
                    Logger.LogException(exception);
                    var message = "خطا در حین ویرایش مرحله " + level;
                    ShowPrePaymentTaskMessage(message, true);
                }

            });
        }

       

    }
}