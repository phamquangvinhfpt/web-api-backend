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
        private AppDbContext _context = null;

        public PrescriptionDAO(AppDbContext context)
        {
            _context = context;
        }

        public async void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID, Guid userID)
        {
            var transaction = _context.Database.BeginTransaction();
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
                await _context.SaveChangesAsync(userID);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public List<Prescription> GetPrescriptionsByDentalID(Guid dentalID)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                return _context.Prescriptions.Where(p => p.DentalRecordId == dentalID).ToList();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
