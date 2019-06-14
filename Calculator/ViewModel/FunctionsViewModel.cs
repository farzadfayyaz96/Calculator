using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    public class FunctionsViewModel : NotifyProperty
    {
        private bool _isEditMode;
        private bool _isErrorMode;
        private string _message;
        private int _selectedContractTypeIndex;
        public FunctionsViewModel(string contractId)
        {
            ItemFunction = new Function
            {
                ContractId = contractId,
                ContractType = "پیش پرداخت"
            };
            SaveCommand = new CommandHandler(Save);
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    FunctionCollection = FunctionDataAccess.SelectAll(contractId);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowMessage("خطا در حین بازیابی کارکرد ها",true);
                    return;
                }

                foreach (var functionDataGridItem in FunctionCollection)
                {
                    functionDataGridItem.DeleteAction = DeleteItem;
                    functionDataGridItem.EditAction = EditItem;
                }
                
            });
           
        }

        public Function ItemFunction { get; set; }
        public ICommand SaveCommand { get; set; }
        public Action<Function> AddFunctionAction { get; set; }
        public Action ClosePopupAction { get; set; }

        public ObservableCollection<FunctionDataGridItem> FunctionCollection { get; set; }

        public int SelectedContractTypeIndex
        {
            get => _selectedContractTypeIndex;
            set
            {
                _selectedContractTypeIndex = value;
                OnPropertyChanged(nameof(SelectedContractTypeIndex));
                if (_selectedContractTypeIndex == 0) ItemFunction.ContractType = "پیش پرداخت";
                else if (_selectedContractTypeIndex == 1) ItemFunction.ContractType = "موقت";
                else if (_selectedContractTypeIndex == 2) ItemFunction.ContractType = "قطعی";
                else if (_selectedContractTypeIndex == 3) ItemFunction.ContractType = "تعدیل";
                else if (_selectedContractTypeIndex == 4) ItemFunction.ContractType = "سپرده";
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
            }
        }

        public bool IsErrorMode
        {
            get => _isErrorMode;
            set
            {
                _isErrorMode = value;
                OnPropertyChanged(nameof(IsErrorMode));
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

        private void ShowMessage(string message,bool isError)
        {
            IsErrorMode = isError;
            Message = message;
        }

        private void Save()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //check
                if (string.IsNullOrEmpty(ItemFunction.Amount))
                {
                    ShowMessage("مقدار مبلغ قرارداد را وارد کنید",true);
                    return;
                }

                //compare contract date to now date
                var dateCompareResult = DateTime.Compare(DateTime.Now, ItemFunction.Date.ToDateTime());
                if (dateCompareResult < 0)
                {
                    ShowMessage("تاریخ قرارداد صحیح نمی باشد",true);
                    return;
                }

                //add new function id
                ItemFunction.Id = Guid.NewGuid().ToString();
                try
                {
                    FunctionDataAccess.Insert(ItemFunction);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowMessage("خطا در حین دخیره کارکرد جدید",true);
                    return;
                }
                //add to data grid
                var functionDataGridItem = new FunctionDataGridItem(ItemFunction)
                {
                    EditAction = EditItem,
                    DeleteAction = DeleteItem,
                };
                FunctionCollection.Add(functionDataGridItem);
                //add to general situation 
                AddFunctionAction(functionDataGridItem.ItemFunction);
                ShowMessage("کارکرد جدید با موفقیت ذخیره شد",false);
                //claer
                ItemFunction.Clear();
                SelectedContractTypeIndex = 0;
            });
            
            

        }

        private void DeleteItem(FunctionDataGridItem function)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    //delete from database
                    FunctionDataAccess.Delete(function.ItemFunction.ContractId);
                    //delete from data gird
                    FunctionCollection.Remove(function);
                    ShowMessage("کارکرد با موفقیت حذف شد",false);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowMessage("خطا در حین حذف کارکرد جدید", true);
                }

                
            });
        }


        private void EditItem(FunctionDataGridItem function)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    //update database
                    FunctionDataAccess.Update(function.ItemFunction);
                    //update data grid
                    foreach (var functionDataGridItem in FunctionCollection)
                    {
                        if (!functionDataGridItem.Equals(function))continue;
                        functionDataGridItem.ItemFunction.ContractType = function.ItemFunction.ContractType;
                        functionDataGridItem.ItemFunction.Amount = functionDataGridItem.ItemFunction.Amount;
                        functionDataGridItem.ItemFunction.Date = functionDataGridItem.ItemFunction.Date;
                        break;
                    }
                    ShowMessage("کارکرد با موفقیت به روزرسانی شد",false);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                    ShowMessage("خطا در حین به روز رسانی کارکرد",true);
                }
            });
        }
    }

    
}
