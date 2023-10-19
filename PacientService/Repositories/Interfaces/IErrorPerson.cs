using PacientService.Entities;

namespace PacientService.Repositories.Interfaces
{
    public interface IErrorPerson
    {
            IQueryable<ErrorPerson> GetErrors();
            ErrorPerson GetErrorEntity(ErrorPerson entity);
            void SaveGetError(ErrorPerson entity);
            void DeleteGetError(string GuidError);

    }
}
