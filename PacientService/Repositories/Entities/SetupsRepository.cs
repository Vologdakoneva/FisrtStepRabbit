using PacientService.Data;
using PacientService.Entities;
using PacientService.Repositories.Interfaces;

namespace PacientService.Repositories.Entities
{
    public class SetupsRepository : ISetups
    {
        private readonly PacientDbContext context;

        public SetupsRepository(PacientDbContext context)
        {
            this.context = context;
        }
        public void DeleteSetup(string Namesetup)
        {
            
        }

        public Setups GetSetupByName(string Namesetup)
        {
            Setups? set = context.Setups.FirstOrDefault(st => st.Namenastr == Namesetup);
            if (set == null)
            {
                set = new Setups();
            }
            return set;
        }

        public IQueryable<Setups> GetSetups()
        {
            return context.Setups;
        }

        public void SaveSetup(Setups entity)
        {
            
        }
    }
}
