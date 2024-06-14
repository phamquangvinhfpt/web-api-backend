using BusinessObject.Data;
using BusinessObject.Models;
using DAO.RecordDAO;
using DAO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.AppointmentsDAO
{
    public class AppointmentDAO
    {
        private static AppointmentDAO instance = null;
        private AppDbContext _context = null;

        public AppointmentDAO()
        {
            _context = new AppDbContext();
        }
        public static AppointmentDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppointmentDAO();
                }
                return instance;
            }
        }

        public Appointment CreateAppointment(AppointmentRequest request)
        {
            
            var appointment = new Appointment
            {
                PatientID = request.PatientID,
                DentistID = request.DentistID,
                ClinicID = request.ClinicID,
                TimeSlot = request.TimeSlot,
                Date = request.Date,
                Type = request.Type,
                Status = BusinessObject.Enums.AppointmentStatus.Scheduled,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            try
            {
                var app = _context.Appointments.Add(appointment);
                _context.SaveChanges();
                return app.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public Appointment GetAppointmentByID(string id)
        {
            return _context.Appointments.FirstOrDefault(p => p.Id.ToString() == id); 
        }
    }
}
