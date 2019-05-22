using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    class NewContractViewModel : NotifyProperty
    {
        private bool _progressBarIsEnable;
        private string _message;
        public NewContractViewModel()
        {
            MakeContractCommand = new CommandHandler(MakeContract);
            NewContract = new Contract();
            
        }

        public Contract NewContract { get; set; }

        public ICommand MakeContractCommand { get; }


        public bool ProgressBarIsEnable
        {
            get => _progressBarIsEnable;
            set
            {
                _progressBarIsEnable = value;
                OnPropertyChanged(nameof(ProgressBarIsEnable));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private void MakeContract()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (string.IsNullOrEmpty(NewContract.ProjectName))
                {
                    ShowError("نام پروژه را وارد کنید");
                    return;
                }

                if (string.IsNullOrEmpty(NewContract.ContractorName))
                {
                    ShowError("نام پیمانکار را وارد کنید");
                    return;
                }
                //compare contract date to now date
                var dateCompareResult = DateTime.Compare(DateTime.Now, NewContract.Date.ToDateTime());
                if (dateCompareResult < 0)
                {
                    ShowError("تاریخ پیمان صحیح نمی باشد");
                    return;
                }

                if (string.IsNullOrEmpty(NewContract.Number))
                {
                    ShowError("شماره پیمان را وارد کنید");
                    return;
                }

                if (string.IsNullOrEmpty(NewContract.Amount))
                {
                    ShowError("مبلغ  پیمان را وارد کنید");
                    return;
                }
                //show progress bar
                ProgressBarIsEnable = true;
                //show message
                Message = "لطفا صبر کنید";
                //add new guid to new contract
                NewContract.Id = Guid.NewGuid().ToString();
                try
                {
                     ContractDataAccess.Insert(NewContract);
                     //add to manage contract data grid
                     ManageContractViewModel.Instance.ContractCollection.Add(new ContractDataGridItem(NewContract));
                     Message = "پیمان جدید با موفقیت ذخیره شد";
                     ProgressBarIsEnable = false;
                    //clear form
                    NewContract.Clear();
                    NewContract = new Contract();
                     
                     
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowError("خطایی در هنگام درج پیمان رخ داده است");
                }
            });//end task

        }

        private void ShowError(string errorMessage)
        {
            ProgressBarIsEnable = false;
            Message = errorMessage;
        }
        
    }
}
