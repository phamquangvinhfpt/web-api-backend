using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repository.Clinics;

namespace Services.Clinics
{
    public class ClinicsService : IClinicsService
    {
        private IClinicsRepository _clinicsRepository;
        public ClinicsService()
        {
            this._clinicsRepository = new ClinicsRepository();
        }
        public void AddClinics(Clinic clinic, Guid userId)
        {
            _clinicsRepository.AddClinics(clinic, userId);
        }

        public void DeleteClinics(Guid Id)
        {
            _clinicsRepository.DeleteClinics(Id);
        }

        public List<Clinic> GetAllClinics()
        {
            return _clinicsRepository.GetAllClinics();
        }

        public Clinic GetClinicsById(Guid Id)
        {
            return _clinicsRepository.GetClinicsById(Id);
        }

        public void UpdateClinics(Clinic clinic)
        {
            _clinicsRepository.UpdateClinics(clinic);
        }
    }
}