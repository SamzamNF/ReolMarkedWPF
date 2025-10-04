using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public SqlProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT ProductID, ShelfNumber, ProductName, UnitPrice, Amount FROM Product";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32("ProductID"),
                                ShelfNumber = reader.GetInt32("ShelfNumber"),
                                ProductName = reader.GetString("ProductName"),
                                UnitPrice = reader.GetDecimal("UnitPrice"),
                                Amount = reader.GetInt32("Amount")
                            });
                        }
                    }
                }
            }
            return products;
        }
        public int AddProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // OUTPUT inserted.ProductID giver UI'et det nye ID
                string sql = @"
                    INSERT INTO Product (ShelfNumber, ProductName, UnitPrice, Amount) 
                    OUTPUT inserted.ProductID
                    VALUES (@ShelfNumber, @ProductName, @UnitPrice, @Amount)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ShelfNumber", product.ShelfNumber);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                    command.Parameters.AddWithValue("@Amount", product.Amount);

                    // ExecuteScalar() sørger for at hente det nye ID
                    int newId = (int)command.ExecuteScalar();
                    return newId;
                }
            }
        }
        public void DeleteProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = @"
                    DELETE FROM Product 
                    WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = @"
                    UPDATE Product
                    SET ShelfNumber = @ShelfNumber, 
                        ProductName = @ProductName, 
                        UnitPrice = @UnitPrice,
                        Amount = @Amount 
                    WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@ShelfNumber", product.ShelfNumber);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                    command.Parameters.AddWithValue("@Amount", product.Amount);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
