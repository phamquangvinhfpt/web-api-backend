using DAO.Requests;

namespace Services.Dentist
{
    public interface IDentistService
    {
        Task<IEnumerable<DentistDetailDTO>> GetAllDentists();
        Task<IEnumerable<DentistDetailDTO>> GetAllDentistsByClinicId(Guid id);
        Task<DentistDetailDTO> GetDentistById(Guid id);
        Task CreateDentist(DentistDetailDTO dentist, Guid userId);
        Task UpdateDentist(DentistDetailDTO dentist, Guid userId);
        Task DeleteDentist(Guid id, Guid userId);
        Task<bool> DentistExists(Guid id);
    }
}