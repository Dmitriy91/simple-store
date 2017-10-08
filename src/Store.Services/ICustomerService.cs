using Store.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Services
{
    public interface ICustomerService : IDisposable
    {
        JuridicalPerson GetJuridicalPersonById(int customerId);

        NaturalPerson GetNaturalPersonById(int customerId);

        IEnumerable<JuridicalPerson> GetJuridicalPersons(IFiltration filtration, out int personsFound);

        IEnumerable<NaturalPerson> GetNaturalPersons(IFiltration filtration, out int personsFound);

        void RemovePersonById(int customerId);

        void UpdateJuridicalPerson(JuridicalPerson juridicalPerson);

        void UpdateNaturalPerson(NaturalPerson naturalPerson);

        void AddJuridicalPerson(JuridicalPerson juridicalPerson);

        void AddNaturalPerson(NaturalPerson naturalPerson);

        Task CommitAsync();

        void Commit();
    }
}
