using PacientService.Data;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;

namespace PacientService.Repositories.Entities
{
    public class ErrorPersonRepository : IErrorPerson
    {
        private readonly PacientDbContext context;

        public ErrorPersonRepository(PacientDbContext context)
        {
            this.context = context;
        }

        public void DeleteGetError(string GuidError)
        {
            throw new NotImplementedException();
        }

        public ErrorPerson GetErrorEntity(ErrorPerson entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ErrorPerson> GetErrors()
        {
            return context.ErrorPerson; 
        }

        public void SaveGetError(ErrorPerson entity)
        {
            throw new NotImplementedException();
        }
    }
}
