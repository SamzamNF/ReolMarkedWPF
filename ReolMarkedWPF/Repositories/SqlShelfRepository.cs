using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    internal class SqlShelfRepository : IShelfRepository
    {
        // Tilføj en connection string og implementer metoderne fra interfacet her
        private readonly string _connectionString;

        public SqlShelfRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddShelf(Shelf shelf)
        {
            throw new NotImplementedException();
        }

        public void DeleteShelf(Shelf shelf)
        {
            throw new NotImplementedException();
        }

        public List<Shelf> GetAllShelves()
        {
            // Midlertidig returnering for at undgå fejl under udvikling
            return new List<Shelf>();
        }

        public void UpdateShelf(Shelf shelf)
        {
            throw new NotImplementedException();
        }
    }
}