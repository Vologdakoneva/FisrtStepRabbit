using Microsoft.EntityFrameworkCore;
using PacientService.Data;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;
//using PromedExchange;

namespace PacientService.Repositories.Entities
{
    public class PersonsRepository : IPerson
    {
        private readonly PacientDbContext context;

        public PersonsRepository(PacientDbContext context)
        {
            this.context = context;
        }
        public void DeletePersons(Guid GuidPerson)
        {
            throw new NotImplementedException();
        }

        public Person GetPersonByEntity(Guid entity)
        {
            return (Person)context.Person.Where(p=>p.PersonLink==entity);
        }

        public IQueryable<Person> GetPersons()
        {
            return (IQueryable<Person>)context.Person.OrderByDescending(p=>p.DateChangePerson);
        }

        public void SavePersons(Person entity)
        {
            if (entity.IDALL == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified; 
            context.SaveChanges();
        }
    }
}
