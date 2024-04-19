using DocumentService.Data;
using DocumentService.Entities;
using DocumentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Repositories.Entities
{
    public class TaskRepository : ITask
    {
        private readonly DocumentDbContext context;

        public TaskRepository(DocumentDbContext context)
        {
            this.context = context;
        }
        public void DeleteTasks(Guid GuidTasks)
        {
            throw new NotImplementedException();
        }

        public UserTasks GetTaskEntity(Guid entity)
        {
            UserTasks? UserTask = this.context.UserTask.Where(p => p.DocLink == entity).FirstOrDefault();
            if (UserTask == null) { return new UserTasks(); }
            else
                return UserTask;
        }

        public IQueryable<UserTasks> GetTasks()
        {
            return  context.UserTask.OrderByDescending(p => p.DataTask); ;
        }

        public void SaveTasks(DocAnaliz entity)
        {
            if (entity.IDALL == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
