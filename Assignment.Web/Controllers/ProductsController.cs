using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Assignment.Web.Controllers
{
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
        public async Task<IHttpActionResult> Get()
        {
            IEnumerable<Product> products = await Task<IEnumerable<Product>>.Run(() =>
            {
                return _productService.GetAllProducts();
            });

            IEnumerable<ProductDto> productsDtos = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            return Ok(productsDtos);
        }

        // GET: api/products/details/1
        [Route("details/{id:int:min(1)}")]
        public IHttpActionResult Get(int id)
        {

            Product product = _productService.GetProductById(id);

            if (product == null)
                return BadRequest();

            ProductDto productDto = Mapper.Map<Product, ProductDto>(product);

            return Ok(productDto);
        }

        // POST: api/products/update/1
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update([FromBody]ProductBindingModel productBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product product = Mapper.Map<ProductBindingModel, Product>(productBindingModel);

            if (_productService.UpdateProduct(product))
            {
                await _productService.CommitAsync();
                return Ok();
            }

            return BadRequest();
        }

        // POST: api/products/add
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> Add([FromBody]ProductBindingModel productBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product product = Mapper.Map<ProductBindingModel, Product>(productBindingModel);

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
            {
                await _productService.CommitAsync();
                return Ok();
            }

            return BadRequest();
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
