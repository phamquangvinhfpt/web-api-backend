using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.PrescriptionsDAO;
using DAO.Requests;

namespace Repository.Prescriptions
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        public void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID) => PrescriptionDAO.Instance.CreatePrescription(request, dentalReID);

        public List<Prescription> GetPrescriptionsByDentalID(Guid dentalID) => PrescriptionDAO.Instance.GetPrescriptionsByDentalID(dentalID);
    }
}