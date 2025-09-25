using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public interface IShelfRepository
    {
        List<Shelf> GetAllShelves();
        void AddShelf(Shelf shelf);
        void DeleteShelf(Shelf shelf);
        void UpdateShelf(Shelf shelf);
    }
}
