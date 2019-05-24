

using System;
using System.Text.RegularExpressions;

namespace Calculator.ViewModel
{
    class AmountSplitter
    {
        public static string Split(string str, int chunkSize)
        {
            var temp = string.Empty;
            var counter = 1;
            for (var i = str.Length - 1; i >= 0; i--)
            {
                var coma = counter % chunkSize == 0 && counter != str.Length ? "," : string.Empty;
                counter++;
                temp = $"{coma}{str[i]}{temp}";
            }
            return temp;
        }

        public static Regex AmountRegex = new Regex("^[0-9]\\d*$");
    }
}
