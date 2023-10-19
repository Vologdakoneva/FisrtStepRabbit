using PacientService.Entities;
using static PacientService.Repositories.Interfaces.ISetups;

namespace PacientService.Repositories.Interfaces
{
    public interface ISetups
    {
        IQueryable<Setups> GetSetups();
        Setups GetSetupByName(string Namesetup);
        void SaveSetup(Setups entity);
        void DeleteSetup(string Namesetup);

    }
}
