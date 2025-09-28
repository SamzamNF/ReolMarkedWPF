using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public class SqlPaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly string _connectionString;

        // Konstruktør, der modtager connection string.
        public SqlPaymentMethodRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Henter en liste over alle betalingsmetoder.
        public List<PaymentMethod> GetAllPaymentMethods()
        {
            var methods = new List<PaymentMethod>();
            string query = "SELECT * FROM PAYMENT_METHOD";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        methods.Add(new PaymentMethod
                        {
                            ShelfVendorID = (int)reader["ShelfVendorID"],
                            PaymentOption = reader["PaymentOption"].ToString(),
                            PaymentInfo = reader["PaymentInfo"].ToString()
                        });
                    }
                }
            }
            return methods;
        }

        // Tilføjer en ny betalingsmetode.
        public void AddPaymentMethod(PaymentMethod paymentMethod)
        {
            string query = "INSERT INTO PAYMENT_METHOD (ShelfVendorID, PaymentOption, PaymentInfo) " +
                           "VALUES (@ShelfVendorID, @PaymentOption, @PaymentInfo)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfVendorID", paymentMethod.ShelfVendorID);
                command.Parameters.AddWithValue("@PaymentOption", paymentMethod.PaymentOption);
                command.Parameters.AddWithValue("@PaymentInfo", paymentMethod.PaymentInfo);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Opdaterer en eksisterende betalingsmetode.
        public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            string query = "UPDATE PAYMENT_METHOD SET ShelfVendorID = @ShelfVendorID, " +
                           "PaymentOption = @PaymentOption, PaymentInfo = @PaymentInfo " +
                           "WHERE ShelfVendorID = @ShelfVendorID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfVendorID", paymentMethod.ShelfVendorID);
                command.Parameters.AddWithValue("@PaymentOption", paymentMethod.PaymentOption);
                command.Parameters.AddWithValue("@PaymentInfo", paymentMethod.PaymentInfo);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Sletter en betalingsmetode baseret på dens unikke ID.
        public void DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            string query = "DELETE FROM PAYMENT_METHOD WHERE ShelfVendorID = @ShelfVendorID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShelfVendorID", paymentMethod.ShelfVendorID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}