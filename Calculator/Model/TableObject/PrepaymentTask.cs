using System.Numerics;
using Arash;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class PrepaymentTask :NotifyProperty
    {
        private string _amount;
        private PersianDate _date;
        private bool _isInsertMode;
        private bool _isUpdateMode;
        private bool _isExistInDatabase;
        public PrepaymentTask(string prepaymentId,string level)
        {
            PrepaymentId = prepaymentId;
            Level = level;
            Date = PersianDate.Today;
        }

        public string PrepaymentId { get; set; }
        public string Level { get; set; }

        public bool IsInsertMode
        {
            get => _isInsertMode;
            set
            {
                _isInsertMode = value;
                OnPropertyChanged(nameof(IsInsertMode));
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

        public bool IsExistInDatabase
        {
            get => _isExistInDatabase;
            set
            {
                _isExistInDatabase = value;
                OnPropertyChanged(nameof(IsExistInDatabase));
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

        public BigInteger AmountBigInteger=>string.IsNullOrEmpty(Amount) ? 0 : BigInteger.Parse(Amount.Replace(",",string.Empty));

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
            return $"prepayment id = {PrepaymentId}\r\nlevel = {Level}\r\namount = {Amount}\r\ndate = {Date}";
        }

    }
}
