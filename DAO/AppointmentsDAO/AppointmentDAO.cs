using BusinessObject.Data;
using BusinessObject.Enums;
using BusinessObject.Models;
using DAO.RecordDAO;
using DAO.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
        // kham 1 lan
        public Appointment CreateAppointment(AppointmentRequest request)
        {

            var appointment = new Appointment
            {
                PatientID = request.PatientID,
                DentistID = request.DentistID,
                ClinicID = request.ClinicID,
                TimeSlot = request.TimeSlot,
                Date = request.Date,
                duration = 0,
                Type = request.Type,
                Status = BusinessObject.Enums.AppointmentStatus.Pending,
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
        

        public void ChangeStatusAppointment(Guid appointmentID, AppointmentStatus status)
        {
            try
            {
                var appoint = _context.Appointments.FirstOrDefault(p => p.Id == appointmentID);
                if(appoint == null)
                {
                    throw new Exception("appointment is not found");
                }
                appoint.Status = status;
                _context.SaveChanges();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Appointment GetAppointmentByID(Guid id)
        {
            return _context.Appointments.Include("Patient").Include("Dentist").FirstOrDefault(p => p.Id == id);
        }
        public Appointment GetAppointmentForCreateDentalByID(Guid id)
        {
            return _context.Appointments.Include("Patient").Include("Dentist").Include("Clinic").FirstOrDefault(p => p.Id == id);
        }

        public Appointment GetAppoitmentAndDental(Guid id)
        {
            return _context.Appointments
                .Include("DentalRecord")
                .FirstOrDefault(p => p.Id == id);
        }

        // update appointment
        public void UpdateAppointment(Appointment appointment)
        {
            try
            {
                _context.Appointments.Update(appointment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // get all appointments
        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments.Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Clinic)
                .ToList();
            //.Include("Patient").Include("Dentist").Include("Clinic")
        }
        // delete appointment
        public void DeleteAppointment(Guid id)
        {
            var appointment = _context.Appointments.FirstOrDefault(p => p.Id == id);
            try
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // create appointment dinh ki cho benh nhan
        public Appointment CreateAppointmentForPeriodic(AppointmentRequest request)
        {
            var appointment = new Appointment
            {
                PatientID = request.PatientID,
                DentistID = request.DentistID,
                ClinicID = request.ClinicID,
                TimeSlot = request.TimeSlot,
                Date = request.Date,
                Type = BusinessObject.Enums.AppointmentType.Periodic,
                Status = BusinessObject.Enums.AppointmentStatus.Scheduled,
                duration = request.duration,
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
        public List<Appointment> GetAllByStatusAndType(AppointmentStatus status, AppointmentType type)
        {
            var list = _context.Appointments
                .Include(p => p.Patient).Include(p => p.Dentist)
                       .Where(p => p.Status == status && p.Type == type)
                       .ToList();
            return list;
        }
        public void UpdateAppointmentDate( Guid Id, DateTime date)
        {
            try
            {
                var flu = _context.Appointments.FirstOrDefault(p => p.Id == Id);
                flu.Date = date;
                _context.Appointments.Update(flu);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // getbyDentistID
        public List<Appointment> GetByDentistID(Guid dentistID)
        {
            return _context.Appointments.Include("Patient").Include("Dentist").Include("Clinic").Where(p => p.DentistID == dentistID).ToList();
        }

    }
}

