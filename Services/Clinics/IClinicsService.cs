using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Services.Clinics
{
    public interface IClinicsService
    {

        public List<Clinic> GetAllClinics();

        public Clinic GetClinicsById(Guid Id);
        public void AddClinics(Clinic clinic, Guid userId);

        public void UpdateClinics(Clinic clinic, Guid id);

        public void DeleteClinics(Guid Id);
    }
}