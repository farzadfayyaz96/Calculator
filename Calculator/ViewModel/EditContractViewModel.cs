using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    class EditContractViewModel : NotifyProperty
    {
        private int _selectedContractTypeIndex;
        private bool _isError;
        private string _message;

        public EditContractViewModel(Contract contract)
        {
            ItemContract = new Contract
            {
                ContractType = contract.ContractType,
                ProjectName = contract.ProjectName,
                ContractorName = contract.ContractorName,
                Id = contract.Id,
                Date = contract.Date,
                Amount = contract.Amount,
                Index = contract.Index,
                Number = contract.Number
            };
            SaveCommand = new CommandHandler(Save);
            _selectedContractTypeIndex = contract.GetSelectedContractTypeIndex();
        }

        public Contract ItemContract { get; set; }
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

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public int SelectedContractTypeIndex
        {
            get => _selectedContractTypeIndex;
            set
            {
                _selectedContractTypeIndex = value;
                OnPropertyChanged(nameof(SelectedContractTypeIndex));
                //not selected
                if (_selectedContractTypeIndex == -1) return;
                //set contract type
                ItemContract.SetContractType($"{_selectedContractTypeIndex}");

            }
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

        private void Save()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    ContractDataAccess.Update(ItemContract);
                    //update view 
                    var list = ManageContractViewModel.Instance.ContractCollection;
                    foreach (var contractDataGridItem in list)
                    {
                        if (!contractDataGridItem.ItemContract.Id.Equals(ItemContract.Id)) continue;
                        var dataGridContract = contractDataGridItem.ItemContract;
                        dataGridContract.ContractType = ItemContract.ContractType;
                        dataGridContract.ProjectName = ItemContract.ProjectName;
                        dataGridContract.ContractorName = ItemContract.ContractorName;
                        dataGridContract.Id = ItemContract.Id;
                        dataGridContract.Date = ItemContract.Date;
                        dataGridContract.Amount = ItemContract.Amount;
                        dataGridContract.Index = ItemContract.Index;
                        dataGridContract.Number = ItemContract.Number;
                        break;
                    }
                    ShowMessage("پیمان با موفقیت به روزرسانی شد");
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowError("خطا در بروز رسانی پیمان");
                }
                
            });
        }

    }
}
