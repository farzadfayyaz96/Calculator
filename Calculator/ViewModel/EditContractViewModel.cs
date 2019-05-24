using System;
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
            ContractSpecificationsControl = new ContractSpecificationsUserControl(itemContract);
            Control = ContractSpecificationsControl;
            FunctionsControl = new FunctionsUserControl(itemContract.Id);
            FunctionsControl.ViewModel.ClosePopupAction = RemovePopupAction;
            PaymentsControl = new PaymentsUserControl(itemContract);
            GeneralSituationControl = new GeneralSituationUserControl();
        }

        public Action<UserControl> AddPopupAction { get; set; }
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

        public GeneralSituationUserControl GeneralSituationControl { get; }

        public FunctionsUserControl FunctionsControl { get; }

        public PaymentsUserControl PaymentsControl { get; }
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                if (_selectedTabIndex == 0) Control = ContractSpecificationsControl;
                else if (_selectedTabIndex == 1) Control = FunctionsControl;
                else if (_selectedTabIndex == 2) Control = PaymentsControl;
            }
        }

        

       

    }
}
