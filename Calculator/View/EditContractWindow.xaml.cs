
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.View
{

    public partial class EditContractWindow 
    {
        public EditContractWindow(Contract contract)
        {
            InitializeComponent();
            DataContext = new EditContractViewModel(contract);
        }
    }
}
