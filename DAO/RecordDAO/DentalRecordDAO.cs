using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Data;
using BusinessObject.Models;
using DAO.Clinics;
using DAO.Requests;
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
            var transactions = _context.Database.BeginTransaction();
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
                transactions.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public List<DentalRecord> GetRecordByClinicOwner(Guid ownerID)
        {
            var transactions = _context.Database.BeginTransaction();
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
                transactions.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public List<DentalRecord> GetRecordByDentist(Guid dentisID)
        {
            var transactions = _context.Database.BeginTransaction();
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
                transactions.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public List<DentalRecord> GetRecordByCustomer(Guid customerID)
        {
            var transactions = _context.Database.BeginTransaction();
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
                transactions.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public DentalRecord GetRecordByID(Guid id)
        {
            var transactions = _context.Database.BeginTransaction();
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
                transactions.Commit();
                return existingRecord;
            }
            catch (Exception ex)
            {
                transactions.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public DentalRecord GetByAppointment(Guid appointmentId)
        {
            var transactions = _context.Database.BeginTransaction();
            try
            {
                var existingRecord = _context.DentalRecords
                .Include("FollowUpAppointments")
                .Where(p => p.AppointmentID == appointmentId).FirstOrDefault();
                if (existingRecord == null)
                {
                    throw new FileNotFoundException("Record is not found");
                }
                transactions.Commit();
                return existingRecord;
            }
            catch (Exception ex)
            {
                transactions.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public DentalRecord CreateDentalRecord(Guid appointmentID, Guid userID)
        {
            var transactions = _context.Database.BeginTransaction();
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
                    transactions.Commit();
                    return dtr.Entity;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                transactions.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}