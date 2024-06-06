using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Clinics;

namespace Repository.Clinics
{
    public class ClinicsRepository : IClinicsRepository
    {
        public void AddClinics(Clinic clinic) => ClinicsDAO.Instance.AddClinics(clinic);
        

        public void DeleteClinics(Guid Id) => ClinicsDAO.Instance.DeleteClinics(Id);
        
        public List<Clinic> GetAllClinics() => ClinicsDAO.Instance.GetAllClinics();
        
        public Clinic GetClinicsById(Guid Id) => ClinicsDAO.Instance.GetClinicsById(Id);
        
        public void UpdateClinics(Clinic clinic) => ClinicsDAO.Instance.UpdateClinics(clinic);
        
    }
}