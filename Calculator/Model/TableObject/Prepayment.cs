
using System;
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

        public Prepayment(string prepaymentId)
        {
            Id = prepaymentId;
            

        }

        public PrepaymentTask TaskLevelOne { get; set; }
        public PrepaymentTask TaskLevelTwo { get; set; }
        public PrepaymentTask TaskLevelThree { get; set; }

        public PersianDate WarrantyDate
        {
            get => _warrantyDate;
            set
            {
                _warrantyDate = value;
                OnPropertyChanged(nameof(WarrantyDate));
            }
        }


        public string Id { get; set; }

        public override string ToString()
        {
            return $"id ={Id}\r\ndate = {WarrantyDate}\r\n";
        }

        public void InitPrepaymentTasks()
        {
            //get prepayment task level 1
            try
            {
                TaskLevelOne = PrepaymentTasksDataAccess.SelectByPrepaymentIdAndLevel(Id, "1");
            }
            catch (ItemNotFoundException e)
            {
                Logger.LogException(e);
                TaskLevelOne = new PrepaymentTask
                {
                    PrepaymentId = Id,
                    Level = "1",
                    Date = PersianDate.Today
                };
            }
            ////get prepayment task level 2
            //try
            //{
            //    TaskLevelTwo = PrepaymentTasksDataAccess.SelectByPrepaymentIdAndLevel(prepaymentId, "2");
            //}
            //catch (ItemNotFoundException e)
            //{
            //    Logger.LogException(e);
            //    TaskLevelTwo = new PrepaymentTask
            //    {
            //        PrepaymentId = prepaymentId,
            //        Level = "2",
            //        Date = PersianDate.Today
            //    };
            //}

            ////get prepayment task level 3
            //try
            //{
            //    TaskLevelThree = PrepaymentTasksDataAccess.SelectByPrepaymentIdAndLevel(prepaymentId, "3");
            //}
            //catch (ItemNotFoundException e)
            //{
            //    Logger.LogException(e);
            //    TaskLevelThree = new PrepaymentTask
            //    {
            //        PrepaymentId = prepaymentId,
            //        Level = "3",
            //        Date = PersianDate.Today
            //    };
            //}
        }
    }
}
