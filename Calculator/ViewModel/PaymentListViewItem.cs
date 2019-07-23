using System;
using System.Windows.Input;
using Arash;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    public class PaymentListViewItem : NotifyProperty
    {
        private bool _isSavedItem;
        public PaymentListViewItem(Action<PaymentListViewItem> addAction,Action<PaymentListViewItem> deleteAction)
        {
            AddCommand = new CommandHandler(()=>addAction(this));
            DeleteCommand = new CommandHandler(()=> deleteAction(this));
            PaymentItem = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                Date = PersianDate.Today
            };

        }

        public Payment PaymentItem { get; }

        public ICommand AddCommand { get; }

        public bool IsSavedItem
        {
            get => _isSavedItem;
            set
            {
                _isSavedItem = value;
                OnPropertyChanged(nameof(IsSavedItem));
            }
        }

        public ICommand DeleteCommand { get; }

    }
}
