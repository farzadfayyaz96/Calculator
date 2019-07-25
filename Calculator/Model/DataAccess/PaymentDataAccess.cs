using System;
using System.Collections.ObjectModel;
using Calculator.Log;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.Model.DataAccess
{
    class PaymentDataAccess
    {
        public static void Insert(Payment payment)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "insert into T_Payment(Payment_Id,Payment_Prepayment_Id,Payment_Amount,Payment_Date) values(@id,@prepaymentId,@amount,@date)";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", payment.Id);
                command.Parameters.AddWithValue("@prepaymentId", payment.PrepaymentId);
                command.Parameters.AddWithValue("@amount", payment.Amount);
                command.Parameters.AddWithValue("@date", payment.Date);
                Logger.LogQuery($"{sql}\r\n{payment}");
                command.ExecuteNonQuery();
                connection.Close();

            }
        }

        public static void Update(Payment payment)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "update T_Payment set Payment_Amount = @amount , Payment_Date = @date where Payment_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id",payment.Id);
                command.Parameters.AddWithValue("@amount", payment.Amount);
                command.Parameters.AddWithValue("@date", payment.Date);
                Logger.LogQuery($"{sql}\r\n{payment}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static ObservableCollection<Payment> SelectByPrepaymentId(string prepaymentId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select * from T_Payment where Payment_Prepayment_Id = @id";
                command.Parameters.AddWithValue("@id", prepaymentId);
                Logger.LogQuery($"{command.CommandText}\r\nid = {prepaymentId}");
                using (var reader = command.ExecuteReader())
                {
                    var list = new ObservableCollection<Payment>();
                    while (reader.Read())
                    {
                        var id = reader["Payment_Id"].ToString();
                        var preId = reader["Payment_Prepayment_Id"].ToString();
                        var item = new Payment(id,preId)
                        {
                            Amount = reader["Payment_Amount"].ToString(),
                            IsExistInDatabase = true
                        };
                        //date
                        var date = reader["Payment_Date"].ToString();
                        try
                        {
                            item.Date = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);

                        }
                        list.Add(item);
                    }
                    connection.Close();
                    return list;

                }
            }
        }

        public static void DeleteAll(string prepaymentId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "delete from T_Payment where Payment_Prepayment_Id = @id";
                command.Parameters.AddWithValue("@id", prepaymentId);
                Logger.LogQuery($"{command.CommandText}\r\nid = {prepaymentId}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Delete(string paymentId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "delete from T_Payment where Payment_Id = @id";
                command.Parameters.AddWithValue("@id", paymentId);
                Logger.LogQuery($"{command.CommandText}\r\nid = {paymentId}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
