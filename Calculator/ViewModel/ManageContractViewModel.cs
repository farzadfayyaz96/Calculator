using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Calculator.Log;
using Calculator.Model.DataAccess;
using Calculator.View;

namespace Calculator.ViewModel
{
    class ManageContractViewModel : NotifyProperty
    {
        private string _searchText;
        private ObservableCollection<ContractDataGridItem> _contractCollection;
        private string _message;
        private bool _progressBarIsEnable;
        private bool _isProjectName;
        private bool _isContractorName;
        private bool _isContractNumber;
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
            IsProjectName = true;
            EditContractWindowList = new List<EditContractWindow>();
        }

        public static ManageContractViewModel Instance { get; } = new ManageContractViewModel();

        public List<EditContractWindow> EditContractWindowList { get; } 

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public bool IsProjectName
        {
            get => _isProjectName;
            set
            {
                _isProjectName = value;
                OnPropertyChanged(nameof(IsProjectName));
            }
        }

        public bool IsContractorName
        {
            get => _isContractorName;
            set
            {
                _isContractorName = value;
                OnPropertyChanged(nameof(IsContractorName));
            }
        }

        public bool IsContractNumber
        {
            get => _isContractNumber;
            set
            {
                _isContractNumber = value;
                OnPropertyChanged(nameof(IsContractNumber));
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
                        var type = IsProjectName ? 1 : IsContractorName ? 2 : 3;
                        ContractCollection = ContractDataAccess.Search(SearchText,type);
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
