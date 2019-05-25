
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    public class PaymentsViewModel : NotifyProperty
    {
        private bool _isError;
        private bool _isPaymentCash;
        private bool _isPaymentPapers;
        private string _message;
        private int _selectedContractTypeIndex;
        public PaymentsViewModel(string contractId)
        {
            ItemPayment = new Payment
            {
                ContractId =  contractId,
            };
            IsPaymentCash = true;
            try
            {
                PaymentCollection = PaymentDataAccess.SelectAllByContractId(contractId);
                foreach (var paymentDataGridItem in PaymentCollection)
                {
                    paymentDataGridItem.DeleteAction = DeletePayment;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                ShowMessage("خطا در حین بازیابی پرداخت ها",true);
            }
            SaveCommand = new CommandHandler(Save);
            SelectedContractIndex = 0;
            

        }

        public Payment ItemPayment { get; set; }
        
        public ICommand SaveCommand { get; }

        public ObservableCollection<PaymentDataGridItem> PaymentCollection { get; }

        public bool IsPaymentCash
        {
            get => _isPaymentCash;
            set
            {
                _isPaymentCash = value;
                OnPropertyChanged(nameof(IsPaymentCash));
                if (_isPaymentCash) ItemPayment.PaymentType = "نقدی";
            }
        }

        public bool IsPaymentPapers
        {
            get => _isPaymentPapers;
            set
            {
                _isPaymentPapers = value;
                OnPropertyChanged(nameof(IsPaymentPapers));
                if (_isPaymentPapers) ItemPayment.PaymentType = "اوراق";
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }

        public int SelectedContractIndex
        {
            get => _selectedContractTypeIndex;
            set
            {
                _selectedContractTypeIndex = value;
                OnPropertyChanged(nameof(SelectedContractIndex));
                if (_selectedContractTypeIndex == 0) ItemPayment.ContractType = "پیش پرداخت";
                else if (_selectedContractTypeIndex == 1) ItemPayment.ContractType = "موقت";
                else if (_selectedContractTypeIndex == 2) ItemPayment.ContractType = "قطعی";
                else if (_selectedContractTypeIndex == 3) ItemPayment.ContractType = "تعدیل";
                else if (_selectedContractTypeIndex == 4) ItemPayment.ContractType = "سپرده";
            }
        }

        private void Save()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //check
                if (string.IsNullOrEmpty(ItemPayment.Amount))
                {
                    ShowMessage("مقدار پرداخت را وارد کنید.",true);
                    return;
                }
                //compare contract date to now date
                var dateCompareResult = DateTime.Compare(DateTime.Now, ItemPayment.Date.ToDateTime());
                if (dateCompareResult < 0)
                {
                    ShowMessage("تاریخ قرارداد صحیح نمی باشد", true);
                    return;
                }

                try
                {
                    //add guid to payment
                    ItemPayment.Id = Guid.NewGuid().ToString();
                    //insert to database
                    PaymentDataAccess.Insert(ItemPayment);
                    //add to data grid
                    PaymentCollection.Add(new PaymentDataGridItem(ItemPayment));
                    //clear 
                    ItemPayment.Clear();
                    IsPaymentCash = true;
                    IsPaymentPapers = false;
                    SelectedContractIndex = 0;
                    ShowMessage("پرداخت جدید با موفقیت درج شد",false);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowMessage("خطا در حین درج پرداخت ",true);
                }
                
            });

        }

        private void DeletePayment(PaymentDataGridItem payment)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    //remove from database
                    PaymentDataAccess.Delete(payment.ItemPayment.Id);
                    //remove from view
                    PaymentCollection.Remove(payment);
                    ShowMessage("پرداخت با موفقیت حذف شد",false);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowMessage("خطا در حین حذف پرداخت",true);
                }
            });
            
        }

        private void ShowMessage(string message,bool isError)
        {
            Message = message;
            IsError = isError;
        }
    }
}
