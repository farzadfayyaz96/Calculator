using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    public class GeneralSituationDataGridItem : NotifyProperty
    {
        private int _index;
        public GeneralSituationDataGridItem(Function function)
        {
            FunctionSituation = function;
            IsFunction = true;
            PaymentSituation = new Payment();
        }

        public GeneralSituationDataGridItem(Payment payment)
        {
            PaymentSituation = payment;
            FunctionSituation = new Function();
        }


        public bool IsFunction { get; set; }
        public Function FunctionSituation { get; }

        public Payment PaymentSituation { get; }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }
        
        

    }
}
