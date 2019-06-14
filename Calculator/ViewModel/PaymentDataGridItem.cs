using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Model.TableObject;
using Calculator.View;

namespace Calculator.ViewModel
{
    public class PaymentDataGridItem
    {
        public PaymentDataGridItem(Payment payment)
        {
            ItemPayment = new Payment
            {
                Id = payment.Id,
                ContractType = payment.ContractType,
                Date = payment.Date,
                Amount = payment.Amount,
                ContractId = payment.ContractId,
                PaymentType = payment.PaymentType
            };

            DeleteCommand = new CommandHandler(ShowDeleteDialog);
            EditCommand = new CommandHandler(()=>{});
        }

        public Payment ItemPayment { get; set; }

        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public Action<PaymentDataGridItem> DeleteAction { get; set; }

        public void ShowDeleteDialog()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //get contract windows
                var contractWindowList = ManageContractViewModel.Instance.EditContractWindowList;
                EditContractWindow editWindow = null;
                foreach (var window in contractWindowList)
                {
                    if (!window.ViewModel.PaymentsControl.ViewModel.ItemPayment.ContractId.Equals(ItemPayment.ContractId))
                    {
                        //not found yet!!!
                        continue;
                    }
                    //found it and add new popup dialog to windows
                    editWindow = window;
                    break;
                }
                //init dialog for delete contract
                var message = $"آیا مایل به حذف کارکرد به مبلغ '{ItemPayment.Amount}' و از نوع '{ItemPayment.ContractType}' می باشید؟";
                var dialog = new DialogUserControl(message, () => DeleteAction(this), editWindow?.ViewModel.RemovePopupAction);
                editWindow?.ViewModel.AddPopupAction(dialog);
            });
            
        }

    }
}
