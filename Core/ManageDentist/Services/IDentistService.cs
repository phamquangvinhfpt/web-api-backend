using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ManageDentist.Models;
using Core.Repository;
namespace Core.ManageDentist.Services
{
    public interface IDentistService
    {
        Task<IEnumerable<DentistDetail>> GetAllDentists();
        Task<DentistDetail> GetDentistById(Guid id);
        Task CreateDentist(DentistDetail dentist);
        Task UpdateDentist(DentistDetail dentist);
        Task DeleteDentist(Guid id);
        Task<bool> DentistExists(Guid id);
    }
}
