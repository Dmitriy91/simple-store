using Assignment.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment.Services
{
    public interface IProductService : IDisposable
    {
        Product GetProductById(int customerId);

        IEnumerable<Product> GetAllProducts();

        bool RemoveProductById(int productId);

        bool UpdateProduct(Product product);

        void AddProduct(Product product);

        Task CommitAsync();

        void Commit();
    }
}
