// Services/Dentist/DentistService.cs
using AutoMapper;
using BusinessObject.Models;
using DAO.Requests;
using Repository;
using Repository.Clinics;

namespace Services.Dentist
{
    public class DentistService : IDentistService
    {
        private readonly IDentistRepository _dentistRepository;
        private readonly IMapper _mapper;
        private readonly IClinicsRepository _clinicRepository;

        public DentistService(IMapper mapper, IDentistRepository dentistRepository, IClinicsRepository clinicsRepository)
        {
            _dentistRepository = dentistRepository;
            _mapper = mapper;
            _clinicRepository = clinicsRepository;
        }

        public async Task<IEnumerable<DentistDetailDTO>> GetAllDentists()
        {
            try
            {
                var dentists = await _dentistRepository.GetAllDentists();
                var dentistDtos = _mapper.Map<IEnumerable<DentistDetailDTO>>(dentists);
                return dentistDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DentistDetailDTO>> GetAllDentistsByClinicId(Guid id)
        {
            try
            {
                var dentists = await _dentistRepository.GetAllDentistsByClinicId(id);
                var dentistDtos = _mapper.Map<IEnumerable<DentistDetailDTO>>(dentists);
                return dentistDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<DentistDetailDTO> GetDentistById(Guid id)
        {
            var dentist = await _dentistRepository.GetDentistById(id);
            return _mapper.Map<DentistDetailDTO>(dentist);
        }

        public async Task CreateDentist(DentistDetailDTO dentist)
        {
            var dentistEntity = _mapper.Map<DentistDetail>(dentist);
            await _dentistRepository.CreateDentist(dentistEntity);
        }

        public async Task UpdateDentist(DentistDetailDTO dentist)
        {
            var dentistEntity = _mapper.Map<DentistDetail>(dentist);
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
