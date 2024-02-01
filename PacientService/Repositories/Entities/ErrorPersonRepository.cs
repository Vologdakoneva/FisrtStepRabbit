using Microsoft.EntityFrameworkCore;
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

        public IQueryable<ErrorPerson> GetErrorEntity(Guid entity)
        {
            return context.ErrorPerson.OrderByDescending(o => o.DataError).Where(e => e.PersonLink == entity);
        }

        public IQueryable<ErrorPerson> GetErrors()
        {
            return context.ErrorPerson.OrderByDescending(o => o.DataError); 
        }

        public void SaveGetError(ErrorPerson entity)
        {
            if (entity.IDALL == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
