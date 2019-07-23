﻿using System.Numerics;
using Arash;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class Payment : NotifyProperty
    {

        private string _amount;
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
