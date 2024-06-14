using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;

namespace Repository.Prescriptions
{
    public interface IPrescriptionRepository
    {
        public void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID);
        public List<Prescription> GetPrescriptionsByDentalID(Guid dentalID);
    }
}