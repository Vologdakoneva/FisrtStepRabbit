using MobileApp.Entities;

namespace MobileApp.Iterfaces
{
    public interface IUsers
    {
        IQueryable<UserApp> GetUsers();
        UserApp GetUserAppByPhone(string phonenumber);
        void SaveUserApp(UserApp entity);
        void DeleteUserApp(string phone);
    }
}
