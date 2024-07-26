// Repository/DentistRepo.cs
using BusinessObject.Models;
using DAO.ManageDentist;

namespace Repository
{
    public class DentistRepo : IDentistRepository
    {
        private readonly DentistDAO _dentistDAO;
        public DentistRepo(DentistDAO dentistDAO)
        {
            _dentistDAO = dentistDAO;
        }
        public Task CreateDentist(DentistDetail dentist, Guid userId)
        {
            return _dentistDAO.CreateDentist(dentist, userId);
        }

        public Task DeleteDentist(Guid id, Guid userId)
        {
            return _dentistDAO.DeleteDentist(id, userId);
        }

        public Task<bool> DentistExists(Guid id)
        {
            return _dentistDAO.DentistExists(id);
        }

        public Task<IEnumerable<DentistDetail>> GetAllDentists()
        {
            return _dentistDAO.GetAllDentists();
        }

        public Task<IEnumerable<DentistDetail>> GetAllDentistsByClinicId(Guid id)
        {
            return _dentistDAO.GetAllDentistsByClinicId(id);
        }

        public Task<DentistDetail> GetDentistById(Guid id)
        {
            return _dentistDAO.GetDentistById(id);
        }

        public Task UpdateDentist(DentistDetail dentist, Guid userId)
        {
            return _dentistDAO.UpdateDentist(dentist, userId);
        }
    }
}
