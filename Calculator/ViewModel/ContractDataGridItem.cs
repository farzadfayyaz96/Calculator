﻿using System;
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
                ContractType = contract.ContractType,
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
            var dialog = new DialogUserControl(message,Delete);
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

        }



    }
}
