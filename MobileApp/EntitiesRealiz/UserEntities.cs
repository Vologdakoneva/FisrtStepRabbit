using Microsoft.EntityFrameworkCore;
using MobileApp.Data;
using MobileApp.Entities;
using MobileApp.Iterfaces;

namespace MobileApp.EntitiesRealiz
{
    public class UserEntities : IUsers
    {
        private readonly MobileDbContext context;

        public UserEntities(MobileDbContext context)
        {
            this.context = context;
        }
        public void DeleteUserApp(string phone)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserApp> GetUsers()
        {
            var usr = (IQueryable<UserApp>)context.UsersApp;
            foreach (var userApps in usr)
            {
                userApps.password = EncryptionHelper.Encrypt(userApps.password);
            }
            return (IQueryable<UserApp>)usr;
        }

        public UserApp GetUserAppByPhone(string phonenumber)
        {
            
            UserApp? userapp = context.UsersApp.Where(p => p.phone == phonenumber).FirstOrDefault();
            if (userapp == null) { return new UserApp(); }
            else
            {
                userapp.password = EncryptionHelper.Encrypt(userapp.password);
                if (userapp.phone != phonenumber)
                    return new UserApp();
                else
                    return userapp;
            }
        }

        public void SaveUserApp(UserApp entity)
        {
            UserApp? userapp = context.UsersApp.Where(p => p.phone == entity.phone).FirstOrDefault();
            if (userapp == null)
                context.Entry(entity).State = EntityState.Added;
            else
            {
                var entityProperties = userapp.GetType().GetProperties();
                context.Entry(userapp).State = EntityState.Modified;
                context.UsersApp.Attach(userapp);
                foreach (var ep in entityProperties)
                {
                    if (ep.Name != "phone")
                    {
                        context.Entry(userapp).Property(ep.Name).IsModified = true;
                        context.Entry(userapp).Property(ep.Name).CurrentValue = context.Entry(entity).Property(ep.Name).CurrentValue;
                    }
                }
                
                context.Entry(userapp).State = EntityState.Modified;

            }
            context.SaveChanges();
        }
    }
}
