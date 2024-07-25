using BusinessObject.Data;
using BusinessObject.Models;
using DAO.Requests;

namespace DAO.MedicalDAO
{
    public class MedicalRecordDAO
    {
        private static MedicalRecordDAO? instance = null;
        private AppDbContext? _context = null;

        public MedicalRecordDAO()
        {
            _context = new AppDbContext();
        }
        public static MedicalRecordDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MedicalRecordDAO();
                }
                return instance;
            }
        }

        public void CreateMedicalRecord(MedicalRecordRequest request, Guid appoinmentid, Guid dentalID, Guid userID)
        {
            var mdcRecord = new MedicalRecord
            {
                AppointmentId = appoinmentid,
                DentalRecordId = dentalID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Symptoms = request.Symptoms,
                Diagnosis = request.Diagnosis,
                Treatment = request.Treatment,
            };
            try
            {
                _context.MedicalRecords.Add(mdcRecord);
                _context.SaveChangesAsync(userID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public MedicalRecord GetMedicalRecordByDentalID(Guid dentalID)
        {
            return _context.MedicalRecords.FirstOrDefault(p => p.DentalRecordId == dentalID);
        }
    }
}
