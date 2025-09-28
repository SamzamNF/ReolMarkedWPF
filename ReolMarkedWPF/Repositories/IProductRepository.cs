using ReolMarkedWPF.Models;
using System.Collections.Generic;

namespace ReolMarkedWPF.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAllProducts();
        public void AddProduct(Product product);
        public void DeleteProduct(Product product);
        public void UpdateProduct(Product product);
    }
}