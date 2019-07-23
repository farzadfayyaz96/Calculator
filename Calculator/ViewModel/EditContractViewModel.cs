using System;
using System.Linq;
using System.Windows.Controls;
using Calculator.Model.TableObject;
using Calculator.View;

namespace Calculator.ViewModel
{
    public class EditContractViewModel : NotifyProperty
    {

        private int _selectedTabIndex;
        private UserControl _control;
        public EditContractViewModel(Contract contract)
        {
            var itemContract = new Contract
            {
                ProjectName = contract.ProjectName,
                ContractorName = contract.ContractorName,
                Id = contract.Id,
                Date = contract.Date,
                Amount = contract.Amount,
                Index = contract.Index,
                Number = contract.Number
            };
            ContractId = itemContract.Id;
            ContractSpecificationsControl = new ContractSpecificationsUserControl(itemContract);
            Control = ContractSpecificationsControl;
            PrepaymentControl = new PrepaymentUserControl(contract.PrepaymentId) {ViewModel = {ContractId = ContractId}};
        }

        public Action<UserControl> AddPopupAction { get; set; }

        public string ContractId { get; }
        public Action RemovePopupAction { get; set; }
        public UserControl Control
        {
            get => _control;
            set
            {
                _control = value;
                OnPropertyChanged(nameof(Control));
            }
        }
        public ContractSpecificationsUserControl ContractSpecificationsControl { get; }

        public PrepaymentUserControl PrepaymentControl { get; }


        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                if (_selectedTabIndex == 0) Control = ContractSpecificationsControl;
                else if (_selectedTabIndex == 1) Control = PrepaymentControl;
               
            }
        }
    }
}
