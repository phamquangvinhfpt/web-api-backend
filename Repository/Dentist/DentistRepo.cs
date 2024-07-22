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
        public Task CreateDentist(DentistDetail dentist)
        {
            return _dentistDAO.CreateDentist(dentist);
        }

        public Task DeleteDentist(Guid id)
        {
            return _dentistDAO.DeleteDentist(id);
        }

        public Task<bool> DentistExists(Guid id)
        {
            return _dentistDAO.DentistExists(id);
        }

        public Task<IEnumerable<DentistDetail>> GetAllDentists()
        {
            return _dentistDAO.GetAllDentists();
        }

        public Task<DentistDetail> GetDentistById(Guid id)
        {
            return _dentistDAO.GetDentistById(id);
        }

        public Task UpdateDentist(DentistDetail dentist)
        {
            return _dentistDAO.UpdateDentist(dentist);
        }
    }
}
