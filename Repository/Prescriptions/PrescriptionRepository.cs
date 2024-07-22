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
        private readonly PrescriptionDAO _prescriptionDAO;
        public PrescriptionRepository(PrescriptionDAO prescriptionDAO)
        {
            _prescriptionDAO = prescriptionDAO;
        }
        public void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID, Guid userID) => _prescriptionDAO.CreatePrescription(request, dentalReID, userID);

        public List<Prescription> GetPrescriptionsByDentalID(Guid dentalID) => _prescriptionDAO.GetPrescriptionsByDentalID(dentalID);
    }
}