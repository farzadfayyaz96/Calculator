using Calculator.Log;

namespace Calculator.Model.DataAccess
{
    class ProgramInfoDataAccess
    {

        public static bool Login(string password)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Program_Info where Program_Info_Key = @key and Program_Info_Value = @password";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@key", "app_password");
                Logger.LogQuery(sql);
                using (var reader = command.ExecuteReader())
                {
                    var result = reader.Read();
                    connection.Close();
                    return result;
                }
            }
        }

        public static void UpdatePassword(string password)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "update T_Program_Info set Program_Info_Value = @password where Program_Info_Key = @key";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@key", "app_password");
                command.Parameters.AddWithValue("@password", password);
                Logger.LogQuery(sql);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


    }
}
