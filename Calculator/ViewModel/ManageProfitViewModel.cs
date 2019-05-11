using System;
using System.Windows.Input;
using Arash;

namespace Calculator.ViewModel
{
    class ManageProfitViewModel : NotifyProperty
    {

        
        private int _year;
        private string _interestRates;
        public ManageProfitViewModel()
        {
           
            Year = new PersianDate(DateTime.Now).Year;
            RightCommand = new CommandHandler(Right);
            LeftCommand = new CommandHandler(Left);
        }

        public ICommand RightCommand { get; }

        public ICommand LeftCommand { get; }

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
            if(Year < 0) return;
            Year--;
        }

        private void Right()
        {
            Year++;
        }




    }
}
