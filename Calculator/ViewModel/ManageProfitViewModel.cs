using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Arash;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    class ManageProfitViewModel : NotifyProperty
    {
        private int _profitListSelectedIndex;
        private string _monthName;
        private int _year;
        private string _interestRates;
        private string _message;
        private bool _isError;
        private ObservableCollection<Profit> _profitCollection;

        public ManageProfitViewModel()
        {

            Year = new PersianDate(DateTime.Now).Year;
            RightCommand = new CommandHandler(Right);
            LeftCommand = new CommandHandler(Left);
            ProfitCollection = new ObservableCollection<Profit>();
            for (var i = 0; i < 12; i++)
            {
                var profit = new Profit($"{i + 1}");
                ProfitCollection.Add(profit);
            }
            //
            InitProfits(Year);
            //
            ProfitListSelectedIndex = 0;
            SaveCommand = new CommandHandler(Save);
        }

        public ICommand SaveCommand { get; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged(nameof(IsError));
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

        public ObservableCollection<Profit> ProfitCollection
        {
            get => _profitCollection;
            set
            {
                _profitCollection = value;
                OnPropertyChanged(nameof(ProfitCollection));
            }
        }

        public ICommand RightCommand { get; }

        public ICommand LeftCommand { get; }

        public int ProfitListSelectedIndex
        {
            get => _profitListSelectedIndex;
            set
            {
                _profitListSelectedIndex = value;
                OnPropertyChanged(nameof(ProfitListSelectedIndex));
                var profit = ProfitCollection[_profitListSelectedIndex];
                MonthName = profit.MonthName;
                InterestRates = profit.InterestRates.Equals("نامشخص") ? string.Empty : profit.InterestRates;
            }
        }

        public int Year
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

        private void Left()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Year < 0) return;
                Year--;
                InitProfits(Year);
            });

        }

        private void Right()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Year++;
                InitProfits(Year);
            });
        }

        private void InitProfits(int year)
        {
            //init new year to profit array 
            foreach (var profit in ProfitCollection)
            {
                profit.Year = $"{year}";
            }
            //load profits from database
            var list = ProfitDateAccess.SelectByYear(year);
            foreach (var profit in ProfitCollection)
            {
                var result = list.Where(x => x.Month == profit.Month).ToList();
                profit.InterestRates = result.Any() ? result[0].InterestRates : "نامشخص";
            }

            foreach (var profit in list)
            {
                try
                {
                    //cast month number to int
                    var monthInt = int.Parse(profit.Month) -1;
                    //set Interest Rates profit
                    ProfitCollection[monthInt].InterestRates = profit.InterestRates;
                }
                catch (FormatException e)
                {
                    Logger.LogException(e);
                }
            }
            //
            var pr = ProfitCollection[_profitListSelectedIndex];
            InterestRates = pr.InterestRates.Equals("نامشخص") ? string.Empty : pr.InterestRates;
        }

        private void Save()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (string.IsNullOrEmpty(InterestRates))
                {
                    ShowError("مقدار درصد سود را وارد کنید.");
                    return;
                }

              try
              {
                    var year = $"{Year}";
                    var month = $"{ProfitListSelectedIndex + 1}";
                    var profit = new Profit(year,month,InterestRates);
                    ProfitDateAccess.InsertOrUpdate(profit);
                    ShowMessage("مبلغ سود با موفقیت ذخیره شد");
                    //replace data to view 
                    ProfitCollection[ProfitListSelectedIndex].InterestRates = profit.InterestRates;
              }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowError("خطا در هنگام ذخیره مبلغ سود");
                }

            });

        }

        private void ShowError(string errorMessage)
        {
            IsError = true;
            Message = errorMessage;
        }

        private void ShowMessage(string message)
        {
            IsError = false;
            Message = message;
        }


    }
}
