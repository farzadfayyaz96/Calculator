using System;

namespace Calculator.CustomException
{
    class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("password wrong exception.")
        {

        }

    }
}
