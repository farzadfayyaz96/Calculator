
using System;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows;
using Arash;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class Function : NotifyProperty
    {
        private int _index;
        private string _amount;
        private PersianDate _date;
        private string _contractType;
        private BigInteger _remainingAmount;
        private string _remainingAmountString;
        public Function()
        {
            RemainingAmount = new BigInteger();
        }

        public string Id { get; set; }

        public string ContractId { get; set; }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

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

        public BigInteger RemainingAmount {
            get=>_remainingAmount;
            set
            {
                _remainingAmount = value;
                OnPropertyChanged(nameof(RemainingAmount));
                RemainingAmountString = _remainingAmount.ToString();
                
            }

        }

        public string RemainingAmountString
        {
            get => _remainingAmountString;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _remainingAmountString = value;
                    OnPropertyChanged(nameof(RemainingAmountString));
                    return;
                }
                var temp = value.Replace(",", "");
                if (!AmountSplitter.AmountRegex.IsMatch(temp) && !string.IsNullOrEmpty(temp))
                {
                    return;
                }
                _remainingAmountString = string.IsNullOrEmpty(temp) ? temp : AmountSplitter.Split(temp, 3);
                OnPropertyChanged(nameof(RemainingAmountString));
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

        public string ContractType
        {
            get => _contractType;
            set
            {
                _contractType = value;
                OnPropertyChanged(nameof(ContractType));
            }
        }

        public override string ToString()
        {
            return $"index = {Index}\r\nid = {Id}\r\ncontract id = {ContractId}\r\ncontract type = {ContractType}\r\namount = {Amount}\r\ndata = {Date.ToString()}\r\n";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            try
            {
                var function = (Function) obj;
                return function.Id.Equals(Id);
            }
            catch (Exception)
            {
                return false;
            }
            
        }


        public void Clear()
        {
            Amount = string.Empty;
            Date = new PersianDate(DateTime.Now);
            Id = string.Empty;

        }

        public static ObservableCollection<FunctionDataGridItem> GetObservableCollection()
        {
            var functionCollection = new ObservableCollection<FunctionDataGridItem>();
            functionCollection.CollectionChanged += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    for (var i = 0; i < functionCollection.Count; i++)
                    {
                        functionCollection[i].ItemFunction.Index = i+1;
                    }
                });
            };
            return functionCollection;
        }
        
    }
}
