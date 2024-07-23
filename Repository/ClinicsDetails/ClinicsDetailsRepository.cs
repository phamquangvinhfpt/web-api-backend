using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.ClinicsDetails;

namespace Repository.ClinicsDetails
{
    public class ClinicsDetailsRepository : IClinicsDetailsRepository
    {
        private readonly ClinicsDetailsDAO _clinicsDetailsDAO;
        public ClinicsDetailsRepository(ClinicsDetailsDAO clinicsDetailsDAO)
        {
            _clinicsDetailsDAO = clinicsDetailsDAO;
        }
        public void AddClinicsDetails(ClinicDetail ClinicsDetails) => _clinicsDetailsDAO.AddClinicsDetails(ClinicsDetails);
        

        public void DeleteClinicsDetails(Guid Id) => _clinicsDetailsDAO.DeleteClinicsDetails(Id);
        

        public List<ClinicDetail> GetAllClinicDetails() => _clinicsDetailsDAO.GetAllClinicDetails();
        
        public ClinicDetail GetClinicDetailById(Guid Id) => _clinicsDetailsDAO.GetClinicDetailById(Id);
        
        public void UpdateClinicsDetails(ClinicDetail ClinicDetail) => _clinicsDetailsDAO.UpdateClinicsDetails(ClinicDetail);
        
    }
}