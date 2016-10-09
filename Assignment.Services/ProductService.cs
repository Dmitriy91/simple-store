using Assignment.Data.Repositories;
using Assignment.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assignment.Data;
using System.Linq;

namespace Assignment.Services
{
    public class ProductService : IProductService
    {
        #region Fields
        private IRepository<Product> _productRepo;
        private IRepository<OrderDetails> _orderDetailsRepo;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public ProductService(IRepository<Product> productRepo,
            IRepository<OrderDetails> orderDetailsRepo,
            IUnitOfWork unitOfWork)
        {
            _productRepo = productRepo;
            _orderDetailsRepo = orderDetailsRepo;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public Product GetProductById(int productId)
        {
            return _productRepo.GetById(productId);
        }

        public IEnumerable<Product> GetProducts(int pageNumber, int pageSize, out int productsFound)
        {
            IQueryable<Product> products = null;

            products = _productRepo.GetAll();
            productsFound = products.Count();

            return products.OrderBy(p => p.Id).
                Skip((pageNumber - 1) * pageSize).
                Take(pageSize).
                ToList();
        }

        public bool RemoveProductById(int productId)
        {
            bool productExists = _productRepo.Exists(p => p.Id == productId);
            bool productIsOrdered = _orderDetailsRepo.Exists(od => od.ProductId == productId);

            if (productExists && !productIsOrdered)
            {
                _productRepo.Delete(new Product { Id = productId });
                return true;
            }

            return false;
        }

        public bool UpdateProduct(Product product)
        {
            bool productExists = _productRepo.Exists(p => p.Id == product.Id);

            if (productExists)
            {
                _productRepo.Update(product);
                return true;
            }

            return false;
        }

        public void AddProduct(Product product)
        {
            _productRepo.Add(product);
        }

        async public Task CommitAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_unitOfWork != null)
                {
                    _unitOfWork.Dispose();
                    _unitOfWork = null;
                }
            }
        }
        #endregion
    }
}
