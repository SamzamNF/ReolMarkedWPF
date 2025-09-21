using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public class SqlRentRepository : IRentRepository<Rent>
    {
        private readonly string _connectionString;

        public SqlRentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        // Sætter data ind til en RENT_AGREEMENT der bliver oprettet. 
        // Der indsættes ikke et RentAgreementID - Da oprettes med IDENTITY automatisk
        public void AddRent(Rent rent)
        {
            string query = "INSERT INTO RENT_AGREEMENT (StartDate, EndDate, ShelfVendorID) VALUES (@StartDate, @EndDate, @ShelfVendorID)";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StartDate", rent.StartDate);
                command.Parameters.AddWithValue("@EndDate", rent.EndDate);
                command.Parameters.AddWithValue("@ShelfVendorID", rent.ShelfVendorID);

                connection.Open();
                command.ExecuteNonQuery();

            }
        }

        // RentAgreementID = KolonneNavn, og @RentAgreementID er variablen
        public void DeleteRent(Rent rent)
        {
            string query = "DELETE FROM RENT_AGREEMENT WHERE RentAgreementID = @RentAgreementID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RentAgreementID", rent.RentID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception($"Ingen aftale med RentAgreementID = {rent.RentID} blev fundet");
                }
            }
        }

        // Henter en liste af alle de rent_agreements der findes i databasen, ved at oprette objekter ud fra dem og så returnere dem i en liste
        public List<Rent> GetAllRents()
        {
            var rents = new List<Rent>();
            string query = "SELECT * FROM RENT_AGREEMENT";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rents.Add(new Rent
                        {
                            StartDate = (DateOnly)reader["StartDate"],
                            EndDate = (DateOnly)reader["EndDate"],
                            RentID = (int)reader["RentAgreementID"],
                            ShelfVendorID = (int)reader["ShelfVendorID"]
                        });
                    }
                }
            }
            return rents;
        }

        // Opdatere detaljer af en rent_agreement som allerede findes, ved at man kan ændrer på start og slutdato, fx hvis en aftale forlænges eller er indtastet forkert
        public void UpdateRent(Rent rent)
        {
            string query = "UPDATE RENT_AGREEMENT SET " +
                "StartDate = @StartDate, " +
                "EndDate = @EndDate " +
                "WHERE RentAgreementID = @RentAgreementID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@StartDate", rent.StartDate);
                command.Parameters.AddWithValue("@EndDate", rent.EndDate);
                command.Parameters.AddWithValue("@RentAgreementID", rent.RentID);

                connection.Open();
                
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception($"Ingen aftale med RentAgreementID = {rent.RentID} blev fundet");
                }
            }
        }
    }
}
