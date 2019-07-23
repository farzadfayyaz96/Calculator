
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Calculator.View;

namespace Calculator.ViewModel
{
    public class PrepaymentViewModel : NotifyProperty
    {
        private string _level1PrepaymentText;
        private string _level2PrepaymentText;
        private string _level3PrepaymentText;
        private string _message;
        private bool _isMessageError;
        public PrepaymentViewModel()
        {
            PaymentCollection = new ObservableCollection<PaymentListViewItem>
            {
                new PaymentListViewItem(AddPayment,DeletePayment),
            };
            DeleteAllCommand = new CommandHandler(DeleteAllPayment);
        }

        public ObservableCollection<PaymentListViewItem> PaymentCollection { get; }
        public ICommand DeleteAllCommand { get; }

        public string Level1PrepaymentText
        {
            get => _level1PrepaymentText;
            set
            {
                _level1PrepaymentText = value;
                OnPropertyChanged(nameof(Level1PrepaymentText));
            }
        }

        public string Level2PrepaymentText
        {
            get => _level2PrepaymentText;
            set
            {
                _level2PrepaymentText = value;
                OnPropertyChanged(nameof(Level2PrepaymentText));
            }
        }

        public string Level3PrepaymentText
        {
            get => _level3PrepaymentText;
            set
            {
                _level3PrepaymentText = value;
                OnPropertyChanged(nameof(Level3PrepaymentText));
            }
        }
        public string ContractId { get; set; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
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

        private void AddPayment(PaymentListViewItem item)
        {
            if (string.IsNullOrEmpty(item.PaymentItem.Amount))
            {
                ShowMessage("مبلغ پرداخت را وارد کنید",true);
                return;
            }
            item.IsSavedItem = true;
            var newItem = new PaymentListViewItem(AddPayment,DeletePayment);
            PaymentCollection.Add(newItem);
            ShowMessage("پرداخت با موفقیت ذخیره شد",false);
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
                var message = $"آیا مایل به حذف پرداخت به مبلغ '{item.PaymentItem.Amount}' و در تاریخ '{item.PaymentItem.Date}' می باشید ؟";
                var dialog = new DialogUserControl(message, () => { PaymentCollection.RemoveAt(i); },editWindow?.ViewModel.RemovePopupAction);
                editWindow?.ViewModel.AddPopupAction(dialog);
                break;
            }
            
        }

        private void DeleteAllPayment()
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

        }

        private void ShowMessage(string message,bool isError)
        {
            Message = message;
            IsMessageError = isError;
        }
    }
}
