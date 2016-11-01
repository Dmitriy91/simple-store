using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Infrastructure;
using Assignment.Web.Infrastructure.ExceptionHandling;
using Assignment.Web.Models;
using AutoMapper;
using WebApi.OutputCache.V2;

namespace Assignment.Web.Controllers
{
    [AutoInvalidateCacheOutput]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        #region Fields
        private IProductService _productService;
        #endregion

        #region Constructors
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Actions
        // GET: api/products/
        [CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> Get([FromUri]ProductFilterBM filter)
        {
            filter = filter ?? new ProductFilterBM();

            if(!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            int productsFound = 0;
            IFiltration filtration = Mapper.Map<ProductFilterBM, Filtration>(filter);
            IEnumerable<Product> products =
                await Task.Run(() => _productService.GetProducts(filtration, out productsFound));
            IEnumerable<ProductDTO> productDtos = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

            return Ok(new
            {
                Products = productDtos,
                PagingInfo = new PagingInfoDTO
                {
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalItems = productsFound
                }
            });
        }

        // GET: api/products/details/1
        [Route("details/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        public IHttpActionResult Get(int id)
        {
            Product product = _productService.GetProductById(id);

            if (product == null)
                throw new BindingModelValidationException("Invalid product id.");

            ProductDTO productDto = Mapper.Map<Product, ProductDTO>(product);

            return Ok(productDto);
        }

        // POST: api/products/update
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update([FromBody]ProductBM productBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            Product product = Mapper.Map<ProductBM, Product>(productBindingModel);

            if (_productService.UpdateProduct(product))
                await _productService.CommitAsync();

            return Ok();
        }

        // POST: api/products/add
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> Add([FromBody]ProductBM productBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            Product product = Mapper.Map<ProductBM, Product>(productBindingModel);

            _productService.AddProduct(product);
            await _productService.CommitAsync();

            return Ok();
        }

        // POST: api/products/delete/1
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_productService.RemoveProductById(id))
                await _productService.CommitAsync();

            return Ok();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_productService != null)
                {
                    _productService.Dispose();
                    _productService = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
