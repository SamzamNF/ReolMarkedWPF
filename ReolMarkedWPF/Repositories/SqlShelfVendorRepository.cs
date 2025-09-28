using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public class SqlShelfVendorRepository : IShelfVendorRepository
    {
        private readonly string _connectionString;

        public SqlShelfVendorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ShelfVendor> GetAllShelfVendors()
        {
            var vendors = new List<ShelfVendor>();
            string query = "SELECT * FROM SHELF_VENDOR";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vendors.Add(new ShelfVendor
                        {
                            ShelfVendorID = (int)reader["ShelfVendorID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }
            return vendors;
        }

        public int AddShelfVendor(ShelfVendor shelfVendor)
        {
            string query = "INSERT INTO SHELF_VENDOR (FirstName, LastName, PhoneNumber, Email) " +
                           "OUTPUT inserted.ShelfVendorID " +
                           "VALUES (@FirstName, @LastName, @PhoneNumber, @Email)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", shelfVendor.FirstName);
                command.Parameters.AddWithValue("@LastName", shelfVendor.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", shelfVendor.PhoneNumber);
                command.Parameters.AddWithValue("@Email", shelfVendor.Email);

                connection.Open();
                int newId = (int)command.ExecuteScalar();

                return newId;
            }
        }

        public void UpdateShelfVendor(ShelfVendor shelfVendor)
        {
            string query = "UPDATE SHELF_VENDOR SET FirstName = @FirstName, LastName = @LastName, " +
                           "PhoneNumber = @PhoneNumber, Email = @Email " +
                           "WHERE ShelfVendorID = @ShelfVendorID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", shelfVendor.FirstName);
                command.Parameters.AddWithValue("@LastName", shelfVendor.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", shelfVendor.PhoneNumber);
                command.Parameters.AddWithValue("@Email", shelfVendor.Email);
                command.Parameters.AddWithValue("@ShelfVendorID", shelfVendor.ShelfVendorID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteShelfVendor(ShelfVendor shelfVendor)
        {
            string query = "DELETE FROM SHELF_VENDOR WHERE ShelfVendorID = @ShelfVendorID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfVendorID", shelfVendor.ShelfVendorID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ShowShelfVendors()
        {
            throw new System.NotImplementedException();
        }
    }
}