using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Core.Auth.Services.ClinicsDetails
{
    public interface IClinicsDetailsService
    {
        public List<ClinicDetail> GetAllClinicDetails();
        
        public ClinicDetail GetClinicDetailById(Guid Id);
        
        public void AddClinicsDetails(ClinicDetail ClinicsDetails);
        
        public void UpdateClinicsDetails(ClinicDetail ClinicDetail);
        
        public void DeleteClinicsDetails(Guid Id);
    }
    }
