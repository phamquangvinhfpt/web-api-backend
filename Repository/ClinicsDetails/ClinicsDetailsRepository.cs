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
        public void AddClinicsDetails(ClinicDetail ClinicsDetails) => ClinicsDetailsDAO.Instance.AddClinicsDetails(ClinicsDetails);
        

        public void DeleteClinicsDetails(Guid Id) => ClinicsDetailsDAO.Instance.DeleteClinicsDetails(Id);
        

        public List<ClinicDetail> GetAllClinicDetails() => ClinicsDetailsDAO.Instance.GetAllClinicDetails();
        
        public ClinicDetail GetClinicDetailById(Guid Id) => ClinicsDetailsDAO.Instance.GetClinicDetailById(Id);
        
        public void UpdateClinicsDetails(ClinicDetail ClinicDetail) => ClinicsDetailsDAO.Instance.UpdateClinicsDetails(ClinicDetail);
        
    }
}