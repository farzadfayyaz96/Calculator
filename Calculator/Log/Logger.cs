using System;
using System.Diagnostics;
using System.IO;
using Arash;

namespace Calculator.Log
{
    class Logger
    {
        private static StreamWriter _writer;
        private static readonly string LogDirectoryPath = $"{Directory.GetCurrentDirectory()}//log";
        private static readonly string LogFilePath = $"{LogDirectoryPath}//error.log";


        private static PersianDate PersianDate => new PersianDate(DateTime.Now);


        private static StreamWriter Writer
        {
            get
            {
                if (!Directory.Exists(LogDirectoryPath))
                {
                    //directory not exist 
                    //create new directory
                    Directory.CreateDirectory(LogDirectoryPath);
                }

                return _writer ?? (_writer = new StreamWriter(LogFilePath));
            }
        }

        public static void Log(string text)
        {
            Writer.WriteLine($"{DateTime.Now.ToShortTimeString()} -- {PersianDate.ToString()} : {text}");
            Writer.WriteLine("----------------------------------------------------------------------");
            Debug.WriteLine(text);
            Debug.WriteLine("----------------------------------------------------------------------");

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

        public static void CloseFile()
        {
            _writer.Close();
        }

    }
}
