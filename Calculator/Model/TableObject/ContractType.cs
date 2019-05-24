namespace Calculator.Model.TableObject
{
    public enum ContractType
    {
        Prepayment,//0
        Temporary,//1
        Certain,//2
        Balancing,//3
        Deposit//4
    }

    public class ContractTypeUtil
    {
        public static string GetNameByContractType(ContractType type)
        {
            if (type == ContractType.Prepayment) return "پیش پرداخت";
            if (type == ContractType.Temporary) return "موقت";
            if (type == ContractType.Certain) return "قطعی";
            if (type == ContractType.Balancing) return "تعدیل";
            return "سپرده";
            
        }

        public static ContractType GetContractTypeByName(string name)
        {
            if (name.Equals("تعدیل")) return ContractType.Balancing;
            if (name.Equals("پیش پرداخت")) return ContractType.Prepayment;
            if (name.Equals("موقت")) return ContractType.Temporary;
            if (name.Equals("قطعی")) return ContractType.Certain;
            return ContractType.Deposit;
        }

        public static ContractType GetContractTypeByIndex(int index)
        {
            if (index == 0) return ContractType.Prepayment;
            if (index == 1) return ContractType.Temporary;
            if (index == 2) return ContractType.Certain;
            if (index == 3) return ContractType.Balancing;
            return ContractType.Deposit;
        }
    }
}
