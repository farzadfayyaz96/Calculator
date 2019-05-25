using System;
using System.Collections.ObjectModel;
using System.Windows;
using Arash;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class Payment :NotifyProperty
    {
        private string _amount;
        private PersianDate _date;
        private int _index;
        private string _contractType;
        private string _paymentType;
        
       
        public string Id { get; set; }

        public string ContractId { get; set; }

        public string Amount
        {
            get => _amount;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                    return;
                }
                var temp = value.Replace(",", "");
                if (!AmountSplitter.AmountRegex.IsMatch(temp) && !string.IsNullOrEmpty(temp))
                {
                    return;
                }
                _amount = string.IsNullOrEmpty(temp) ? temp : AmountSplitter.Split(temp, 3);
                OnPropertyChanged(nameof(Amount));

            }
        }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        public string ContractType
        {
            get => _contractType;
            set
            {
                _contractType = value;
                OnPropertyChanged(nameof(ContractType));
            }
        }

        public string PaymentType
        {
            get => _paymentType;
            set
            {
                _paymentType = value;
                OnPropertyChanged(nameof(PaymentType));
            }
        }

        public PersianDate Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public void Clear()
        {
            Amount = string.Empty;
            Date = new PersianDate(DateTime.Now);
            Id = string.Empty;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            try
            {
                var payment = (Payment) obj;
                return payment.Id.Equals(Id);
            }
            catch (Exception )
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"id = {Id}\r\ncontract id = {ContractId}\r\namount = {Amount}\r\npayment type = {PaymentType}\r\ndate = {Date}";
        }
        public static ObservableCollection<PaymentDataGridItem> GetPaymentDataGridItemCollection()
        {
            var list = new ObservableCollection<PaymentDataGridItem>();
            list.CollectionChanged += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        list[i].ItemPayment.Index = i + 1;
                    }
                });
               
            };
            return list;
        }

    }
}
