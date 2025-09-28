using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public class SqlTransactionProductRepository : ITransactionProductRepository
    {
        private readonly string _connectionString;

        public SqlTransactionProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TransactionProduct> GetAllTransactionProducts()
        {
            var products = new List<TransactionProduct>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // RETTELSE:
                // Den oprindelige query henviste til en tabel "TransactionProducts" og kolonner som "Id" og "Name".
                // Dette er rettet til det korrekte tabelnavn "TRANSACTION_PRODUCT" og de faktiske kolonnenavne 
                // ("TransactionID", "ProductID", "UnitPrice", "Amount") fra databasen.
                string query = "SELECT TransactionID, ProductID, UnitPrice, Amount FROM TRANSACTION_PRODUCT";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new TransactionProduct
                        {
                            // RETTELSE:
                            // Indlæsningen af data fra databasen er gjort mere robust.
                            // I stedet for at bruge faste index (f.eks. reader.GetInt32(0)), som kan fejle hvis
                            // rækkefølgen af kolonner i databasen ændres, bruges GetOrdinal("KolonneNavn").
                            // Dette sikrer, at koden altid finder den korrekte kolonne baseret på dens navn.
                            TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                            Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
                        };
                        products.Add(product);
                    }
                }
            }
            return products;
        }

        public void AddTransactionProduct(TransactionProduct transactionProduct)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // RETTELSE: Tabelnavnet var forkert. Rettet fra "TransactionProducts" til "TRANSACTION_PRODUCT".
                string query = "INSERT INTO TRANSACTION_PRODUCT (TransactionID, ProductID, UnitPrice, Amount) VALUES (@TransactionID, @ProductID, @UnitPrice, @Amount)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionID", transactionProduct.TransactionID);
                    cmd.Parameters.AddWithValue("@ProductID", transactionProduct.ProductID);
                    cmd.Parameters.AddWithValue("@UnitPrice", transactionProduct.UnitPrice);
                    cmd.Parameters.AddWithValue("@Amount", transactionProduct.Amount);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTransactionProduct(TransactionProduct transactionProduct)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // RETTELSE: Tabelnavnet var forkert. Rettet fra "TransactionProducts" til "TRANSACTION_PRODUCT".
                string query = "DELETE FROM TRANSACTION_PRODUCT WHERE TransactionID = @TransactionID AND ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionID", transactionProduct.TransactionID);
                    cmd.Parameters.AddWithValue("@ProductID", transactionProduct.ProductID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTransactionProduct(TransactionProduct transactionProduct)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // RETTELSE: Tabelnavnet var forkert. Rettet fra "TransactionProducts" til "TRANSACTION_PRODUCT".
                string query = "UPDATE TRANSACTION_PRODUCT SET UnitPrice = @UnitPrice, Amount = @Amount WHERE TransactionID = @TransactionID AND ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionID", transactionProduct.TransactionID);
                    cmd.Parameters.AddWithValue("@ProductID", transactionProduct.ProductID);
                    cmd.Parameters.AddWithValue("@UnitPrice", transactionProduct.UnitPrice);
                    cmd.Parameters.AddWithValue("@Amount", transactionProduct.Amount);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}