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
        private static DentalRecordDAO instance = null;
        private AppDbContext _context = null;

        public DentalRecordDAO () {
            _context = new AppDbContext();
        }
        public static DentalRecordDAO Instance{
            get{
                if(instance == null){
                    instance = new DentalRecordDAO();
                }
                return instance;
            }
        }

        public List<DentalRecord> getAllRecord(){
            return _context.DentalRecords
                .Include(p => p.Appointment.Dentist)
                .Include(p => p.Appointment.Patient)
             .Include("Appointment")
            .ToList();
        }
        public List<DentalRecord> GetRecordByClinicOwner(Guid ownerID)
        {
            List<DentalRecord> list = new List<DentalRecord>();
            var clinics = _context.Clinics.Where(p => p.OwnerID == ownerID).ToList();
            if(clinics.Count > 0)
            {
                foreach(var clinic in clinics)
                {
                    var appointments = _context.Appointments.Where(p => p.ClinicID == clinic.Id).ToList();
                    if(appointments.Count > 0)
                    {
                        foreach(var appointment in appointments)
                        {
                            var existingRecord = _context.DentalRecords
                                .Include(p => p.Appointment.Dentist)
                .Include(p => p.Appointment.Patient)
                                .Where(p => p.AppointmentID == appointment.Id).FirstOrDefault();
                            if(existingRecord != null)
                            {
                                list.Add(existingRecord);
                            }
                        }
                    }
                }
            }
            return list;
        }
        public List<DentalRecord> GetRecordByDentist(Guid dentisID)
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
        public List<DentalRecord> GetRecordByCustomer(Guid customerID)
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
        public DentalRecord GetRecordByID(Guid id){
            var existingRecord = _context.DentalRecords
                .Include("Appointment")
             .Include("Prescriptions")
             .Include("MedicalRecord")
             .Include("FollowUpAppointments")
                .Where(p => p.Id == id).FirstOrDefault();
            if(existingRecord == null){
                throw new FileNotFoundException("Record is not found");
            }
            return existingRecord;
        }
        public DentalRecord GetByAppointment(Guid appointmentId)
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
        public DentalRecord CreateDentalRecord(Guid appointmentID, Guid userID)
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
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}