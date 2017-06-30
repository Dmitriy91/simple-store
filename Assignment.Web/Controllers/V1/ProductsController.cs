using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Assignment.Services;
using Assignment.Web.Infrastructure.ExceptionHandling;
using BM = Assignment.Web.Models.BM;
using DTO = Assignment.Web.Models.DTO;
using AutoMapper;
using WebApi.OutputCache.V2;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Controllers.V1
{
    /// <summary>
    /// Products
    /// </summary>
    [AutoInvalidateCacheOutput]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/{apiVersion}/products")]
    public class ProductsController : ApiController
    {
        #region Fields
        private IProductService _productService;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Actions
        // GET: api/products/
        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.PaginatedData<DTO.Product>))]
        [ModelStateValidation]
        public async Task<IHttpActionResult> Get([FromUri]BM.ProductFilter filter)
        {
            int productsFound = 0;
            IFiltration filtration = Mapper.Map<BM.ProductFilter, Filtration>(filter);
            IEnumerable<Entities.Product> products =
                await Task.Run(() => _productService.GetProducts(filtration, out productsFound));
            IEnumerable<DTO.Product> productDtos = Mapper.Map<IEnumerable<Entities.Product>, IEnumerable<DTO.Product>>(products);

            var paginatedData = new DTO.PaginatedData<DTO.Product>
            {
                Collection = productDtos,
                PagingInfo = new DTO.PagingInfo
                {
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalItems = productsFound
                }
            };

            return Ok(paginatedData);
        }

        // GET: api/products/details/1
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("details/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<DTO.Product>))]
        [ModelStateValidation]
        public IHttpActionResult Get(int id)
        {
            Entities.Product product = _productService.GetProductById(id);

            if (product == null)
                throw new BindingModelValidationException("Invalid product id.");

            DTO.Product productDto = Mapper.Map<Entities.Product, DTO.Product>(product);

            return Ok(productDto);
        }

        // POST: api/products/update
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> Update([FromBody]BM.Product product)
        {
            Entities.Product productEntity = Mapper.Map<BM.Product, Entities.Product>(product);

            _productService.UpdateProduct(productEntity);
            await _productService.CommitAsync();

            return Ok();
        }

        // POST: api/products/add
        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> Add([FromBody]BM.Product product)
        {
            Entities.Product productEntity = Mapper.Map<BM.Product, Entities.Product>(product);

            _productService.AddProduct(productEntity);
            await _productService.CommitAsync();

            return Ok();
        }

        // POST: api/products/delete/1
        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            _productService.RemoveProductById(id);
            await _productService.CommitAsync();

            return Ok();
        }
        #endregion

        /// <summary>
        /// Dispose related resources
        /// </summary>
        /// <param name="disposing"></param>
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
