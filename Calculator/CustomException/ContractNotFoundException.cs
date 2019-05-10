using System;

namespace Calculator.CustomException
{
    class ContractNotFoundException : Exception
    {
        public ContractNotFoundException(string parameter,string value):base($"Contract with {parameter} : {value} not found")
        {
            
        }

    }
}
