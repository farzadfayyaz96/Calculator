using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Calculator.Model.TableObject;

namespace Calculator.ViewModel
{
    public class GeneralSituationViewModel : NotifyProperty
    {

        public GeneralSituationViewModel()
        {
            GeneralSituationCollection = new ObservableCollection<GeneralSituationDataGridItem>();
        }

        

        public ObservableCollection<GeneralSituationDataGridItem> GeneralSituationCollection { get; }

        public void AddCollection(IEnumerable<Function> collection)
        {
            foreach (var item in collection)
            {
                item.RemainingAmount = BigInteger.Parse(item.Amount.Replace(",",""));
                GeneralSituationCollection.Add(new GeneralSituationDataGridItem(item));
            }
            ReIndex();
        }

        public void AddCollection(IEnumerable<Payment> collection)
        {
            foreach (var payment in collection)
            {
                AddPayment(payment);
            }
           
        }

        public void AddFunction(Function function)
        {
            GeneralSituationCollection.Add(new GeneralSituationDataGridItem(function));
        }


        public void AddPayment(Payment payment)
        {
            //calculate payment amount
            var paymentAmount = BigInteger.Parse(payment.Amount.Replace(",", ""));
            //get all functions
            var functionList = GeneralSituationCollection.Where(x => x.IsFunction)
                //get available functions list 
                .Where(x=> x.FunctionSituation.ContractType.Equals(payment.ContractType) && !x.FunctionSituation.RemainingAmount.IsZero);
            //
            var insertFlag = false;
            var insertIndex = 0;
            foreach (var funcItem in functionList)
            {
                var subtractResult = funcItem.FunctionSituation.RemainingAmount - paymentAmount;
                var compareResult = BigInteger.Compare(subtractResult, 0);
                //check function amount 
                if (compareResult < 0)
                {
                    continue;
                }
                //function amount is enough
                funcItem.FunctionSituation.RemainingAmount = subtractResult;
                insertIndex = funcItem.Index;
                insertFlag = true;
                //exit loop
                break;
            }

            if (insertFlag)
            {
                for (var i = insertIndex; i < GeneralSituationCollection.Count ; i++)
                {
                    var item = GeneralSituationCollection[i];
                    if(!item.IsFunction)continue;
                    //insert it 
                    GeneralSituationCollection.Insert(i,new GeneralSituationDataGridItem(payment));
                    //exit loop
                    break;
                }
                
            }
            else
            {
                ///////////////////////////////////////////////////////// not found -> ask question
                GeneralSituationCollection.Add(new GeneralSituationDataGridItem(payment));
            }
            ReIndex();

        }


        public void ReIndex()
        {
            var index = 1;
            foreach (var item in GeneralSituationCollection)
            {
                item.Index = index++;
            }
            
        }

    }
}
