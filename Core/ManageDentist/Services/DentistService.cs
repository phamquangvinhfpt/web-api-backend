using Core.ManageDentist.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ManageDentist.Models;
using Core.Repository;
using AutoMapper;
namespace Core.ManageDentist.Services
{
    public class DentistService : IDentistService
    {
        private readonly IDentistRepository _dentistRepository;
        private readonly IMapper _mapper;

        public DentistService(IDentistRepository dentistRepository, IMapper mapper)
        {
            _dentistRepository = dentistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DentistDetail>> GetAllDentists()
        {
            var dentists = await _dentistRepository.GetAllDentists();
            return _mapper.Map<IEnumerable<Core.ManageDentist.Models.DentistDetail>>(dentists);
        }

        public async Task<DentistDetail> GetDentistById(Guid id)
        {
            var dentist = await _dentistRepository.GetDentistById(id);
            return _mapper.Map<Core.ManageDentist.Models.DentistDetail>(dentist);
        }

        public async Task CreateDentist(DentistDetail dentist)
        {
            var dentistEntity = _mapper.Map<BusinessObject.Models.DentistDetail>(dentist);
            await _dentistRepository.CreateDentist(dentistEntity);
        }

        public async Task UpdateDentist(DentistDetail dentist)
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
