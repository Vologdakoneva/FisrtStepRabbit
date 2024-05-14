using DocumentService.Entities;

namespace DocumentService.Repositories.Interfaces
{
    public interface ItelegramChat
    {
        IQueryable<telegramChat> GetChatAll();
        telegramChat GetChatbeUser(string user);
        void SaveChat(telegramChat entity);
        void DeleteChat(telegramChat entity);
    }
}
