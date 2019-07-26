using System;
using System.Diagnostics;
using System.IO;
using Arash;

namespace Calculator.Log
{
    class Logger
    {
        
        private static readonly string LogDirectoryPath = $"{Directory.GetCurrentDirectory()}//log";
        private static readonly string LogFilePath = $"{LogDirectoryPath}//error.log";


        private static PersianDate PersianDate => new PersianDate(DateTime.Now);


        public static void Log(string text)
        {
            if (!Directory.Exists(LogDirectoryPath))
            {
                //directory not exist 
                //create new directory
                Directory.CreateDirectory(LogDirectoryPath);
            }

            using (var writer = new StreamWriter(LogFilePath,true))
            {
                writer.WriteLine($"{DateTime.Now.ToShortTimeString()} -- {PersianDate.ToString()} : {text}");
                writer.WriteLine("----------------------------------------------------------------------");
                writer.Close();
                Debug.WriteLine(text);
                Debug.WriteLine("----------------------------------------------------------------------");

            }
        }

        public static void LogException(Exception e)
        {
            var message = $"{e.Message}\r\n{e.StackTrace}";
            Log(message);
        }

        public static void LogQuery(string sql)
        {
            var message = $"execute sql = {sql}";
            Log(message);
        }

    }
}
