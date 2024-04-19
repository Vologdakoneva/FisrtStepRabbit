using DocumentService.Entities;

namespace DocumentService.Repositories.Interfaces
{
    public interface ITask
    {
        IQueryable<UserTasks> GetTasks();
        UserTasks GetTaskEntity(Guid entity);
        void SaveTasks(DocAnaliz entity);
        void DeleteTasks(Guid GuidTasks);
    }
}

