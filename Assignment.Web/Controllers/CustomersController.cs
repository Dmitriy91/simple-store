using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Assignment.Web.Infrastructure.ExceptionHandling;
using Assignment.Web.Infrastructure;
using WebApi.OutputCache.V2;

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
        public async Task<IHttpActionResult> GetAllJuridicalPersons([FromUri]PaginationBindingModel pagination)
        {
            pagination = pagination ?? new PaginationBindingModel();

            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            int personsFound = 0;

            IEnumerable<JuridicalPerson> juridcalPersons =
                await Task.Run(() => _customerService.GetJuridicalPersons(pagination.PageNumber, pagination.PageSize, out personsFound));
            IEnumerable<JuridicalPersonDto> juridicalPersonDtos =
                Mapper.Map<IEnumerable<JuridicalPerson>, IEnumerable<JuridicalPersonDto>>(juridcalPersons);

            return Ok(new
            {
                JuridicalPersons = juridicalPersonDtos,
                PagingInfo = new PagingInfoDto
                {
                    CurrentPage = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalItems = personsFound
                }
            });
        }

        // GET: api/customers/juridical-persons
        [Route("natural-persons")]
        [CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GetAllNaturalPersons([FromUri]PaginationBindingModel pagination)
        {
            pagination = pagination ?? new PaginationBindingModel();

            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            int personsFound = 0;

            IEnumerable<NaturalPerson> naturalPersons =
                await Task.Run(() => _customerService.GetNaturalPersons(pagination.PageNumber, pagination.PageSize, out personsFound));
            IEnumerable<NaturalPersonDto> naturalPersonDtos =
                Mapper.Map<IEnumerable<NaturalPerson>, IEnumerable<NaturalPersonDto>>(naturalPersons);

            return Ok(new
            {
                NaturalPersons = naturalPersonDtos,
                PagingInfo = new PagingInfoDto
                {
                    CurrentPage = pagination.PageNumber,
                    PageSize = pagination.PageSize,
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

            JuridicalPersonDto juridicalPersonDto = Mapper.Map<JuridicalPerson, JuridicalPersonDto>(juridicalPerson);

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

            NaturalPersonDto naturalPersonDto = Mapper.Map<NaturalPerson, NaturalPersonDto>(naturalPerson);

            return Ok(naturalPersonDto);
        }

        // POST: api/customers/juridical-person/update
        [HttpPost]
        [Route("juridical-person/update")]
        public async Task<IHttpActionResult> UpdateJuridicalPerson([FromBody]JuridicalPersonBindingModel juridicalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            JuridicalPerson juridicalPerson = Mapper.Map<JuridicalPersonBindingModel, JuridicalPerson>(juridicalPersonBindingModel);

            if (_customerService.UpdateJuridicalPerson(juridicalPerson))
                await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/juridical-person/update
        [HttpPost]
        [Route("natural-person/update")]
        public async Task<IHttpActionResult> UpdateNaturalPerson([FromBody]NaturalPersonBindingModel naturalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            NaturalPerson naturalPerson = Mapper.Map<NaturalPersonBindingModel, NaturalPerson>(naturalPersonBindingModel);

            if (_customerService.UpdateNaturalPerson(naturalPerson))
                await _customerService.CommitAsync();

            return Ok();
        }

        // POST: api/customers/juridical-person/add
        [HttpPost]
        [Route("juridical-person/add")]
        public async Task<IHttpActionResult> AddJuridicalPerson([FromBody]JuridicalPersonBindingModel juridicalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            JuridicalPerson juridicalPerson = Mapper.Map<JuridicalPersonBindingModel, JuridicalPerson>(juridicalPersonBindingModel);

            if (_customerService.AddJuridicalPerson(juridicalPerson))
                await _customerService.CommitAsync();
            else
                throw new BindingModelValidationException("Already exists.");

            return Ok();
        }

        // POST: api/customers/natural-person/add
        [HttpPost]
        [Route("natural-person/add")]
        public async Task<IHttpActionResult> AddNaturalPerson([FromBody]NaturalPersonBindingModel naturalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            NaturalPerson naturalPerson = Mapper.Map<NaturalPersonBindingModel, NaturalPerson>(naturalPersonBindingModel);

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
