using System;
using System.Collections.ObjectModel;
using System.Windows;
using Arash;
using Calculator.ViewModel;

namespace Calculator.Model.TableObject
{
    public class Contract :NotifyProperty
    {
        private string _index;
        private string _projectName;
        private string _contractorName;
        private PersianDate _date;
        private string _number;
        private string _amount;

        public string Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }
        public string Id { get; set; }

        public string PrepaymentId { get; set; }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        public string ContractorName
        {
            get => _contractorName;
            set
            {
                _contractorName = value;
                OnPropertyChanged(nameof(ContractorName));
            }
        }

        public PersianDate Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public string Number
        {
            get => _number;
            set
            {
                
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public string Amount
        {
            get=> _amount; 
            set
            {
                var temp = value.Replace(",", "");
                if (!AmountSplitter.AmountRegex.IsMatch(temp) && !string.IsNullOrEmpty(temp))
                {
                    return;
                }
                _amount = string.IsNullOrEmpty(temp) ? temp : AmountSplitter.Split(temp, 3);
                OnPropertyChanged(nameof(Amount));
            }
        }


        public void Clear()
        {
            Index = string.Empty;
            Id = string.Empty;
            ProjectName = string.Empty;
            ContractorName = string.Empty;
            //set date to now
            Date = new PersianDate(DateTime.Now);
            Number = string.Empty;
            Amount = string.Empty;

        }

        public override string ToString()
        {
            return
                $"project name = {ProjectName}\r\ncontractorName = {ContractorName}\r\ndate = {Date}\r\nnumber = {Number}\r\namount = {Amount}\r\nPrepayment Id = {PrepaymentId}";
        }

        public override bool Equals(object obj)
        {
            try
            {
                var contract = (Contract) obj;
                return contract != null && contract.Id.Equals(Id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static ObservableCollection<ContractDataGridItem> GetObservableCollection()
        {

            var contractCollection = new ObservableCollection<ContractDataGridItem>();
            contractCollection.CollectionChanged += (sender, args) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    for (var i = 0; i < contractCollection.Count; i++)
                    {
                        contractCollection[i].ItemContract.Index = $"{i + 1}";
                    }
                });
            };
            return contractCollection;
        }
    }
}
