using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repository.ClinicsDetails;

namespace Core.Auth.Services.ClinicsDetails
{
    public class ClinicsDetailsService : IClinicsDetailsService
    {
        private IClinicsDetailsRepository _clinicsDetailsRepository;
        public ClinicsDetailsService()
        {
            this._clinicsDetailsRepository = new ClinicsDetailsRepository();
        }
        public void AddClinicsDetails(ClinicDetail ClinicsDetails)
        {
            _clinicsDetailsRepository.AddClinicsDetails(ClinicsDetails);
        }

        public void DeleteClinicsDetails(Guid Id)
        {
            _clinicsDetailsRepository.DeleteClinicsDetails(Id);
        }

        public List<ClinicDetail> GetAllClinicDetails()
        {
            return _clinicsDetailsRepository.GetAllClinicDetails();
        }

        public ClinicDetail GetClinicDetailById(Guid Id)
        {
            return _clinicsDetailsRepository.GetClinicDetailById(Id);
        }

        public void UpdateClinicsDetails(ClinicDetail ClinicDetail)
        {
            _clinicsDetailsRepository.UpdateClinicsDetails(ClinicDetail);
        }
    }
}