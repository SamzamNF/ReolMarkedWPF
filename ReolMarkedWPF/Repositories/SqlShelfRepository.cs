using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    internal class SqlShelfRepository : IShelfRepository
    {
        private readonly string _connectionString;

        public SqlShelfRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Shelf> GetAllShelves()
        {
            var shelves = new List<Shelf>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                /*connection.Open();
                string sql = @"
                    SELECT ShelfNumber, ShelfType, Price, RentAgreementID 
                    FROM Shelf";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var shelf = new Shelf
                            {
                                ShelfNumber = reader.GetInt32("ShelfNumber"),
                                ShelfType = reader.GetString("ShelfType"),
                                Price = reader.GetDecimal("Price"),
                                RentAgreementID = reader.GetInt32("RentAgreementID"),
                            };

                            shelves.Add(shelf);
                        }
                    }
                }*/
            }

            return shelves;
        }

        public void AddShelf(Shelf shelf)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString)) // Der bruges using så connection automatisk lukker
            {
                connection.Open();

                // @ er for læsbarheden
                string sql = @"
                    INSERT INTO Shelf (ShelfNumber, ShelfType, Price, RentAgreementID) 
                    VALUES (@ShelfNumber, @ShelfType, @Price, @RentAgreementID)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ShelfNumber", shelf.ShelfNumber);
                    command.Parameters.AddWithValue("@ShelfType", shelf.ShelfType);
                    command.Parameters.AddWithValue("@Price", shelf.Price);
                    command.Parameters.AddWithValue("@RentAgreementID", shelf.RentAgreementID);

                    command.ExecuteNonQuery();  // Returnerer et antal på hvor mange rækker der blev påvirket
                }
            }
        }

        public void DeleteShelf(Shelf shelf)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = @"
                    DELETE FROM Shelf 
                    WHERE ShelfNumber = @ShelfNumber";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ShelfNumber", shelf.ShelfNumber);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateShelf(Shelf shelf)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = @"
                    UPDATE Shelf 
                    SET ShelfType = @ShelfType, 
                        Price = @Price, 
                        RentAgreementID = @RentAgreementID 
                    WHERE ShelfNumber = @ShelfNumber";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ShelfNumber", shelf.ShelfNumber);
                    command.Parameters.AddWithValue("@ShelfType", shelf.ShelfType);
                    command.Parameters.AddWithValue("@Price", shelf.Price);
                    command.Parameters.AddWithValue("@RentAgreementID", shelf.RentAgreementID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}