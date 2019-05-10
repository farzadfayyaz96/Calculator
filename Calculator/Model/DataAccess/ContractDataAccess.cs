using System;
using System.Collections.ObjectModel;
using Calculator.CustomException;
using Calculator.Log;
using Calculator.Model.TableObject;
using Calculator.ViewModel;

namespace Calculator.Model.DataAccess
{
    class ContractDataAccess
    {
        public static ObservableCollection<ContractDataGridItem> SelectAll()
        {
            var connection = DatabaseConnection.Connection;
            var list = Contract.GetObservableCollection();
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Contract_Info";
                command.CommandText = sql;
                Logger.Log($"execute sql = {sql}");
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contract = new Contract
                        {
                            Id = reader["Contract_Id"].ToString(),
                            ProjectName = reader["Contract_Project_Name"].ToString(),
                            ContractorName = reader["Contract_Contractor_Name"].ToString(),
                            Number = reader["Contract_Number"].ToString(),
                            Amount = reader["Contract_Amount"].ToString()
                            
                        };
                        //date
                        var date = reader["Contract_Date"].ToString();
                        try
                        {
                            contract.Date = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);

                        }
                        list.Add(new ContractDataGridItem(contract));
                    }
                    connection.Close();
                    return list;
                }
            }
            
        }

        public static ObservableCollection<ContractDataGridItem> Search(string text)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "select *  from T_Contract_Info where Contract_Project_Name like @text or Contract_Contractor_Name like @text or Contract_Number like @text";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@text",$"%{text}%");
                Logger.Log($"execute sql = {sql}\r\ntext = {text}");
                using (var reader = command.ExecuteReader())
                {
                    var list = Contract.GetObservableCollection();
                    while (reader.Read())
                    {
                        var contract = new Contract
                        {
                            Id = reader["Contract_Id"].ToString(),
                            ProjectName = reader["Contract_Project_Name"].ToString(),
                            ContractorName = reader["Contract_Contractor_Name"].ToString(),
                            Number = reader["Contract_Number"].ToString(),
                            Amount = reader["Contract_Amount"].ToString()
                        };
                        //date
                        var date = reader["Contract_Date"].ToString();
                        try
                        {
                            contract.Date = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                        }
                        list.Add(new ContractDataGridItem(contract));
                    }
                    connection.Close();
                    return list;
                }
            }
        }
        public static Contract SelectById(string contractId)
        {
            var connection = DatabaseConnection.Connection;
            
            using (var command = connection.CreateCommand())
            {
                const string sql = "select * from T_Contract_Info where Contract_Id = @contractId";
                command.CommandText = sql;
                Logger.Log($"execute sql = {sql}\r\ncontract id = {contractId}");
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var contract = new Contract
                        {
                            Id = contractId,
                            ProjectName = reader["Contract_Project_Name"].ToString(),
                            ContractorName = reader["Contract_Contractor_Name"].ToString(),
                            Number = reader["Contract_Number"].ToString(),
                            Amount = reader["Contract_Amount"].ToString()
                        };
                        //date
                        var date = reader["Contract_Date"].ToString();
                        try
                        {
                            contract.Date = DateConverter.StringToPersianDate(date);
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                        }
                        connection.Close();
                        return contract;
                    }
                    throw new ContractNotFoundException("contract id",contractId);
                }
            }
        }

        public static void Insert(Contract contract)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "insert into T_Contract_Info(Contract_Id,Contract_Project_Name,Contract_Contractor_Name,Contract_Date,Contract_Number,Contract_Amount) values(@id,@projectName,@contractorName,@date,@number,@amount)";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", contract.Id);
                command.Parameters.AddWithValue("@projectName", contract.ProjectName);
                command.Parameters.AddWithValue("@contractorName", contract.ContractorName);
                command.Parameters.AddWithValue("@date", contract.Date);
                command.Parameters.AddWithValue("@number", contract.Number);
                command.Parameters.AddWithValue("@amount", contract.Amount);
                Logger.Log($"execute sql = {sql}\r\n{contract}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Update(Contract contract) 
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "update T_Contract_Info set Contract_Project_Name = @projectName,Contract_Contractor_Name = @contractorName,Contract_Date = @date , Contract_Number = @number , Contract_Amount = @amount  where Contract_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", contract.Id);
                command.Parameters.AddWithValue("@projectName", contract.ProjectName);
                command.Parameters.AddWithValue("@contractorName", contract.ContractorName);
                command.Parameters.AddWithValue("@date", contract.Date);
                command.Parameters.AddWithValue("@number", contract.Number);
                command.Parameters.AddWithValue("@amount", contract.Amount);
                Logger.Log($"execute sql = {sql}\n\r{contract}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void Delete(string contractId)
        {
            var connection = DatabaseConnection.Connection;
            using (var command = connection.CreateCommand())
            {
                const string sql = "delete from T_Contract_Info where Contract_Id = @id";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", contractId);
                Logger.Log($"execute sql = {sql}\n\r contract id = {contractId}");
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


    }
}
