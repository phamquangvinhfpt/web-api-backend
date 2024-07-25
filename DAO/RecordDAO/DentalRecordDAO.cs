using BusinessObject.Data;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DAO.RecordDAO
{
    public class DentalRecordDAO
    {
        private readonly AppDbContext _context;

        public DentalRecordDAO(AppDbContext context)
        {
            _context = context;
        }

        public List<DentalRecord> getAllRecord()
        {
            try
            {
                return _context.DentalRecords
                .Include(p => p.Appointment.Dentist)
                .Include(p => p.Appointment.Patient)
                .Include(p => p.Prescriptions)
                .Include(p => p.MedicalRecord)
                .Include(p => p.FollowUpAppointments)
                .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<DentalRecord> GetRecordByClinicOwner(Guid ownerID)
        {
            try
            {
                List<DentalRecord> list = new List<DentalRecord>();
                var clinics = _context.Clinics.Where(p => p.OwnerID == ownerID).ToList();
                if (clinics.Count > 0)
                {
                    foreach (var clinic in clinics)
                    {
                        var appointments = _context.Appointments.Where(p => p.ClinicID == clinic.Id).ToList();
                        if (appointments.Count > 0)
                        {
                            foreach (var appointment in appointments)
                            {
                                var existingRecord = _context.DentalRecords
                                    .Include(p => p.Appointment.Dentist)
                    .Include(p => p.Appointment.Patient)
                                    .Where(p => p.AppointmentID == appointment.Id).FirstOrDefault();
                                if (existingRecord != null)
                                {
                                    list.Add(existingRecord);
                                }
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<DentalRecord> GetRecordByDentist(Guid dentisID)
        {
            try
            {
                List<DentalRecord> list = new List<DentalRecord>();
                var appointments = _context.Appointments.Where(p => p.DentistID == dentisID).ToList();
                if (appointments.Count > 0)
                {
                    foreach (var appointment in appointments)
                    {
                        var existingRecord = _context.DentalRecords
                            .Include(p => p.Appointment.Dentist)
                    .Include(p => p.Appointment.Patient)
                                    .Where(p => p.AppointmentID == appointment.Id).FirstOrDefault();
                        if (existingRecord != null)
                        {
                            list.Add(existingRecord);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<DentalRecord> GetRecordByCustomer(Guid customerID)
        {
            try
            {
                List<DentalRecord> list = new List<DentalRecord>();
                var appointments = _context.Appointments.Where(p => p.PatientID == customerID).ToList();
                if (appointments.Count > 0)
                {
                    foreach (var appointment in appointments)
                    {
                        var existingRecord = _context.DentalRecords.Include(p => p.Appointment.Dentist).Include(p => p.Appointment.Patient)
                                    .Where(p => p.AppointmentID == appointment.Id).FirstOrDefault();
                        if (existingRecord != null)
                        {
                            list.Add(existingRecord);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DentalRecord GetRecordByID(Guid id)
        {
            try
            {
                var existingRecord = _context.DentalRecords
                .Include("Appointment")
                .Include("Prescriptions")
                .Include("MedicalRecord")
                .Include("FollowUpAppointments")
                .Where(p => p.Id == id).FirstOrDefault();
                if (existingRecord == null)
                {
                    throw new FileNotFoundException("Record is not found");
                }
                return existingRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DentalRecord GetByAppointment(Guid appointmentId)
        {
            try
            {
                var existingRecord = _context.DentalRecords
                .Include("FollowUpAppointments")
                .Where(p => p.AppointmentID == appointmentId).FirstOrDefault();
                if (existingRecord == null)
                {
                    throw new FileNotFoundException("Record is not found");
                }
                return existingRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DentalRecord CreateDentalRecord(Guid appointmentID, Guid userID)
        {
            try
            {
                DentalRecord dentalRecord = new DentalRecord
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    AppointmentID = appointmentID
                };
                try
                {
                    var dtr = _context.DentalRecords.Add(dentalRecord);
                    _context.SaveChangesAsync(userID);
                    return dtr.Entity;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}