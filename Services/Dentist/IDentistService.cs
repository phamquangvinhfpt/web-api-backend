using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
namespace Services.Dentist
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