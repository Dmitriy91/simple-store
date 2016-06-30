using Assignment.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment.Services
{
    public interface ICustomerService : IDisposable
    {
        JuridicalPerson GetJuridicalPersonById(int customerId);

        NaturalPerson GetNaturalPersonById(int customerId);

        IEnumerable<JuridicalPerson> GetAllJuridicalPersons();

        IEnumerable<NaturalPerson> GetAllNaturalPersons();

        bool RemovePersonById(int customerId);

        bool UpdateJuridicalPerson(JuridicalPerson juridicalPerson);

        bool UpdateNaturalPerson(NaturalPerson naturalPerson);

        void AddJuridicalPerson(JuridicalPerson juridicalPerson);

        void AddNaturalPerson(NaturalPerson naturalPerson);

        Task CommitAsync();

        void Commit();
    }
}
