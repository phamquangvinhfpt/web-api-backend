using System;
using System.Collections.Generic;
using BusinessObject.Models;
using DAO.Clinics;

namespace Repository.Clinics
{
    public class ClinicsRepository : IClinicsRepository
    {
        private readonly ClinicsDAO _clinicsDAO;
        public ClinicsRepository(ClinicsDAO clinicsDAO)
        {
            _clinicsDAO = clinicsDAO;
        }
        public void AddClinics(Clinic clinic, Guid userId) => _clinicsDAO.AddClinics(clinic, userId);

        public void DeleteClinics(Guid id) => _clinicsDAO.DeleteClinics(id);

        public List<Clinic> GetAllClinics() => _clinicsDAO.GetAllClinics();

        public Clinic GetClinicsById(Guid id) => _clinicsDAO.GetClinicsById(id);

        public void UpdateClinics(Clinic clinic, Guid id) => _clinicsDAO.UpdateClinics(clinic, id);
    }
}
