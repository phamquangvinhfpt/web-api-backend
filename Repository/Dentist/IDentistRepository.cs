using BusinessObject.Models;

namespace Repository
{

    public interface IDentistRepository
    {
        Task<IEnumerable<DentistDetail>> GetAllDentists();
        Task<IEnumerable<DentistDetail>> GetAllDentistsByClinicId(Guid id);
        Task<DentistDetail> GetDentistById(Guid id);
        Task CreateDentist(DentistDetail dentist, Guid userId);
        Task UpdateDentist(DentistDetail dentist, Guid userId);
        Task DeleteDentist(Guid id, Guid userId);
        Task<bool> DentistExists(Guid id);

    }
}