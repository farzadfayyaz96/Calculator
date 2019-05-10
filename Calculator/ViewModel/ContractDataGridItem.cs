using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    class ContractDataGridItem
    {
        
        public ContractDataGridItem(Contract contract)
        {
            ItemContract = contract;
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
            });
        }

        private void Edit()
        {

        }

    }
}
