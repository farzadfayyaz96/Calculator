using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
            get=> _amount; 
            set
            {
                var regex = new Regex("^[0-9]\\d*$");
                if (!regex.IsMatch(value) && !string.IsNullOrEmpty(value))
                {
                    return;
                }
                _amount = value;

                OnPropertyChanged(nameof(Amount));
                OnPropertyChanged(nameof(AmountWithComa));
            }
        }

        public string AmountWithComa => Split(Amount, 3);


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

        public static string Split(string str, int chunkSize)
        {
            var temp = string.Empty;
            var counter = 1;
            for (var i = str.Length - 1; i >= 0; i--)
            {
                var coma = counter % chunkSize == 0 && counter != str.Length ? "," : string.Empty;
                counter++;
                temp = $"{coma}{str[i]}{temp}";
            }
            return temp;
        }


    }
}
