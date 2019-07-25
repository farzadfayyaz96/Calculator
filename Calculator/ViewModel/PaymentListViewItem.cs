using System;
using System.Windows.Input;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    public class PaymentListViewItem : NotifyProperty
    {
        public PaymentListViewItem(Action<PaymentListViewItem> buttonAction,Payment payment)
        {
            PaymentCommand = new CommandHandler(()=> buttonAction(this));
            PaymentItem = payment;

        }

        public Payment PaymentItem { get; }

        public ICommand PaymentCommand { get; }

        


    }
}
