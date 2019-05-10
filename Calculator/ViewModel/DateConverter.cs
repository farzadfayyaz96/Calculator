using System;
using Arash;

namespace Calculator.ViewModel
{
    class DateConverter
    {
        public static PersianDate PersianToGregorianDate(DateTime date)
        {
            return new PersianDate(date);
        }

        public static string PersianToGregorianString(DateTime date)
        {
            return new PersianDate(date).ToDateTime().ToShortDateString();
        }

        public static PersianDate StringToPersianDate(string date)
        {
            var dateArray = date.Split('/');
            var year = int.Parse(dateArray[0]);
            var month = int.Parse(dateArray[1]);
            var day = int.Parse(dateArray[2]);
            return new PersianDate(year, month, day);
        }

        

    }
}
