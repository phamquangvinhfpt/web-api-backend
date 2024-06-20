using BusinessObject.Data;
using BusinessObject.Models;
using DAO.RecordDAO;
using DAO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PrescriptionsDAO
{
    public class PrescriptionDAO
    {
        private static PrescriptionDAO instance = null;
        private AppDbContext _context = null;

        public PrescriptionDAO()
        {
            _context = new AppDbContext();
        }
        public static PrescriptionDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PrescriptionDAO();
                }
                return instance;
            }
        }

        public void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID, Guid userID)
        {
            List<Prescription> listPre = new List<Prescription>();
            foreach (var item in request)
            {
                listPre.Add(new Prescription
                {
                    DentalRecordId = dentalReID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    MedicineName = item.MedicineName,
                    Dosage = item.Dosage,
                    Instructions = item.Instructions
                });
            }
            try
            {
                _context.Prescriptions.AddRange(listPre);
                _context.SaveChangesAsync(userID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public List<Prescription> GetPrescriptionsByDentalID(Guid dentalID)
        {
            return _context.Prescriptions.Where(p => p.DentalRecordId == dentalID).ToList();
        }
    }
}
