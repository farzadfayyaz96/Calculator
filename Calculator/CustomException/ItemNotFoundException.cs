using System;

namespace Calculator.CustomException
{
    class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string itemName,string parameter,string value):base($"{itemName} with {parameter} : {value} not found")
        {
            
        }

    }
}
