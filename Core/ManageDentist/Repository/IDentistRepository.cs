using BusinessObject.Models;
namespace Core.ManageDentist.Repository
{
    public interface IDentistRepository
    {
        Task<IEnumerable<DentistDetail>> GetAllDentists();
        Task<DentistDetail> GetDentistById(Guid id);
        Task CreateDentist(DentistDetail dentist);
        Task UpdateDentist(DentistDetail dentist);
        Task DeleteDentist(Guid id);
        Task<bool> DentistExists(Guid id);
    }
}
