
using System;
using System.Data;
using Calculator.CustomException;
using Calculator.Log;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.Model.DataAccess
{
    class PrepaymentDataAccess
    {

        public static void Insert(Prepayment prepayment)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "insert into T_Prepayment(Prepayment_Id,Prepayment_Warranty_Date) values(@id,@date)";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", prepayment.Id);
                command.Parameters.AddWithValue("@date", prepayment.WarrantyDate);
                Logger.LogQuery($"execute sql = {sql}\r\n{prepayment}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static Prepayment SelectById(string prepaymentId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Prepayment where Prepayment_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", prepaymentId);
                Logger.LogQuery($"execute sql = {sql}\r\n id = {prepaymentId}");
                Prepayment prepayment = null;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        prepayment = new Prepayment (prepaymentId);
                        //date
                        if(connection.State == ConnectionState.Closed)connection.Open();
                        var date = reader["Prepayment_Warranty_Date"].ToString();
                        try
                        {
                            prepayment.WarrantyDate = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                        }
                    }
                    else throw new ItemNotFoundException("prepayment","id",prepaymentId);
                }
                connection.Close();
                return prepayment;
            }
        }

        public static void Update(Prepayment prepayment)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "update T_Prepayment set Prepayment_Warranty_Date = @date where Prepayment_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", prepayment.Id);
                command.Parameters.AddWithValue("@date", prepayment.WarrantyDate);
                Logger.LogQuery($"{sql}\r\n{prepayment}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
