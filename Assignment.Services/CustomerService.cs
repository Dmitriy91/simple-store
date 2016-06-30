﻿using Assignment.Data.Repositories;
using Assignment.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Assignment.Data;

namespace Assignment.Services
{
    public class CustomerService : ICustomerService
    {
        #region Fields
        private IRepository<Customer> _customerRepo;
        private IRepository<JuridicalPerson> _juridicalPersonRepo;
        private IRepository<NaturalPerson> _naturalPersonRepo;
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

        public IEnumerable<JuridicalPerson> GetAllJuridicalPersons()
        {
            return _juridicalPersonRepo.GetAll()
                .Include(jp => jp.Customer)
                .ToList();
        }

        public IEnumerable<NaturalPerson> GetAllNaturalPersons()
        {
            return _naturalPersonRepo.GetAll()
                .Include(np => np.Customer)
                .ToList();
        }

        public bool RemovePersonById(int customerId)
        {
            bool customerExists = _customerRepo.Exists(c => c.Id == customerId);

            if (customerExists)
            {
                _customerRepo.Delete(new Customer { Id = customerId });
                return true;
            }

            return false;
        }

        public bool UpdateJuridicalPerson(JuridicalPerson juridicalPerson)
        {
            bool jpExists = _juridicalPersonRepo.Exists(jp => jp.CustomerId == juridicalPerson.CustomerId);

            if (jpExists)
            {
                _customerRepo.Update(juridicalPerson.Customer);
                _juridicalPersonRepo.Update(juridicalPerson);
                return true;
            }

            return false;
        }

        public bool UpdateNaturalPerson(NaturalPerson naturalPerson)
        {
            bool npExists = _naturalPersonRepo.Exists(np => np.CustomerId == naturalPerson.CustomerId);

            if (npExists)
            {
                _customerRepo.Update(naturalPerson.Customer);
                _naturalPersonRepo.Update(naturalPerson);
                return true;
            }

            return false;
        }

        public void AddJuridicalPerson(JuridicalPerson juridicalPerson)
        {
            _juridicalPersonRepo.Add(juridicalPerson);
        }

        public void AddNaturalPerson(NaturalPerson naturalPerson)
        {
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