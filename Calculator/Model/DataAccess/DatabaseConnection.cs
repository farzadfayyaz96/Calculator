
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Calculator.Model.DataAccess
{
    class DatabaseConnection
    {
        private static SQLiteConnection _connection;
        public static readonly string DataBasePath = $"{Directory.GetCurrentDirectory()}\\data.db";
        private static readonly object Lock = new object();

        private DatabaseConnection()
        {
        }

        public static SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    lock (Lock)
                    {
                        if (_connection != null) return _connection;
                        _connection = new SQLiteConnection($"Data Source={DataBasePath};Version=3;Compress=True");
                        _connection.Open();
                    }
                }
                else if (_connection.State == ConnectionState.Closed)
                {

                    _connection.Open();
                }

                return _connection;
            }
        }
    }
}
