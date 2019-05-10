using System;
using System.Collections.ObjectModel;
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
            get =>_amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public override string ToString()
        {
            return
                $"project name = {ProjectName}\r\ncontractorName = {ContractorName}\r\ndate = {Date}\r\nnumber = {Number}\r\n amount = {Amount}";
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
                for (var i = 0; i < contractCollection.Count; i++)
                {
                    contractCollection[i].ItemContract.Index = $"{i + 1}";
                }
            };
            return contractCollection;
        }

        
    }
}
