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
    }
}
