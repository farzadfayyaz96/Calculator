using System.Numerics;
using Arash;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class Payment : NotifyProperty
    {

        private string _amount;
        private BigInteger _remainingAmount;
        private PersianDate _date;


        public string Id { get; set; }

        public string Amount
        {
            get => _amount;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                    RemainingAmount = 0;
                    return;
                }
                var temp = value.Replace(",", "");
                if (!AmountSplitter.AmountRegex.IsMatch(temp) && !string.IsNullOrEmpty(temp))
                {
                    return;
                }
                _amount = string.IsNullOrEmpty(temp) ? temp : AmountSplitter.Split(temp, 3);
                OnPropertyChanged(nameof(Amount));
                RemainingAmount = BigInteger.Parse(_amount.Replace(",", ""));
            }
        }

        public BigInteger RemainingAmount
        {
            get => _remainingAmount;
            set
            {
                _remainingAmount = value;
                OnPropertyChanged(nameof(RemainingAmount));

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



    }
}
