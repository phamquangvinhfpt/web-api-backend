using DAO.Requests;

namespace Services.Dentist
{
    public interface IDentistService
    {
        Task<IEnumerable<DentistDetailDTO>> GetAllDentists();
        Task<IEnumerable<DentistDetailDTO>> GetAllDentistsByClinicId(Guid id);
        Task<DentistDetailDTO> GetDentistById(Guid id);
        Task CreateDentist(DentistDetailDTO dentist);
        Task UpdateDentist(DentistDetailDTO dentist);
        Task DeleteDentist(Guid id);
        Task<bool> DentistExists(Guid id);
    }
}