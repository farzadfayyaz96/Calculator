
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.View
{
    public partial class PaymentsUserControl 
    {
        public PaymentsUserControl(Contract contract)
        {
            InitializeComponent();
            DataContext = new PaymentsViewModel(contract);
        }
    }
}
