using System;

namespace Calculator.CustomException
{
    class DatabaseItemNotFoundException : Exception
    {
        public DatabaseItemNotFoundException()
        {

        }

        public DatabaseItemNotFoundException(string itemName,string parameter, string value) : base($"{itemName} with {parameter} : {value} not found")
        {

        }
    }
}
