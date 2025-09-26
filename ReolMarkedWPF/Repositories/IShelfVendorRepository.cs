using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public interface IShelfVendorRepository
    {
        List<ShelfVendor> GetAllShelfVendors();
        int AddShelfVendor(ShelfVendor shelfVendor);
        void DeleteShelfVendor(ShelfVendor shelfVendor);
        void UpdateShelfVendor(ShelfVendor shelfVendor);
        void ShowShelfVendors();
    }
}