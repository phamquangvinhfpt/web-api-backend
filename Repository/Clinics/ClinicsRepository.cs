using System;
using System.Collections.Generic;
using BusinessObject.Models;
using DAO.Clinics;

namespace Repository.Clinics
{
    public class ClinicsRepository : IClinicsRepository
    {
        public void AddClinics(Clinic clinic) => ClinicsDAO.Instance.AddClinics(clinic);

        public void DeleteClinics(Guid id) => ClinicsDAO.Instance.DeleteClinics(id);

        public List<Clinic> GetAllClinics() => ClinicsDAO.Instance.GetAllClinics();

        public Clinic GetClinicsById(Guid id) => ClinicsDAO.Instance.GetClinicsById(id);

        public void UpdateClinics(Clinic clinic) => ClinicsDAO.Instance.UpdateClinics(clinic);
    }
}
