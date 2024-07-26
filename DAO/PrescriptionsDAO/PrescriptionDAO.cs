using BusinessObject.Data;
using BusinessObject.Models;
using DAO.Requests;

namespace DAO.PrescriptionsDAO
{
    public class PrescriptionDAO
    {
        private AppDbContext? _context = null;

        public PrescriptionDAO(AppDbContext context)
        {
            _context = context;
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
            try
            {
                return _context.Prescriptions.Where(p => p.DentalRecordId == dentalID).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
