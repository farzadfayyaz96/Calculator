using System;
using System.Collections.ObjectModel;
using System.Numerics;
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
        private string _paymentSum;
        private BigInteger _paymentSumBigInteger;


        public PrepaymentViewModel()
        {
            PaymentCollection = new ObservableCollection<PaymentListViewItem>();
            DeleteAllCommand = new CommandHandler(DeleteAllPayment);
            AddPrepaymentCommand = new CommandHandler(AddPrepayment);
            PaymentSumBigInteger = 0;
        }


        public ObservableCollection<PaymentListViewItem> PaymentCollection { get; }
        public ICommand DeleteAllCommand { get; }
        public ICommand AddPrepaymentCommand { get; }

        public BigInteger PaymentSumBigInteger
        {
            get=>_paymentSumBigInteger;
            set
            {
                _paymentSumBigInteger = value;
                OnPropertyChanged(nameof(PaymentSumBigInteger));
                PaymentSum = _paymentSumBigInteger == 0 ? string.Empty : _paymentSumBigInteger.ToString();
            }
        }
        public string PaymentSum
        {
            get => _paymentSum;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _paymentSum = value;
                    OnPropertyChanged(nameof(PaymentSum));
                    return;
                }
                var temp = value.Replace(",", "");
                if (!AmountSplitter.AmountRegex.IsMatch(temp) && !string.IsNullOrEmpty(temp))
                {
                    return;
                }
                _paymentSum = string.IsNullOrEmpty(temp) ? temp : AmountSplitter.Split(temp, 3);
                OnPropertyChanged(nameof(PaymentSum));

            }
        }

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

        public void PaymentButtonAction(PaymentListViewItem item)
        {
            if (item.PaymentItem.IsExistInDatabase)
            {
                //update
                if (item.PaymentItem.IsUpdateMode)
                {
                    if (string.IsNullOrEmpty(item.PaymentItem.Amount))
                    {
                        ShowPaymentMessage("مبلغ پرداخت را وارد کنید", true);
                        return;
                    }
                    try
                    {
                        PaymentDataAccess.Update(item.PaymentItem);
                        item.PaymentItem.IsUpdateMode = false;
                        //change payment sum 
                        OnPropertyChanged(PaymentSum);
                        ShowPaymentMessage("پرداخت با موفقیت ویرایش شد",false);
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                        ShowPaymentMessage("خطا در حین ویرایش پرداخت",true);
                    }
                }
                //delete
                else
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
                        var message = $"آیا مایل به حذف پرداخت به مبلغ '{item.PaymentItem.Amount}' و در تاریخ '{item.PaymentItem.Date}' می باشید ؟";
                        var dialog = new DialogUserControl(message, () =>
                            {
                                try
                                {
                                    PaymentDataAccess.Delete(item.PaymentItem.Id);
                                    item.PaymentItem.IsExistInDatabase = false;
                                    PaymentCollection.RemoveAt(i);

                                    if (PaymentCollection.Count == 0)
                                    {
                                        //add temp item 
                                        var paymentId = Guid.NewGuid().ToString();  
                                        var tempPayment = new Payment(paymentId,PrepaymentItem.Id)
                                        {
                                            Date = PersianDate.Today
                                        };
                                        var temp = new PaymentListViewItem(PaymentButtonAction,tempPayment);
                                        PaymentCollection.Add(temp);
                                    }
                                    ShowPaymentMessage("پرداخت با موفقیت حذف شد",false);
                                }
                                catch (Exception e)
                                {
                                    Logger.LogException(e);
                                    ShowPaymentMessage("خطا در حین حذف پرداخت",true);
                                }
                                
                            },
                            editWindow?.ViewModel.RemovePopupAction);
                        editWindow?.ViewModel.AddPopupAction(dialog);
                        break;
                    }
                }
            }
            //insert
            else
            {
                //inset
                if (string.IsNullOrEmpty(item.PaymentItem.Amount))
                {
                    ShowPaymentMessage("مبلغ پرداخت را وارد کنید", true);
                    return;
                }


                var paymentId = Guid.NewGuid().ToString();
                var payment = new Payment(paymentId, PrepaymentItem.Id)
                {
                    Date = PersianDate.Today
                };
                var newItem = new PaymentListViewItem(PaymentButtonAction, payment);
                try
                {
                    PaymentDataAccess.Insert(item.PaymentItem);
                    item.PaymentItem.IsExistInDatabase = true;
                    item.PaymentItem.IsInsetMode = false;
                    PaymentCollection.Add(newItem);
                    ShowPaymentMessage($"پرداخت با مبلغ  {item.PaymentItem.Amount} با موفقیت ذخیره شد", false);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowPaymentMessage("خطا در حین ذخیره پرداخت", false);
                }
            }
            UpdatePaymentSum();
        }

        public void UpdatePaymentSum()
        {
            //update payment sum 
            var sum = BigInteger.Zero;
            foreach (var paymentListViewItem in PaymentCollection)
            {
                sum += paymentListViewItem.PaymentItem.AmountBigInteger;
            }
            PaymentSumBigInteger = sum;
        }

        private void DeleteAllPayment()
        {
            Application.Current.Dispatcher.Invoke(() =>
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
                var message = "آیا مایل به حذف همه ی پرداخت ها هستید؟";
                var dialog = new DialogUserControl(message, () =>
                {
                    try
                    {
                        //delete all
                        PaymentDataAccess.DeleteAll(PrepaymentItem.Id);
                        PaymentCollection.Clear();
                        var paymentId = Guid.NewGuid().ToString();
                        var payment = new Payment(paymentId, PrepaymentItem.Id)
                        {
                            Date = PersianDate.Today
                        };
                        PaymentCollection.Add(new PaymentListViewItem(PaymentButtonAction, payment));
                        UpdatePaymentSum();
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                        ShowPaymentMessage("خطا در حین حذف همه پرداخت ها",true);
                    }
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

        public void AddPrepayment()
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

                    
                }
                else
                {
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
                }

                PrepaymentItem.UpdatePrepaymentSum();
                
            });
        }

       
       

    }
}