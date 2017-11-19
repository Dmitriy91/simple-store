using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Store.Services;
using Store.Web.Infrastructure.ExceptionHandling;
using DTO = Store.Contracts;
using BM = Store.Web.Models.BM;
using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using WebApi.OutputCache.V2;
using Store.Web.Infrastructure.ValidationAttributes;

namespace Store.Web.Controllers.V1
{
    /// <summary>
    /// Customers
    /// </summary>
    [AutoInvalidateCacheOutput]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/{apiVersion}/customers")]
    public class CustomersController : ApiController
    {
        #region Fields
        private ICustomerService _customerService;
        private IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="customerService"></param>
        /// <param name="mapper"></param>
        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        #endregion

        #region Actions
        // GET: api/customers/juridical-persons
        /// <summary>
        /// Get juridical persons
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("juridical-persons")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.PaginatedData<DTO.JuridicalPerson>))]
        [ModelStateValidation]
        public async Task<IHttpActionResult> GetJuridicalPersons([FromUri]BM.JuridicalPersonFilter filter)
        {
            int personsFound = 0;
            IFiltration filtration = _mapper.Map<BM.JuridicalPersonFilter, Filtration>(filter);
            IEnumerable<Entities.JuridicalPerson> juridcalPersons =
                await Task.Run(() => _customerService.GetJuridicalPersons(filtration, out personsFound));
            IEnumerable<DTO.JuridicalPerson> juridicalPersonDtos =
                _mapper.Map<IEnumerable<Entities.JuridicalPerson>, IEnumerable<DTO.JuridicalPerson>>(juridcalPersons);

            var paginatedData = new DTO.PaginatedData<DTO.JuridicalPerson>
            {
                Collection = juridicalPersonDtos,
                PagingInfo = new DTO.PagingInfo
                {
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalItems = personsFound
                }
            };

            return Ok(paginatedData);
        }

        // GET: api/customers/natural-persons
        /// <summary>
        /// Get natural persons
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("natural-persons")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.PaginatedData<DTO.NaturalPerson>))]
        [ModelStateValidation]
        public async Task<IHttpActionResult> GetNaturalPersons([FromUri]BM.NaturalPersonFilter filter)
        {
            int personsFound = 0;
            IFiltration filtration = _mapper.Map<BM.NaturalPersonFilter, Filtration>(filter);
            IEnumerable<Entities.NaturalPerson> naturalPersons =
                await Task.Run(() => _customerService.GetNaturalPersons(filtration, out personsFound));
            IEnumerable<DTO.NaturalPerson> naturalPersonDtos =
                _mapper.Map<IEnumerable<Entities.NaturalPerson>, IEnumerable<DTO.NaturalPerson>>(naturalPersons);

            var paginatedData = new DTO.PaginatedData<DTO.NaturalPerson>
            {
                Collection = naturalPersonDtos,
                PagingInfo = new DTO.PagingInfo
                {
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalItems = personsFound
                }
            };

            return Ok(paginatedData);
        }

        // GET: api/customers/juridical-person/1
        /// <summary>
        /// Get juridical person by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [Route("juridical-person/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.JuridicalPerson))]
        public IHttpActionResult GetJuridicalPerson(int id)
        {
            Entities.JuridicalPerson juridicalPerson = _customerService.GetJuridicalPersonById(id);

            if (juridicalPerson == null)
                throw new BindingModelValidationException("Invalid juridical person id.");

            DTO.JuridicalPerson juridicalPersonDto = _mapper.Map<Entities.JuridicalPerson, DTO.JuridicalPerson>(juridicalPerson);

            return Ok(juridicalPersonDto);
        }

        // GET: api/customers/natural-person/1
        /// <summary>
        /// Get natural person by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("natural-person/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.NaturalPerson))]
        public IHttpActionResult GetNaturalPerson(int id)
        {
            Entities.NaturalPerson naturalPerson = _customerService.GetNaturalPersonById(id);

            if (naturalPerson == null)
                throw new BindingModelValidationException("Invalid natural person id.");

            DTO.NaturalPerson naturalPersonDto = _mapper.Map<Entities.NaturalPerson, DTO.NaturalPerson>(naturalPerson);

            return Ok(naturalPersonDto);
        }

        // POST: api/customers/juridical-person/update
        /// <summary>
        /// Update juridical person
        /// </summary>
        /// <param name="juridicalPerson"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("juridical-person/update")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> UpdateJuridicalPerson([FromBody]BM.JuridicalPerson juridicalPerson)
        {
            Entities.JuridicalPerson juridicalPersonEntity = _mapper.Map<BM.JuridicalPerson, Entities.JuridicalPerson>(juridicalPerson);

            _customerService.UpdateJuridicalPerson(juridicalPersonEntity);
            await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/natural-person/update
        /// <summary>
        /// Update natural person
        /// </summary>
        /// <param name="naturalPerson"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("natural-person/update")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> UpdateNaturalPerson([FromBody]BM.NaturalPerson naturalPerson)
        {
            Entities.NaturalPerson naturalPersonEntity = _mapper.Map<BM.NaturalPerson, Entities.NaturalPerson>(naturalPerson);

            _customerService.UpdateNaturalPerson(naturalPersonEntity);
            await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/juridical-person/add
        /// <summary>
        /// Add juridical person
        /// </summary>
        /// <param name="juridicalPerson"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("juridical-person/add")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> AddJuridicalPerson([FromBody]BM.JuridicalPerson juridicalPerson)
        {
            Entities.JuridicalPerson juridicalPersonEntity = _mapper.Map<BM.JuridicalPerson, Entities.JuridicalPerson>(juridicalPerson);

            _customerService.AddJuridicalPerson(juridicalPersonEntity);
            await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/natural-person/add
        /// <summary>
        /// Add natural person
        /// </summary>
        /// <param name="naturalPerson"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("natural-person/add")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> AddNaturalPerson([FromBody]BM.NaturalPerson naturalPerson)
        {
            Entities.NaturalPerson naturalPersonEntity = _mapper.Map<BM.NaturalPerson, Entities.NaturalPerson>(naturalPerson);

            _customerService.AddNaturalPerson(naturalPersonEntity);
            await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/delete/1
        /// <summary>
        /// Delete customer by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            _customerService.RemovePersonById(id);
            await _customerService.CommitAsync();

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
                if (_customerService != null)
                {
                    _customerService.Dispose();
                    _customerService = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
