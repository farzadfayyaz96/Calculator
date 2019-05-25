using System;
using System.Collections.ObjectModel;
using Calculator.Log;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.Model.DataAccess
{
    class PaymentDataAccess
    {
        public static ObservableCollection<PaymentDataGridItem> SelectAllByContractId(string contractId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Payment where Contract_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", contractId);
                Logger.LogQuery($"{sql}\r\ncontract id = {contractId}");
                using (var reader = command.ExecuteReader())
                {
                    var list = Payment.GetPaymentDataGridItemCollection();
                    while (reader.Read())
                    {
                        var payment = new Payment
                        {
                            Id = reader["Payment_Id"].ToString(),
                            Amount = reader["Payment_Amount"].ToString(),
                            ContractId = reader["Contract_Id"].ToString(),
                            PaymentType = reader["Payment_Type"].ToString(),
                            ContractType = reader["Contract_Type"].ToString()
                        };
                        //date
                        var date = reader["Payment_Date"].ToString();
                        try
                        {
                            payment.Date = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                        }
                        list.Add(new PaymentDataGridItem(payment));
                    }
                    connection.Close();
                    return list;
                }

            }
        }

        public static void Insert(Payment payment)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "insert into T_Payment(Payment_Id,Contract_Id,Contract_Type,Payment_Type,Payment_Date,Payment_Amount) values(@id,@contractId,@contractType,@paymentType,@date,@amount)";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id",payment.Id);
                command.Parameters.AddWithValue("@contractId", payment.ContractId);
                command.Parameters.AddWithValue("@contractType", payment.ContractType);
                command.Parameters.AddWithValue("@paymentType", payment.PaymentType);
                command.Parameters.AddWithValue("@date", payment.Date);
                command.Parameters.AddWithValue("@amount", payment.Amount);
                Logger.LogQuery($"{sql}\r\n{payment}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Update(Payment payment)
        {
            var connection = DatabaseConnection.Connection;
            const string sql = "update T_Payment set Contract_Type = @contractType , Payment_Type = @paymentType , Payment_Date = @date , Payment_Amount = @amount where Payment_Id = @id";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", payment.Id);
                command.Parameters.AddWithValue("@contractType", payment.ContractType);
                command.Parameters.AddWithValue("@paymentType", payment.PaymentType);
                command.Parameters.AddWithValue("@date", payment.Date.ToString());
                command.Parameters.AddWithValue("@amount", payment.Amount);
                Logger.LogQuery($"{sql}\n\r{payment}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Delete(string paymentId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "delete from T_Payment where Payment_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", paymentId);
                Logger.LogQuery($"{sql}\r\npayment id = {paymentId}");
                command.ExecuteNonQuery();
                connection.Close();
                
            }
        }

    }
}
