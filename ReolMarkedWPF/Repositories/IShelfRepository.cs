using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReolMarkedWPF.Model;

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
