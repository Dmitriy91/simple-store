using Store.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Services
{
    public interface IProductService : IDisposable
    {
        Product GetProductById(int customerId);

        List<Product> GetProducts(IFiltration filtration, out int productsFound);

        void RemoveProductById(int productId);

        void UpdateProduct(Product product);

        void AddProduct(Product product);

        Task CommitAsync();

        void Commit();
    }
}
