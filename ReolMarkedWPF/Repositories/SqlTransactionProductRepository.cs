using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public class SqlTransactionProductRepository : ITransactionProductRepository

    //Forbindelse til database
    {
        private readonly string _connectionString;

        //Constructor

        public SqlTransactionProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }




        //Liste over alle transaktionsprodukter

        public List<TransactionProduct> GetAllTransactionProducts()
        {
            var products = new List<TransactionProduct>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Name, Price FROM TransactionProducts";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new TransactionProduct
                        {
                            TransactionID = reader.GetInt32(0),
                            ProductID = reader.GetInt32(1),
                            UnitPrice = reader.GetDecimal(2),
                            Amount = reader.GetInt32(3)
                        };
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        //Tilføj et transaktionsprodukt

        public void AddTransactionProduct(TransactionProduct transactionProduct)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO TransactionProducts (TransactionID, ProductID, UnitPrice, Amount) VALUES (@TransactionID, @ProductID, @UnitPrice, @Amount)";
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

        //Slet et transaktionsprodukt

        public void DeleteTransactionProduct(TransactionProduct transactionProduct)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM TransactionProducts WHERE TransactionID = @TransactionID AND ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionID", transactionProduct.TransactionID);
                    cmd.Parameters.AddWithValue("@ProductID", transactionProduct.ProductID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Redi´ger / opdater et transaktionsprodukt

        public void UpdateTransactionProduct(TransactionProduct transactionProduct)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE TransactionProducts SET UnitPrice = @UnitPrice, Amount = @Amount WHERE TransactionID = @TransactionID AND ProductID = @ProductID";
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
