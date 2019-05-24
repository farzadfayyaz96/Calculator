using System;
using System.Collections.ObjectModel;
using Calculator.Log;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.Model.DataAccess
{
    class FunctionDataAccess
    {
        public static ObservableCollection<FunctionDataGridItem> SelectAll(string contractId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Function where Contract_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", contractId);
                Logger.LogQuery(sql);
                using (var reader = command.ExecuteReader())
                {
                    var list = Function.GetObservableCollection();
                    while (reader.Read())
                    {
                        var function = new Function
                        {
                            Id = reader["Function_Id"].ToString(),
                            ContractId = reader["Contract_Id"].ToString(),
                            ContractType = ContractTypeUtil.GetContractTypeByName(reader["Function_Type"].ToString()),
                            Amount = reader["Function_Amount"].ToString(),
                        };
                        //date
                        var date = reader["Function_Date"].ToString();
                        try
                        {
                            function.Date = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                        }
                        list.Add(new FunctionDataGridItem(function));
                    }
                    connection.Close();
                    return list;
                }
            }

        }

        public static void Insert(Function function)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "insert into T_Function(Function_Id,Contract_Id,Function_Type,Function_Date,Function_Amount) values(@id,@contractId,@type,@date,@amount)";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", function.Id);
                command.Parameters.AddWithValue("@contractId", function.ContractId);
                command.Parameters.AddWithValue("@type", function.ContractTypeName);
                command.Parameters.AddWithValue("@date", function.Date);
                command.Parameters.AddWithValue("@amount", function.Amount);
                Logger.LogQuery($"{sql}\r\n{function}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        public static void Update(Function function)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "update T_Function set Function_Type = @type , Function_Date = @date , Function_Amount = @amount where Function_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id",function.Id);
                command.Parameters.AddWithValue("@type",function.ContractType);
                command.Parameters.AddWithValue("@date",function.Date);
                command.Parameters.AddWithValue("@amount",function.Amount);
                Logger.LogQuery(sql);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static void Delete(string functionId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "delete from T_Function where Function_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", functionId);
                Logger.LogQuery(sql);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
