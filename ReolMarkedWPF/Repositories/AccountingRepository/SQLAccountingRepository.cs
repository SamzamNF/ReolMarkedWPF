using System.Data;
using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models.AccountingModels;

namespace ReolMarkedWPF.Repositories.AccountingRepository
{
    public class SQLAccountingRepository : IAccountingRepository
    {
        private readonly string _connectionString;

        public SQLAccountingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task<List<AccountingResult>> GetAccountingData(DateOnly startDate, DateOnly endDate)
        {
            // Opretter liste
            var accountingList = new List<AccountingResult>();

            // Opretter forbindelse med min connectionstring
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Opretter en SqlCommand, der bruger stored procedure 'uspCreateAccounting'
                SqlCommand command = new SqlCommand("uspCreateAccounting", connection);
                
                // Angiver at kommandoen er en stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // Tilføjer parametre til stored proceduren
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                // Åbner forbindelsen med async for at undgå blokering af UI (lag/ui freeze)
                await connection.OpenAsync();

                // Bruger using, så DataReader selv lukker når den er færdig
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Læser data række for række
                    while (await reader.ReadAsync())
                    {
                        var account = new AccountingResult
                        {
                            ShelfVendorID = (int)reader["ShelfVendorID"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            PaymentInfo = (string)reader["PaymentInfo"],
                            TotalRent = (decimal)reader["TotalRent"],
                            TotalSale = (decimal)reader["TotalSale"],
                            AfterComission = (decimal)reader["AfterComission"],
                            Payment = (decimal)reader["Payment"]
                        };
                        accountingList.Add(account);
                    }

                }
            }

                return accountingList;
        }

        public async Task<List<AccountingResult>> GetAllSales(int? ShelfVendorID)
        {
            var soldDataList = new List<AccountingResult>();

            // Opretter forbindelse med connectionString
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Opretter SqlCommand, der bruger stored procedure "uspSalesForShelfVendor"
                SqlCommand command = new SqlCommand("uspSalesForShelfVendor", connection);

                // Angiver at kommandoen er en stored procedure
                command.CommandType = CommandType.StoredProcedure;

                // Tilføjer parametre til stored proceduren, for at filtrere
                command.Parameters.AddWithValue("@ShelfVendorID", ShelfVendorID);

                // Åbner forbindelsen med async for at undgå blokering af UI (lag/ui freeze)
                await connection.OpenAsync();

                // Bruger using, så DataReader selv lukker når den er færdig
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    // Læser data række for række
                    while (await reader.ReadAsync())
                    {
                        var soldData = new AccountingResult
                        {
                            ShelfVendorID = (int)reader["ShelfVendorID"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            TotalSale = (decimal)reader["TotalSale"]
                        };
                        soldDataList.Add(soldData);
                    }
                }

                return soldDataList;

            }
        }
    }
}
