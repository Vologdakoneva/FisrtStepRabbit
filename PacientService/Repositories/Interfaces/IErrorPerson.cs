using PacientService.Entities;

namespace PacientService.Repositories.Interfaces
{
    public interface IErrorPerson
    {
            IQueryable<ErrorPerson> GetErrors();
            IQueryable<ErrorPerson> GetErrorEntity(Guid entity);
            void SaveGetError(ErrorPerson entity);
            void DeleteGetError(string GuidError);

    }
}
