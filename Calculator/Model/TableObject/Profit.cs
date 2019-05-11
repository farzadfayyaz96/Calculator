using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    class Profit : NotifyProperty
    {
        private string _month;
        private string _year;
        private string _interestRates;

        public Profit(string year, string month, string interestRates)
        {
            Year = year;
            Month = month;
            InterestRates = interestRates;
        }

        public string Month
        {
            get => _month;
            set
            {
                _month = value;
                OnPropertyChanged(nameof(Month));
            }
        }

        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        public string InterestRates
        {
            get => _interestRates;
            set
            {
                _interestRates = value;
                OnPropertyChanged(nameof(InterestRates));
            }
        }

        public override string ToString()
        {
            return $"year = {Year}\r\nmonth = {Month}\r\nInterestRates = {InterestRates}";
        }
    }
}
