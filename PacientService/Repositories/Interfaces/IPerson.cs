using PacientService.Entities;

namespace PacientService.Repositories.Interfaces
{
    public interface IPerson
    {
            IQueryable<Person> GetPersons();
            Person GetPersonByEntity(Person entity);
            void SavePersons(Person entity);
            void DeletePersons(string GuidPerson);

    }
}
