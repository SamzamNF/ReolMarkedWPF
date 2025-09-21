using ReolMarkedWPF.Models;
using System.Collections.Generic;

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
            // Implementeres senere
            return new List<ShelfVendor>();
        }

        public void AddShelfVendor(ShelfVendor shelfVendor)
        {
            // Implementeres senere
        }

        public void DeleteShelfVendor(ShelfVendor shelfVendor)
        {
            // Implementeres senere
        }

        public void UpdateShelfVendor(ShelfVendor shelfVendor)
        {
            // Implementeres senere
        }
    }
}