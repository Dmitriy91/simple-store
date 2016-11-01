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
using static Assignment.Services.CustomerService;

namespace Assignment.Web.Controllers
{
    [AutoInvalidateCacheOutput]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        #region Fields
        private ICustomerService _customerService;
        #endregion

        #region Constructors
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion

        #region Actions
        // GET: api/customers/juridical-persons
        [Route("juridical-persons")]
        [CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GetJuridicalPersons([FromUri]JuridicalPersonFilterBM filter)
        {
            filter = filter ?? new JuridicalPersonFilterBM();

            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            int personsFound = 0;
            IFiltration filtration = Mapper.Map<JuridicalPersonFilterBM, Filtration>(filter);
            IEnumerable<JuridicalPerson> juridcalPersons =
                await Task.Run(() => _customerService.GetJuridicalPersons(filtration, out personsFound));
            IEnumerable<JuridicalPersonDTO> juridicalPersonDtos =
                Mapper.Map<IEnumerable<JuridicalPerson>, IEnumerable<JuridicalPersonDTO>>(juridcalPersons);

            return Ok(new
            {
                JuridicalPersons = juridicalPersonDtos,
                PagingInfo = new PagingInfoDTO
                {
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalItems = personsFound
                }
            });
        }

        // GET: api/customers/natural-persons
        [Route("natural-persons")]
        [CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GetNaturalPersons([FromUri]NaturalPersonFilterBM naturalPersonFilter)
        {
            naturalPersonFilter = naturalPersonFilter ?? new NaturalPersonFilterBM();

            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            int personsFound = 0;
            IFiltration jpFilter = Mapper.Map<NaturalPersonFilterBM, Filtration>(naturalPersonFilter);
            jpFilter = Mapper.Map<NaturalPersonFilterBM, Filtration>(naturalPersonFilter);
            IEnumerable<NaturalPerson> naturalPersons =
                await Task.Run(() => _customerService.GetNaturalPersons(jpFilter, out personsFound));
            IEnumerable<NaturalPersonDTO> naturalPersonDtos =
                Mapper.Map<IEnumerable<NaturalPerson>, IEnumerable<NaturalPersonDTO>>(naturalPersons);

            return Ok(new
            {
                NaturalPersons = naturalPersonDtos,
                PagingInfo = new PagingInfoDTO
                {
                    CurrentPage = jpFilter.PageNumber,
                    PageSize = jpFilter.PageSize,
                    TotalItems = personsFound
                }
            });
        }

        // GET: api/customers/juridical-person/1
        [Route("juridical-person/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        public IHttpActionResult GetJuridicalPerson(int id)
        {
            JuridicalPerson juridicalPerson = _customerService.GetJuridicalPersonById(id);

            if (juridicalPerson == null)
                throw new BindingModelValidationException("Invalid juridical person id.");

            JuridicalPersonDTO juridicalPersonDto = Mapper.Map<JuridicalPerson, JuridicalPersonDTO>(juridicalPerson);

            return Ok(juridicalPersonDto);
        }

        // GET: api/customers/natural-person/1
        [Route("natural-person/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        public IHttpActionResult GetNaturalPerson(int id)
        {
            NaturalPerson naturalPerson = _customerService.GetNaturalPersonById(id);

            if (naturalPerson == null)
                throw new BindingModelValidationException("Invalid natural person id.");

            NaturalPersonDTO naturalPersonDto = Mapper.Map<NaturalPerson, NaturalPersonDTO>(naturalPerson);

            return Ok(naturalPersonDto);
        }

        // POST: api/customers/juridical-person/update
        [HttpPost]
        [Route("juridical-person/update")]
        public async Task<IHttpActionResult> UpdateJuridicalPerson([FromBody]JuridicalPersonBM juridicalPersonBM)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            JuridicalPerson juridicalPerson = Mapper.Map<JuridicalPersonBM, JuridicalPerson>(juridicalPersonBM);

            if (_customerService.UpdateJuridicalPerson(juridicalPerson))
                await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/juridical-person/update
        [HttpPost]
        [Route("natural-person/update")]
        public async Task<IHttpActionResult> UpdateNaturalPerson([FromBody]NaturalPersonBM naturalPersonBM)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            NaturalPerson naturalPerson = Mapper.Map<NaturalPersonBM, NaturalPerson>(naturalPersonBM);

            if (_customerService.UpdateNaturalPerson(naturalPerson))
                await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/juridical-person/add
        [HttpPost]
        [Route("juridical-person/add")]
        public async Task<IHttpActionResult> AddJuridicalPerson([FromBody]JuridicalPersonBM juridicalPersonBM)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            JuridicalPerson juridicalPerson = Mapper.Map<JuridicalPersonBM, JuridicalPerson>(juridicalPersonBM);

            if (_customerService.AddJuridicalPerson(juridicalPerson))
                await _customerService.CommitAsync();
            else
                throw new BindingModelValidationException("Already exists.");

            return Ok();
        }

        // POST: api/customers/natural-person/add
        [HttpPost]
        [Route("natural-person/add")]
        public async Task<IHttpActionResult> AddNaturalPerson([FromBody]NaturalPersonBM naturalPersonBM)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            NaturalPerson naturalPerson = Mapper.Map<NaturalPersonBM, NaturalPerson>(naturalPersonBM);

            if (_customerService.AddNaturalPerson(naturalPerson))
                await _customerService.CommitAsync();
            else
                throw new BindingModelValidationException("Already exists.");

            return Ok();
        }

        // POST: api/customers/delete/1
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_customerService.RemovePersonById(id))
                await _customerService.CommitAsync();

            return Ok();
        }
        #endregion

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
