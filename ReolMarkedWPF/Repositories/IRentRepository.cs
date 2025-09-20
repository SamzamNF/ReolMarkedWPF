using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    // Interface til at implementere SQL database som gemmer "rent". Kan også bruges til at implementere memory versioner til UnitTesting
    // Løst koblet interface, som henviser til en klasse den bruger, som man definere når interfaced bliver implementeret
    public interface IRentRepository<T> where T : class
    {
        List<T> GetAllRents();
        void AddRent(T rent);
        void DeleteRent(T rent);
        void UpdateRent(T rent);
    }
}
