using System.Numerics;
using Arash;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class Payment : NotifyProperty
    {

        private string _amount;
        private PersianDate _date;
        private bool _isExistInDatabase;
        private bool _isInsertModel;
        private bool _isUpdateMode;

        public Payment(string id,string prepaymentId)
        {
            Id = id;
            PrepaymentId = prepaymentId;
        }

        public string Id { get; }
        public string PrepaymentId { get; }

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

        public BigInteger AmountBigInteger => string.IsNullOrEmpty(Amount) ? 0 : BigInteger.Parse(Amount.Replace(",",string.Empty));

        public bool IsExistInDatabase
        {
            get => _isExistInDatabase;
            set
            {
                _isExistInDatabase = value;
                OnPropertyChanged(nameof(IsExistInDatabase));
            }
        }

        public bool IsInsetMode
        {
            get => _isInsertModel;
            set
            {
                _isInsertModel = value;
                OnPropertyChanged(nameof(IsInsetMode));
            }
        }

        public bool IsUpdateMode
        {
            get => _isUpdateMode;
            set
            {
                _isUpdateMode = value;
                OnPropertyChanged(nameof(IsUpdateMode));
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

        public override string ToString()
        {
            return $"id = {Id}\r\nprepayment id = {PrepaymentId}\r\namount = {Amount}\r\ndate = {Date}";
        }

    }
}
