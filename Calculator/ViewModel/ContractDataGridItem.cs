using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    public class ContractDataGridItem
    {
        
        public ContractDataGridItem(Contract contract)
        {
            ItemContract = new Contract
            {
                Id = contract.Id,
                ProjectName = contract.ProjectName,
                ContractorName = contract.ContractorName,
                Date =  contract.Date,
                Amount = contract.Amount,
                ContractType = contract.ContractType,
                Index = contract.Index,
                Number = contract.Number
            };
            EditCommand = new CommandHandler(Edit);
            DeleteCommand = new CommandHandler(Delete);
        }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public Contract ItemContract { get; }

        private void Delete()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //show progress bar 
                ManageContractViewModel.Instance.ProgressBarIsEnable = true;
                try
                {
                    //remove from database
                    ContractDataAccess.Delete(ItemContract.Id);
                    //remove from data grid
                    ManageContractViewModel.Instance.ContractCollection.Remove(this);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ManageContractViewModel.Instance.Message = "خطا در هنگام حذف پیمان";
                }
                finally
                {
                    //hide progress bar
                    ManageContractViewModel.Instance.ProgressBarIsEnable = false;
                }
            });
        }

        private void Edit()
        {

        }

    }
}
