
using System;
using System.Numerics;
using Arash;
using Calculator.CustomException;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class Prepayment : NotifyProperty
    {
        private PersianDate _warrantyDate;
        private PrepaymentTask _taskLevelOne;
        private PrepaymentTask _taskLevelTwo;
        private PrepaymentTask _taskLevelThree;
        private string _prepaymentSum;
        private BigInteger _prepaymentSumBigInteger;
        public Prepayment(string prepaymentId)
        {
            Id = prepaymentId;
        }

        public PrepaymentTask TaskLevelOne
        {
            get=>_taskLevelOne;
            set
            {
                _taskLevelOne = value;
                OnPropertyChanged(nameof(TaskLevelOne));
            }
        }

        public PrepaymentTask TaskLevelTwo
        {
            get=>_taskLevelTwo;
            set
            {
                _taskLevelTwo = value;
                OnPropertyChanged(nameof(TaskLevelTwo));
            }
        }

        public PrepaymentTask TaskLevelThree
        {
            get=>_taskLevelThree;
            set
            {
                _taskLevelThree = value;
                OnPropertyChanged(nameof(TaskLevelThree));
            }
        }

        public PersianDate WarrantyDate
        {
            get => _warrantyDate;
            set
            {
                _warrantyDate = value;
                OnPropertyChanged(nameof(WarrantyDate));
            }
        }

        public BigInteger PrepaymentSumBigInteger
        {
            get=> _prepaymentSumBigInteger;
            set
            {
                _prepaymentSumBigInteger = value;
                OnPropertyChanged(nameof(PrepaymentSumBigInteger));
                PrepaymentSum = _prepaymentSumBigInteger == 0 ? string.Empty : _prepaymentSumBigInteger.ToString();
            }
        }
        public string PrepaymentSum
        {
            get => _prepaymentSum;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _prepaymentSum = value;
                    OnPropertyChanged(nameof(PrepaymentSum));
                    return;
                }
                var temp = value.Replace(",", "");
                if (!AmountSplitter.AmountRegex.IsMatch(temp) && !string.IsNullOrEmpty(temp))
                {
                    return;
                }
                _prepaymentSum = string.IsNullOrEmpty(temp) ? temp : AmountSplitter.Split(temp, 3);
                OnPropertyChanged(nameof(PrepaymentSum));

            }
        }

        public string Id { get; set; }

        public override string ToString()
        {
            return $"id ={Id}\r\ndate = {WarrantyDate}\r\n";
        }

        public void InitPrepaymentTasks(bool initFlag,Action<PersianDate,PersianDate,PersianDate> setDateAction)
        {
            if (initFlag)
            {
                TaskLevelOne = new PrepaymentTask(Id,"1");
                TaskLevelTwo = new PrepaymentTask(Id,"2");
                TaskLevelThree = new PrepaymentTask(Id,"3");
            }
            else
            {
                //get prepayment task level 1
                try
                {
                    TaskLevelOne = PrepaymentTasksDataAccess.SelectByPrepaymentIdAndLevel(Id, "1");
                }
                catch (ItemNotFoundException e)
                {
                    Logger.LogException(e);
                    TaskLevelOne = new PrepaymentTask(Id, "1")
                    {
                        Date = PersianDate.Today
                    };
                }
                //get prepayment task level 2
                try
                {
                    TaskLevelTwo = PrepaymentTasksDataAccess.SelectByPrepaymentIdAndLevel(Id, "2");
                }
                catch (ItemNotFoundException e)
                {
                    Logger.LogException(e);
                    TaskLevelTwo = new PrepaymentTask(Id, "2")
                    {
                        Date = PersianDate.Today
                    };
                }

                //get prepayment task level 3
                try
                {
                    TaskLevelThree = PrepaymentTasksDataAccess.SelectByPrepaymentIdAndLevel(Id, "3");
                }
                catch (ItemNotFoundException e)
                {
                    Logger.LogException(e);
                    TaskLevelThree = new PrepaymentTask(Id, "3")
                    {
                        Date = PersianDate.Today
                    };
                }
                UpdatePrepaymentSum();
            }

            setDateAction(TaskLevelOne.Date, TaskLevelTwo.Date, TaskLevelThree.Date);

        }

        public void UpdatePrepaymentSum()
        {
            PrepaymentSumBigInteger = TaskLevelOne.AmountBigInteger + TaskLevelTwo.AmountBigInteger + TaskLevelThree.AmountBigInteger;
        }
    }


}
