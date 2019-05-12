using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private ContractType _contractType;
        private string _contractTypeString;

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

        public ContractType ContractType
        {
            get => _contractType;
            set
            {
                _contractType = value;
                OnPropertyChanged(nameof(ContractType));
                if (_contractType == ContractType.Balancing) ContractTypeString = "تعدیل";
                else if (_contractType == ContractType.Certain) ContractTypeString = "قطعی";
                else if (_contractType == ContractType.Deposit) ContractTypeString = "سپرده";
                else if (_contractType == ContractType.Prepayment) ContractTypeString = "پیش پرداخت";
                else if (_contractType == ContractType.Temporary) ContractTypeString = "موقت";
            }
        }

        public string ContractTypeString
        {
            get => _contractTypeString;
            set
            {
                _contractTypeString = value;
                OnPropertyChanged(nameof(ContractTypeString));
            }
        }

        public void SetContractType(string type)
        {
            if (type.Equals("0")) ContractType = ContractType.Prepayment;
            else if (type.Equals("1")) ContractType = ContractType.Temporary;
            else if (type.Equals("2")) ContractType = ContractType.Certain;
            else if (type.Equals("3")) ContractType = ContractType.Balancing;
            else if (type.Equals("4")) ContractType = ContractType.Deposit;
        }

        public string GetContractTypeValue()
        {
            if (ContractType == ContractType.Prepayment) return "0";
            if (ContractType == ContractType.Temporary) return "1";
            if (ContractType == ContractType.Certain) return "2";
            if (ContractType == ContractType.Balancing) return "3";
            return "4";
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
            //contract type to default
            ContractType = ContractType.Prepayment;
            ContractTypeString = string.Empty;
            
        }

        public override string ToString()
        {
            return
                $"project name = {ProjectName}\r\ncontractorName = {ContractorName}\r\ndate = {Date}\r\nnumber = {Number}\r\namount = {Amount}";
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
