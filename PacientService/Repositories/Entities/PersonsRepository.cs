using PacientService.Data;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;
using PromedExchange;

namespace PacientService.Repositories.Entities
{
    public class PersonsRepository : IPerson
    {
        private readonly PacientDbContext context;

        public PersonsRepository(PacientDbContext context)
        {
            this.context = context;
        }
        public void DeletePersons(string GuidPerson)
        {
            throw new NotImplementedException();
        }

        public Person GetPersonByEntity(Person entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Person> GetPersons()
        {
            return context.Person;
        }

        public void SavePersons(Person entity)
        {
            throw new NotImplementedException();
        }
    }
}
