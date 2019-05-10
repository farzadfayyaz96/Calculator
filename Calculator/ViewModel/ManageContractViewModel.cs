using System;
using System.Collections.ObjectModel;
using System.Windows;
using Calculator.Log;
using Calculator.Model.DataAccess;

namespace Calculator.ViewModel
{
    class ManageContractViewModel : NotifyProperty
    {
        private string _searchText;
        private ObservableCollection<ContractDataGridItem> _contractCollection;
        private string _message;
        private bool _progressBarIsEnable;
        private ManageContractViewModel()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    ContractCollection = ContractDataAccess.SelectAll();
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            });

        }

        public static ManageContractViewModel Instance { get; } = new ManageContractViewModel();

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
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
        public ObservableCollection<ContractDataGridItem> ContractCollection
        {
            get => _contractCollection;
            set
            {
                _contractCollection = value;
                OnPropertyChanged(nameof(ContractCollection));
                Message = string.Empty;
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    //check length of text 
                    if (_searchText.Length <= 2)
                    {
                        if (!_searchText.Equals(string.Empty)) return;
                        //show progress bar
                        ProgressBarIsEnable = true;
                        try
                        {
                            ContractCollection = ContractDataAccess.SelectAll();
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                            Message = "خطا در هنگام بازیابی اطلاعات";
                        }
                        finally
                        {
                            //hide progress bar
                            ProgressBarIsEnable = false;
                        }
                        return;
                    }
                    //show progress bar
                    ProgressBarIsEnable = true;
                    try
                    {
                        ContractCollection = ContractDataAccess.Search(SearchText);
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                        Message = "خطا در هنگام بازیابی اطلاعات";
                    }
                    finally
                    {
                        //hide progress bar
                        ProgressBarIsEnable = false;
                    }
                });
                
            }
        }
    }
}
