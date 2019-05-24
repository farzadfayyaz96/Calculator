using System.Windows.Controls;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.View
{

    public partial class EditContractWindow
    {
        public EditContractWindow(Contract contract)
        {
            InitializeComponent();
            ViewModel = new EditContractViewModel(contract);
            DataContext = ViewModel;
            Title = contract.ProjectName;
            ViewModel.AddPopupAction = AddPopup;
            ViewModel.RemovePopupAction = RemovePopup;

        }

        public EditContractViewModel ViewModel { get; }

        private void AddPopup(UserControl control)
        {
            Grid.SetRow(control, 0);
            Grid.SetRowSpan(control, 2);
            MainGrid.Children.Add(control);
        }

        private void RemovePopup()
        {
            var index = MainGrid.Children.Count - 1;
            MainGrid.Children.RemoveAt(index);
        }

    }
}
