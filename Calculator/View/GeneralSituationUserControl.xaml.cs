

using System.Windows.Controls;
using Calculator.ViewModel;

namespace Calculator.View
{
  
    public partial class GeneralSituationUserControl 
    {

        public GeneralSituationUserControl()
        {
            InitializeComponent();
            ViewModel = new GeneralSituationViewModel();
            DataContext = ViewModel;
        }

        public GeneralSituationViewModel ViewModel { get; }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid.UnselectAll();
            
        }
    }
}
