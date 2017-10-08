using Store.Data.Repositories;
using Store.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Store.Data;
using System.Linq;
using System.Linq.Dynamic;
using System;

namespace Store.Services
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<OrderDetails> _orderDetailsRepo;
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

        public IEnumerable<Product> GetProducts(IFiltration filtration, out int productsFound)
        {
            IQueryable<Product> products = null;
            string productName = filtration["ProductName"];
            string sortBy = filtration.SortBy;

            if (productName == null)
                products = _productRepo.GetAll();
            else
                products = _productRepo.GetMany(p => p.ProductName.Contains(productName));

            productsFound = products.Count();

            if (sortBy == null)
                products = products.OrderBy(p => p.Id);
            else
                products = products.OrderBy(sortBy);

            return products.Paginate(filtration.PageNumber, filtration.PageSize);
        }

        public void RemoveProductById(int productId)
        {
            bool productExists = _productRepo.Exists(p => p.Id == productId);
            bool productIsOrdered = _orderDetailsRepo.Exists(od => od.ProductId == productId);

            if (!productExists || productIsOrdered)
                throw new ApplicationException("The product doesn't exist or it's ordered");

            _productRepo.Delete(new Product { Id = productId });
        }

        public void UpdateProduct(Product product)
        {
            bool productExists = _productRepo.Exists(p => p.Id == product.Id);

            if (!productExists)
                throw new ApplicationException("The product doesn't exist");

            _productRepo.Update(product);
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
