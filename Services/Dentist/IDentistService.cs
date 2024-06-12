using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO.Requests;

namespace Services.Dentist
{
    public interface IDentistService
    {
        Task<IEnumerable<DentistDetailDTO>> GetAllDentists();
        Task<DentistDetailDTO> GetDentistById(Guid id);
        Task CreateDentist(DentistDetailDTO dentist);
        Task UpdateDentist(DentistDetailDTO dentist);
        Task DeleteDentist(Guid id);
        Task<bool> DentistExists(Guid id);
    }
}