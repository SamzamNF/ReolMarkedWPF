using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    internal class SqlTransactionRepository : ITransactionRepository<Transaction>
    {
        private readonly string _connectionString;

        public SqlTransactionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Sætter data ind til en transaction - IDENTITY tilføjer ID i databasen og outputter et ID
        public int AddTransaction(Transaction transaction)
        {
            string query = "INSERT INTO [TRANSACTION] (TransactionDate) " +
                           "OUTPUT inserted.TransactionID " +
                           "VALUES (@TransactionDate)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);

                connection.Open();

                int newId = (int)command.ExecuteScalar();

                return newId;
            }
        }

        // Sletter en transaktion fra databasen baseret på TransactionID 
        // 'TransactionID' er kolonnenavnet og '@TransactionID' er den tilhørende parameter
        public void DeleteTransaction(Transaction transaction)
        {
            string query = "DELETE FROM [TRANSACTION] WHERE TransactionID = @TransactionID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                // Tjekker om den faktisk har opdateret nogle rows, ellers kast fejl
                if (rowsAffected == 0)
                {
                    throw new Exception($"Ingen transaktion fundet med ID {transaction.TransactionID}");
                }
            }
        }

        // Henter en liste af alle transaktioner der findes i databasen ved at oprette objekter ud fra DB, og så returnere dem i en liste
        public List<Transaction> GetAllTransactions()
        {
            var transactions = new List<Transaction>();
            
            string query = "SELECT TransactionID, TransactionDate " +
                           "FROM [TRANSACTION]";

            using (SqlConnection connection = new SqlConnection( _connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                // Læser data fra DB
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    // Fortsætter indtil der ikke er mere data
                    while (reader.Read())
                    {
                        // Tilføjer data som objekter til listen
                        transactions.Add(new Transaction
                        {
                            TransactionDate = DateOnly.FromDateTime((DateTime)reader["TransactionDate"]),
                            TransactionID = (int)reader["TransactionID"]
                        });
                    }
                }
            }
            return transactions;
        }

        // Opdatere transaktion dato baseret på transaktion ID'et, hvis evt en fejl sker i systemet når den oprettes.
        public void UpdateTransaction(Transaction transaction)
        {
            string query = "UPDATE [TRANSACTION] SET " +
                           "TransactionDate = @TransactionDate " +
                           "WHERE TransactionID = @TransactionID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                //Giver parameterene deres værdier
                command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
                command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);

                connection.Open();
                
                // Tjekker om den faktisk har opdateret nogle rows, ellers kast fejl
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception($"Ingen transaktion med ID {transaction.TransactionID} blev fundet");
                }
            }
        }
    }
}
