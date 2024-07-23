using BusinessObject.Models;

namespace Repository
{

    public interface IDentistRepository
    {
        Task<IEnumerable<DentistDetail>> GetAllDentists();
        Task<IEnumerable<DentistDetail>> GetAllDentistsByClinicId(Guid id);
        Task<DentistDetail> GetDentistById(Guid id);
        Task CreateDentist(DentistDetail dentist);
        Task UpdateDentist(DentistDetail dentist);
        Task DeleteDentist(Guid id);
        Task<bool> DentistExists(Guid id);

    }
}