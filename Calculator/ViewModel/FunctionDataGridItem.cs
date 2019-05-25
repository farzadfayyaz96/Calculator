using System;
using System.Windows;
using System.Windows.Input;
using Calculator.Model.TableObject;
using Calculator.View;

namespace Calculator.ViewModel
{
    public class FunctionDataGridItem : NotifyProperty
    {
        public FunctionDataGridItem(Function function)
        {
            ItemFunction = new Function
            {
                ContractType = function.ContractType,
                Amount = function.Amount,
                ContractId = function.ContractId,
                Id = function.Id,
                Date = function.Date
            };
            DeleteCommand = new CommandHandler(ShowDeleteDialog);
            EditCommand = new CommandHandler(()=> EditAction(this));
        }

        public Action<FunctionDataGridItem> EditAction { get; set; }

        public Action<FunctionDataGridItem> DeleteAction { get; set; }


        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public Function ItemFunction { get; }

        private void ShowDeleteDialog()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //get contract windows
                var contractWindowList = ManageContractViewModel.Instance.EditContractWindowList;
                EditContractWindow editWindow = null;
                foreach (var window in contractWindowList)
                {
                    if (!window.ViewModel.FunctionsControl.ViewModel.ItemFunction.ContractId.Equals(ItemFunction.ContractId))
                    {
                        //not found yet!!!
                        continue;
                    }
                    //found it and add new popup dialog to windows
                    editWindow = window;
                    break;
                }
                //init dialog for delete contract
                var message = $"آیا مایل به حذف کارکرد به مبلغ '{ItemFunction.Amount}' و از نوع '{ItemFunction.ContractType}' می باشید؟";
                var dialog = new DialogUserControl(message, () => DeleteAction(this),editWindow?.ViewModel.RemovePopupAction);
                editWindow?.ViewModel.AddPopupAction(dialog);

            });
            
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            try
            {
                var funcItem = (FunctionDataGridItem)obj;
                return funcItem.ItemFunction.Equals(ItemFunction);
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
