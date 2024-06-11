using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Data;
using BusinessObject.Models;
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
            // .Include("Appointment")
            // .Include("Prescriptions")
            // .Include("MedicalRecord")
            // .Include("FollowUpAppointments")
            .ToList();
        }
        public DentalRecord GetRecordByID(string id){
            var existingRecord = _context.DentalRecords.Where(p => p.Id.ToString() == id).FirstOrDefault();
            if(existingRecord == null){
                throw new FileNotFoundException("Record is not found");
            }
            return existingRecord;
        }
        public void CreateDentalRecord(CreateDentalRecordRequest request)
        {
            var appointment = new Appointment
            {
                PatientID = request.appointment.PatientID,
                DentistID = request.appointment.DentistID,
                ClinicID = request.appointment.ClinicID,
                TimeSlot = request.appointment.TimeSlot,
                Date = request.appointment.Date,
                Type = request.appointment.Type,
                Status = BusinessObject.Enums.AppointmentStatus.Scheduled,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            List<Prescription> listPre = new List<Prescription>();
            foreach(var item in request.prescriptionRequests){
                listPre.Add(new Prescription
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    MedicineName = item.MedicineName,
                    Dosage = item.Dosage,
                    Instructions = item.Instructions
                });
            }
            var mdcRecord = new MedicalRecord
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Symptoms = request.MedicalRecordRequest.Symptoms,
                Diagnosis = request.MedicalRecordRequest.Diagnosis,
                Treatment = request.MedicalRecordRequest.Treatment,
            };
            List<FollowUpAppointment> listFLUA = new List<FollowUpAppointment>();
            foreach (var item in request.followUpAppointmentRequests)
            {
                listFLUA.Add(new FollowUpAppointment
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ScheduledDate = item.ScheduledDate,
                    Reason = item.Reason,
                });
            }
            DentalRecord dentalRecord = new DentalRecord
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Appointment = appointment,
                Prescriptions = listPre,
                MedicalRecord = mdcRecord,
                FollowUpAppointments = listFLUA
            };
            try
            {
                _context.DentalRecords.Add(dentalRecord);
                _context.SaveChanges();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}