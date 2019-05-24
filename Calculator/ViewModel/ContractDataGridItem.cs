using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;
using Calculator.View;

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
                Index = contract.Index,
                Number = contract.Number
            };
            EditCommand = new CommandHandler(Edit);
            DeleteCommand = new CommandHandler(ShowDeleteDialog);
        }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public Contract ItemContract { get; }

        private void ShowDeleteDialog()
        {
            //init dialog for delete contract
            var message = $"آیا مایل به حذف \"{ItemContract.ProjectName}\" می باشید؟";
            var dialog = new DialogUserControl(message,Delete,MainViewModel.Instance.RemovePopupAction);
            //add popup
            MainViewModel.Instance.AddPopupAction(dialog);
        }

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                //searching for exist edit contract windows
                var list = ManageContractViewModel.Instance.EditContractWindowList;
                foreach (var window in list)
                {
                    if (!window.ViewModel.FunctionsControl.ViewModel.ItemFunction.ContractId.Equals(ItemContract.Id))
                    {
                        //not found so continue
                        continue;
                    }
                    //found it so active it and get out 
                    window.Activate();
                    return;
                }
                //not exist so make new windows and add it to list
                var editContractWindow = new EditContractWindow(ItemContract);
                editContractWindow.Show();
                list.Add(editContractWindow);
            });
            

        }



    }
}
