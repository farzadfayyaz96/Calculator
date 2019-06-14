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
            ContractSpecificationsControl = new ContractSpecificationsUserControl(itemContract);
            Control = ContractSpecificationsControl;
            FunctionsControl = new FunctionsUserControl(itemContract.Id);
            FunctionsControl.ViewModel.ClosePopupAction = RemovePopupAction;
            PaymentsControl = new PaymentsUserControl(itemContract);
            GeneralSituationControl = new GeneralSituationUserControl();
            var functions = FunctionsControl.ViewModel.FunctionCollection.Select(x => x.ItemFunction);
            GeneralSituationControl.ViewModel.AddCollection(functions);
            FunctionsControl.ViewModel.AddFunctionAction = GeneralSituationControl.ViewModel.AddFunction;
            var payments = PaymentsControl.ViewModel.PaymentCollection.Select(x => x.ItemPayment);
            GeneralSituationControl.ViewModel.AddCollection(payments);
            PaymentsControl.ViewModel.AddPaymentAction = GeneralSituationControl.ViewModel.AddPayment;

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
                else if (_selectedTabIndex == 3) Control = GeneralSituationControl;
            }
        }

        

       

    }
}
