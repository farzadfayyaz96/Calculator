using System;
using System.Collections.Generic;
using Calculator.CustomException;
using Calculator.Log;
using Calculator.Model.TableObject;

namespace Calculator.Model.DataAccess
{
    class ProfitDateAccess
    {
        public static List<Profit> SelectByYear(int year)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Profit where Profit_Year = @year";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@year", year);
                Logger.LogQuery($"{sql}\n\ryear = {year}");
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<Profit>();
                    while (reader.Read())
                    {
                        var profitYear = reader["Profit_Year"].ToString();
                        var month = reader["Profit_Month"].ToString();
                        var interestRates = reader["Profit_Interest_Rates"].ToString();
                        var profit = new Profit(profitYear, month,interestRates);
                        list.Add(profit);
                    }
                    connection.Close();
                    return list;
                }

            }
        }

       

        public static Profit SelectProfit(Profit profit)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Profit where Profit_Year = @year and Profit_Month = @month";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@year", profit.Year);
                command.Parameters.AddWithValue("@month", profit.Month);
                Logger.LogQuery($"{sql}\n\r{profit}");
                using (var reader = command.ExecuteReader())
                {
                    //found
                    if (reader.Read())
                    {
                        var profitYear = reader["Profit_Year"].ToString();
                        var month = reader["Profit_Month"].ToString();
                        var interestRates = reader["Profit_Interest_Rates"].ToString();
                        //close database 
                        connection.Close();
                        //return object
                        return new Profit(profitYear, month, interestRates);
                    }
                    //not found
                    //close database
                    connection.Close();
                    throw new DatabaseItemNotFoundException();
                }
            }
        }

        public static void Insert(Profit profit)
        {
            Console.WriteLine(profit);
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "insert into T_Profit(Profit_Year,Profit_Month,Profit_Interest_Rates) values(@year,@month,@rates)";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@year", profit.Year);
                command.Parameters.AddWithValue("@month", profit.Month);
                command.Parameters.AddWithValue("@rates", profit.InterestRates);
                Logger.LogQuery($"{sql}\r\n{profit}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Update(Profit profit)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "update T_Profit set Profit_Interest_Rates=@rates where Profit_Year=@year and Profit_Month=@month";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@year", profit.Year);
                command.Parameters.AddWithValue("@month", profit.Month);
                command.Parameters.AddWithValue("@rates", profit.InterestRates);
                Logger.LogQuery($"{sql}\n\r{profit}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void InsertOrUpdate(Profit profit)
        {
            try
            {
                //select profit -- if not exist throws exception
                SelectProfit(profit);
                //exist so update it
                Update(profit);
            }
            catch (DatabaseItemNotFoundException)
            {
                //item not found so insert it
                Insert(profit);
            }
        }
    }
}
