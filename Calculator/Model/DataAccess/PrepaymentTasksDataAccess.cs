using System;
using Calculator.CustomException;
using Calculator.Log;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.Model.DataAccess
{
    class PrepaymentTasksDataAccess
    {

        public static void Insert(PrepaymentTask task)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "insert into T_Prepayment_Tasks(Prepayment_Tasks_Prepayment_Id,Prepayment_Tasks_Level,Prepayment_Tasks_Amount,Prepayment_Tasks_Date) values(@id,@level,@amount,@date)";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id",task.PrepaymentId);
                command.Parameters.AddWithValue("@level",task.Level);
                command.Parameters.AddWithValue("@amount",task.Amount);
                command.Parameters.AddWithValue("@date",task.Date);
                Logger.LogQuery($"{sql}\r\n{task}");
                command.ExecuteNonQuery();
                connection.Close();
            }


        }

        public static void Update(PrepaymentTask task)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "update T_Prepayment_Tasks set Prepayment_Tasks_Amount = @amount , Prepayment_Tasks_Date = @date where Prepayment_Tasks_Prepayment_Id = @id and Prepayment_Tasks_Level = @level";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id",task.PrepaymentId);
                command.Parameters.AddWithValue("@level", task.Level);
                command.Parameters.AddWithValue("@date", task.Date);
                command.Parameters.AddWithValue("@amount", task.Amount);
                Logger.LogQuery($"{sql}\r\n{task}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static PrepaymentTask SelectByPrepaymentIdAndLevel(string prepaymentId, string level)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Prepayment_Tasks where Prepayment_Tasks_Prepayment_Id = @id and Prepayment_Tasks_Level = @level";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", prepaymentId);
                command.Parameters.AddWithValue("@level", level);
                Logger.LogQuery($"{sql}\r\nid = {prepaymentId}\r\nlevel = {level}");
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var task = new PrepaymentTask(prepaymentId,level)
                        {
                            Amount = reader["Prepayment_Tasks_Amount"].ToString(),
                            IsExistInDatabase = true
                        };
                        //date
                        var date = reader["Prepayment_Tasks_Date"].ToString();
                        try
                        {
                            task.Date = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);

                        }
                        connection.Close();
                        return task;
                    }
                    throw new ItemNotFoundException("prepayment level "+ level ,"prepayment id" , prepaymentId);
                }
                

            }
        }

        public static void Delete(string taskId,string level)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql =
                    "delete from T_Prepayment_Tasks where Prepayment_Tasks_Prepayment_Id = @id and Prepayment_Tasks_Level = @level";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", taskId);
                command.Parameters.AddWithValue("@level", level);
                Logger.LogQuery(sql);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
