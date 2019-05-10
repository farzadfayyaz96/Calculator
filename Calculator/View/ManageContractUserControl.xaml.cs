using System.Windows.Controls;

namespace Calculator.View
{
    
    public partial class ManageContractUserControl 
    {
        public ManageContractUserControl()
        {
            InitializeComponent();
        }

        private void DataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ContractDataGrid.UnselectAll();
        }
    }
}
