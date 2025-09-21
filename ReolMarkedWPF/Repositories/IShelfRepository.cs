using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    internal interface IShelfRepository
    {
        List<Shelf> GetAllShelves();
        void AddShelf(Shelf shelf);
        void DeleteShelf(Shelf shelf);
        void UpdateShelf(Shelf shelf);
    }
}
