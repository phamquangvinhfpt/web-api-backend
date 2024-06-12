using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.Models;
using DAO.Requests;
using Repository;

namespace Services.Dentist
{
    public class DentistService : IDentistService
    {
        private readonly IDentistRepository _dentistRepository;
        private readonly IMapper _mapper;

        public DentistService(IMapper mapper, IDentistRepository dentistRepository)
        {
            _dentistRepository = dentistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DentistDetailDTO>> GetAllDentists()
        {
            var dentists = await _dentistRepository.GetAllDentists();
            return _mapper.Map<IEnumerable<DentistDetailDTO>>(dentists);
        }

        public async Task<DentistDetailDTO> GetDentistById(Guid id)
        {
            var dentist = await _dentistRepository.GetDentistById(id);
            return _mapper.Map<DentistDetailDTO>(dentist);
        }

        public async Task CreateDentist(DentistDetailDTO dentist)
        {
            var dentistEntity = _mapper.Map<BusinessObject.Models.DentistDetail>(dentist);
            await _dentistRepository.CreateDentist(dentistEntity);
        }

        public async Task UpdateDentist(DentistDetailDTO dentist)
        {
            var dentistEntity = _mapper.Map<BusinessObject.Models.DentistDetail>(dentist);
            await _dentistRepository.UpdateDentist(dentistEntity);
        }

        public async Task DeleteDentist(Guid id)
        {
            await _dentistRepository.DeleteDentist(id);
        }

        public async Task<bool> DentistExists(Guid id)
        {
            return await _dentistRepository.DentistExists(id);
        }

        
    }
}
