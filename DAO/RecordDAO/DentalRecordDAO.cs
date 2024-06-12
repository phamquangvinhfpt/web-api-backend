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
        public DentalRecord CreateDentalRecord(Guid appointmentID)
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
                _context.SaveChanges();
                return dtr.Entity;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}