using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Store.Data;
using Store.Data.Repositories;
using Store.Entities;

namespace Store.Services
{
    public class CustomerService : ICustomerService
    {
        #region Fields
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<JuridicalPerson> _juridicalPersonRepo;
        private readonly IRepository<NaturalPerson> _naturalPersonRepo;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public CustomerService(IRepository<Customer> customerRepo, 
            IRepository<JuridicalPerson> juridicalPersonRepo,
            IRepository<NaturalPerson> naturalPersonRepo,
            IUnitOfWork unitOfWork)
        {
            _customerRepo = customerRepo;
            _juridicalPersonRepo = juridicalPersonRepo;
            _naturalPersonRepo = naturalPersonRepo;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public  JuridicalPerson GetJuridicalPersonById(int customerId)
        {
            return _juridicalPersonRepo.GetAll()
                .Include(jp => jp.Customer)
                .FirstOrDefault(jp => jp.CustomerId == customerId);
        }

        public NaturalPerson GetNaturalPersonById(int customerId)
        {
            return _naturalPersonRepo.GetAll()
                .Include(np => np.Customer)
                .FirstOrDefault(np => np.CustomerId == customerId);
        }

        public List<JuridicalPerson> GetJuridicalPersons(IFiltration filtration, out int personsFound)
        {
            IQueryable<JuridicalPerson> juridicalPersons = null;
            StringBuilder whereClause = new StringBuilder();
            IList<object> filterValues = new List<object>();
            int filterValueInx = 0;

            if (filtration.Filters != null)
            {
                foreach (var kvp in filtration.Filters)
                {
                    filterValues.Add(kvp.Value);
                    whereClause.AppendFormat("{0}.Contains(@{1}) AND ", kvp.Key, filterValueInx);
                    filterValueInx++;
                }
            }

            if (whereClause.Length > 0)
            {
                whereClause.Remove(whereClause.Length - 5, 5); // Remove last ' AND '
                juridicalPersons = _juridicalPersonRepo.GetAll().
                Include(jp => jp.Customer).
                Where(whereClause.ToString(), filterValues.ToArray());
            }
            else
            {
                juridicalPersons = _juridicalPersonRepo.GetAll().Include(jp => jp.Customer);
            }

            personsFound = juridicalPersons.Count();

            if (filtration.SortBy == null)
                juridicalPersons = juridicalPersons.OrderBy(p => p.CustomerId);
            else
                juridicalPersons = juridicalPersons.OrderBy(filtration.SortBy);

            return juridicalPersons.Paginate(filtration.PageNumber, filtration.PageSize);
        }

        public List<NaturalPerson> GetNaturalPersons(IFiltration filtration, out int personsFound)
        {
            IQueryable<NaturalPerson> naturalPersons = null;
            StringBuilder whereClause = new StringBuilder();
            IList<object> filterVales = new List<object>();
            int filterValueInx = 0;

            if (filtration.Filters != null)
            {
                foreach (var kvp in filtration.Filters)
                {
                    if (kvp.Key == "Birthdate")
                    {
                        DateTime birthdate = DateTime.Parse(kvp.Value).Date;

                        whereClause.AppendFormat("{0}.Value == @{1} AND ", kvp.Key, filterValueInx);
                        filterVales.Add(birthdate);
                    }
                    else
                    {
                        whereClause.AppendFormat("{0}.Contains(@{1}) AND ", kvp.Key, filterValueInx);
                        filterVales.Add(kvp.Value);
                    }

                    filterValueInx++;
                }
            }

            if (whereClause.Length > 0)
            {
                whereClause.Remove(whereClause.Length - 5, 5); // Remove last ' AND '
                naturalPersons = _naturalPersonRepo.GetAll().
                Include(jp => jp.Customer).
                Where(whereClause.ToString(), filterVales.ToArray());
            }
            else
            {
                naturalPersons = _naturalPersonRepo.GetAll().Include(jp => jp.Customer);
            }

            personsFound = naturalPersons.Count();

            if (filtration.SortBy == null)
                naturalPersons = naturalPersons.OrderBy(jp => jp.CustomerId);
            else
                naturalPersons = naturalPersons.OrderBy(filtration.SortBy);

            return naturalPersons.Paginate(filtration.PageNumber, filtration.PageSize);
        }

        public void RemovePersonById(int customerId)
        {
            bool customerExists = _customerRepo.Exists(c => c.Id == customerId);

            if (!customerExists)
                throw new ApplicationException("The user doesn't exist");

            _customerRepo.Delete(new Customer { Id = customerId });
        }

        public void UpdateJuridicalPerson(JuridicalPerson juridicalPerson)
        {
            bool jpExists = _juridicalPersonRepo.Exists(jp => 
                jp.CustomerId != juridicalPerson.CustomerId &&
                jp.TIN == juridicalPerson.TIN);

            if (jpExists) // TIN is occupied
                throw new ApplicationException("The TIN is occupied");

            _customerRepo.Update(juridicalPerson.Customer);
            _juridicalPersonRepo.Update(juridicalPerson);
        }

        public void UpdateNaturalPerson(NaturalPerson naturalPerson)
        {
            bool npExists = _naturalPersonRepo.Exists(np =>
                np.CustomerId != naturalPerson.CustomerId &&
                np.FirstName == naturalPerson.FirstName &&
                np.LastName == naturalPerson.LastName &&
                np.MiddleName == naturalPerson.MiddleName);

            if (npExists)
                throw new ApplicationException("This person already exists");

            _customerRepo.Update(naturalPerson.Customer);
            _naturalPersonRepo.Update(naturalPerson);
        }

        public void AddJuridicalPerson(JuridicalPerson juridicalPerson)
        {
            bool jpExists = _juridicalPersonRepo.Exists(jp => jp.TIN == juridicalPerson.TIN);

            if (jpExists)
                throw new ApplicationException("This person already exists");

            _juridicalPersonRepo.Add(juridicalPerson);
        }

        public void AddNaturalPerson(NaturalPerson naturalPerson)
        {
            bool npExists = _naturalPersonRepo.Exists(np => 
                np.FirstName == naturalPerson.FirstName &&
                np.LastName == naturalPerson.LastName &&
                np.MiddleName == naturalPerson.MiddleName);

            if (npExists)
                throw new ApplicationException("This person already exists");

            _naturalPersonRepo.Add(naturalPerson);
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
