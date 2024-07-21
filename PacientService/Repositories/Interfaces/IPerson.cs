using PacientService.Entities;
//using PromedExchange;

namespace PacientService.Repositories.Interfaces
{
    public interface IPerson
    {
        IQueryable<Person> GetPersons();
        IQueryable<Person> GetPersonsиеBtSnils(string snils, DateTime datarogd);
        Person GetPersonByEntity(Guid entity);
        void SavePersons(Person entity);
        void DeletePersons(Guid GuidPerson);

    }
}
