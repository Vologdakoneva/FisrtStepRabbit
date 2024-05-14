using DocumentService.Data;
using DocumentService.Entities;
using DocumentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Repositories.Entities
{
    public class telegramChatReposytory : ItelegramChat
    {
        private readonly DocumentDbContext context;

        public telegramChatReposytory(DocumentDbContext context)
        {
            this.context = context;
        }
        public void DeleteChat(DocumentService.Entities.telegramChat entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DocumentService.Entities.telegramChat> GetChatAll()
        {
             return context.ChatId;
            
        }

        public DocumentService.Entities.telegramChat GetChatbeUser(string user)
        {
            telegramChat? tasks = context.ChatId.Where(p => p.Username == user).FirstOrDefault();
            return tasks;
        }

        public void SaveChat(DocumentService.Entities.telegramChat entity)
        {
            if (entity.Username == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
