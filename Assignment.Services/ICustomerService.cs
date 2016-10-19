﻿using Assignment.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment.Services
{
    public interface ICustomerService : IDisposable
    {
        JuridicalPerson GetJuridicalPersonById(int customerId);

        NaturalPerson GetNaturalPersonById(int customerId);

        IEnumerable<JuridicalPerson> GetJuridicalPersons(int pageNumber, int pageSize, out int personsFound);

        IEnumerable<NaturalPerson> GetNaturalPersons(int pageNumber, int pageSize, out int personsFound);

        bool RemovePersonById(int customerId);

        bool UpdateJuridicalPerson(JuridicalPerson juridicalPerson);

        bool UpdateNaturalPerson(NaturalPerson naturalPerson);

        bool AddJuridicalPerson(JuridicalPerson juridicalPerson);

        bool AddNaturalPerson(NaturalPerson naturalPerson);

        Task CommitAsync();

        void Commit();
    }
}
