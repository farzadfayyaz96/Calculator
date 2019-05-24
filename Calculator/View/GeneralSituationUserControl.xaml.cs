

using Calculator.ViewModel;

namespace Calculator.View
{
  
    public partial class GeneralSituationUserControl 
    {
        public GeneralSituationUserControl()
        {
            InitializeComponent();
            DataContext = new GeneralSituationViewModel();
        }
    }
}
