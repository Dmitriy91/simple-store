using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Assignment.Web.Controllers
{
    [Authorize]
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
        public async Task<IHttpActionResult> GetAllJuridicalPersons()
        {
            IEnumerable<JuridicalPerson> juridcalPersons = await Task<IEnumerable<JuridicalPerson>>.Run(() =>
            {
                return _customerService.GetAllJuridicalPersons();
            });

            IEnumerable<JuridicalPersonDto> juridicalPersonDtos = Mapper.Map<IEnumerable<JuridicalPerson>, IEnumerable<JuridicalPersonDto>>(juridcalPersons);

            return Ok(juridicalPersonDtos);
        }

        // GET: api/customers/juridical-persons
        [Route("natural-persons")]
        public async Task<IHttpActionResult> GetAllNaturalPersons()
        {
            IEnumerable<NaturalPerson> naturalPersons = await Task<IEnumerable<JuridicalPerson>>.Run(() =>
            {
                return _customerService.GetAllNaturalPersons();
            });

            IEnumerable<NaturalPersonDto> naturalPersonDtos = Mapper.Map<IEnumerable<NaturalPerson>, IEnumerable<NaturalPersonDto>>(naturalPersons);

            return Ok(naturalPersonDtos);
        }

        // GET: api/customers/juridical-person/1
        [Route("juridical-person/{id:int:min(1)}")]
        public IHttpActionResult GetJuridicalPerson(int id)
        {
            JuridicalPerson juridicalPerson = _customerService.GetJuridicalPersonById(id);

            if (juridicalPerson == null)
                return BadRequest();

            JuridicalPersonDto juridicalPersonDto = Mapper.Map<JuridicalPerson, JuridicalPersonDto>(juridicalPerson);

            return Ok(juridicalPersonDto);
        }

        // GET: api/customers/natural-person/1
        [Route("natural-person/{id:int:min(1)}")]
        public IHttpActionResult GetNaturalPerson(int id)
        {
            NaturalPerson naturalPerson = _customerService.GetNaturalPersonById(id);

            if (naturalPerson == null)
                return BadRequest();

            NaturalPersonDto naturalPersonDto = Mapper.Map<NaturalPerson, NaturalPersonDto>(naturalPerson);

            return Ok(naturalPersonDto);
        }

        // POST: api/customers/juridical-person/update/1
        [HttpPost]
        [Route("juridical-person/update")]
        public async Task<IHttpActionResult> UpdateJuridicalPerson([FromBody]JuridicalPersonBindingModel juridicalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            JuridicalPerson juridicalPerson = Mapper.Map<JuridicalPersonBindingModel, JuridicalPerson>(juridicalPersonBindingModel);

            if (_customerService.UpdateJuridicalPerson(juridicalPerson))
            {
                await _customerService.CommitAsync();
                return Ok();
            }

            return BadRequest();
        }

        // POST: api/customers/juridical-person/update/1
        [HttpPost]
        [Route("natural-person/update")]
        public async Task<IHttpActionResult> UpdateNaturalPerson([FromBody]NaturalPersonBindingModel naturalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            NaturalPerson naturalPerson = Mapper.Map<NaturalPersonBindingModel, NaturalPerson>(naturalPersonBindingModel);

            if (_customerService.UpdateNaturalPerson(naturalPerson))
            {
                await _customerService.CommitAsync();
                return Ok();
            }

            return BadRequest();
        }

        // POST: api/customers/juridical-person/add
        [HttpPost]
        [Route("juridical-person/add")]
        public async Task<IHttpActionResult> AddJuridicalPerson([FromBody]JuridicalPersonBindingModel juridicalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            JuridicalPerson juridicalPerson = Mapper.Map<JuridicalPersonBindingModel, JuridicalPerson>(juridicalPersonBindingModel);

            if (_customerService.AddJuridicalPerson(juridicalPerson))
            {
                await _customerService.CommitAsync();
                return Ok();
            }

            return BadRequest();
        }

        // POST: api/customers/natural-person/add
        [HttpPost]
        [Route("natural-person/add")]
        public async Task<IHttpActionResult> AddNaturalPerson([FromBody]NaturalPersonBindingModel naturalPersonBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            NaturalPerson naturalPerson = Mapper.Map<NaturalPersonBindingModel, NaturalPerson>(naturalPersonBindingModel);

            if (_customerService.AddNaturalPerson(naturalPerson))
            {
                await _customerService.CommitAsync();
                return Ok();
            }

            return BadRequest();
        }

        // POST: api/customers/delete/1
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_customerService.RemovePersonById(id))
            {
                await _customerService.CommitAsync();

                return Ok();
            }

            return BadRequest();
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
