using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.ManageDentist;

namespace Repository
{
    public class DentistRepo : IDentistRepository
    {
        
        public Task CreateDentist(DentistDetail dentist)
        {
           return DentistDAO.Instance.CreateDentist(dentist);
        }

        public Task DeleteDentist(Guid id)
        {
            return DentistDAO.Instance.DeleteDentist(id);
        }

        public Task<bool> DentistExists(Guid id)
        {
            return DentistDAO.Instance.DentistExists(id);
        }

        public Task<IEnumerable<DentistDetail>> GetAllDentists()
        {
            return DentistDAO.Instance.GetAllDentists();
        }

        public Task<DentistDetail> GetDentistById(Guid id)
        {
            return DentistDAO.Instance.GetDentistById(id);
        }

        public Task UpdateDentist(DentistDetail dentist)
        {
            return DentistDAO.Instance.UpdateDentist(dentist);
        }
    }
}