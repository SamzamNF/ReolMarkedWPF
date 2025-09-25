using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    internal interface IProductRepository
    {
        public List<Product> GetAllProducts();
        public void AddProduct(Product product);
        public void DeleteProduct(Product product);
        public void UpdateProduct(Product product);
    }
}
