using ReolMarkedWPF.Models;
using System.Collections.Generic;

namespace ReolMarkedWPF.Repositories
{
    public interface IShelfVendorRepository
    {
        List<ShelfVendor> GetAllShelfVendors();
        void AddShelfVendor(ShelfVendor shelfVendor);
        void DeleteShelfVendor(ShelfVendor shelfVendor);
        void UpdateShelfVendor(ShelfVendor shelfVendor);
    }
}