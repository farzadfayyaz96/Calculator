using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    class ContractSpecificationsViewModel:NotifyProperty
    {
        private bool _progressBarIsEnable;
        private bool _isError;
        private string _message;
        public ContractSpecificationsViewModel(Contract contract)
        {

            EditContract = contract;
            SaveCommand = new CommandHandler(Save);
        }

        public Contract EditContract { get; set; }
        public ICommand SaveCommand { get; }
        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }
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
        private void Save()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    ProgressBarIsEnable = true;
                    ContractDataAccess.Update(EditContract);
                    //update view 
                    var list = ManageContractViewModel.Instance.ContractCollection;
                    foreach (var contractDataGridItem in list)
                    {
                        if (!contractDataGridItem.ItemContract.Id.Equals(EditContract.Id)) continue;
                        var dataGridContract = contractDataGridItem.ItemContract;
                        dataGridContract.ProjectName = EditContract.ProjectName;
                        dataGridContract.ContractorName = EditContract.ContractorName;
                        dataGridContract.Id = EditContract.Id;
                        dataGridContract.Date = EditContract.Date;
                        dataGridContract.Amount = EditContract.Amount;
                        dataGridContract.Index = EditContract.Index;
                        dataGridContract.Number = EditContract.Number;
                        break;
                    }
                    ShowMessage("پیمان با موفقیت به روزرسانی شد");
                    ProgressBarIsEnable = false;
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowError("خطا در بروز رسانی پیمان");
                    ProgressBarIsEnable = false;
                }

            });
        }

        private void ShowError(string message)
        {
            Message = message;
            IsError = true;
        }

        private void ShowMessage(string message)
        {
            Message = message;
            IsError = false;
        }
    }
}
