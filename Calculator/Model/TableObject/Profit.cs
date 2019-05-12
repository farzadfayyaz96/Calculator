using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    class Profit : NotifyProperty
    {
        private string _month;
        private string _year;
        private string _interestRates;
        private string _monthName;

        public Profit(string year, string month, string interestRates)
        {
            Year = year;
            Month = month;
            InterestRates = interestRates;
        }

        public Profit(string month)
        {
            Month = month;
            if (month.Equals("1")) MonthName = "فروردین";
            else if (month.Equals("2")) MonthName = "اردیبهشت";
            else if (month.Equals("3")) MonthName = "خرداد";
            else if (month.Equals("4")) MonthName = "تیر";
            else if (month.Equals("5")) MonthName = "مرداد";
            else if (month.Equals("6")) MonthName = "شهرویور";
            else if (month.Equals("7")) MonthName = "مهر";
            else if (month.Equals("8")) MonthName = "آبان";
            else if (month.Equals("9")) MonthName = "آذر";
            else if (month.Equals("10")) MonthName = "دی";
            else if (month.Equals("11")) MonthName = "بهمن";
            else if (month.Equals("12")) MonthName = "اسفند";
        }

        public Profit() { }

        public string Month
        {
            get => _month;
            set
            {
                _month = value;
                OnPropertyChanged(nameof(Month));
                
            }
        }

        public string MonthName
        {
            get => _monthName;
            set
            {
                _monthName = value;
                OnPropertyChanged(nameof(MonthName));
                
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
