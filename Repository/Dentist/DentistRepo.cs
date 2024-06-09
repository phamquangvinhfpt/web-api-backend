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
            throw new NotImplementedException();
        }

        public Task<bool> DentistExists(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DentistDetail>> GetAllDentists()
        {
            throw new NotImplementedException();
        }

        public Task<DentistDetail> GetDentistById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDentist(DentistDetail dentist)
        {
            throw new NotImplementedException();
        }
    }
}