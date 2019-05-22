using Calculator.Model.TableObject;
using Calculator.View;

namespace Calculator.ViewModel
{
    class EditContractViewModel : NotifyProperty
    {
       

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

            ContractSpecificationsUserControl = new ContractSpecificationsUserControl(itemContract);

        }


        public  ContractSpecificationsUserControl ContractSpecificationsUserControl { get; }
        


        

       

    }
}
