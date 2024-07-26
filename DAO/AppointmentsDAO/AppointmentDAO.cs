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
        private bool IsValidAppointmentDate(DateTime date)
        {
            return date.Date >= DateTime.Now.Date;
        }

        private bool IsTimeSlotAvailable(Guid dentistId, Guid clinicId, DateTime date, TimeSpan timeSlot)
        {
            return !_context.Appointments.Any(a =>
                a.DentistID == dentistId &&
                a.ClinicID == clinicId &&
                a.Date.Date == date.Date &&
                a.TimeSlot == timeSlot &&
                a.Status != AppointmentStatus.Cancelled);
        }
        private bool IsTimeSlotAvailableForUpdate(Guid appointmentId, Guid dentistId, Guid clinicId, DateTime date, TimeSpan timeSlot)
        {
            return !_context.Appointments.Any(a =>
                a.Id != appointmentId &&
                a.DentistID == dentistId &&
                a.ClinicID == clinicId &&
                a.Date.Date == date.Date &&
                a.TimeSlot == timeSlot &&
                a.Status != AppointmentStatus.Cancelled);
        }

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
            if (!IsValidAppointmentDate(request.Date))
            {
                throw new Exception("Invalid appointment date. Please choose a future date.");
            }

            if (!IsTimeSlotAvailable(request.DentistID, request.ClinicID, request.Date, request.TimeSlot))
            {
                throw new Exception("The selected time slot is not available. Please choose another time.");
            }
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
        

        public void ChangeStatusAppointment(Guid appointmentID, AppointmentStatus status, Guid userID)
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
                var existingAppointment = _context.Appointments.FirstOrDefault(a => a.Id == appointment.Id);

                if (existingAppointment == null)
                {
                    throw new Exception("Appointment not found.");
                }

                // Kiểm tra tính hợp lệ của ngày
                if (!IsValidAppointmentDate(appointment.Date))
                {
                    throw new Exception("Invalid appointment date. Please choose a future date.");
                }

                // Kiểm tra tính khả dụng của thời gian slot khi cập nhật
                if (!IsTimeSlotAvailableForUpdate(appointment.Id, appointment.DentistID, appointment.ClinicID, appointment.Date, appointment.TimeSlot))
                {
                    throw new Exception("The selected time slot is not available. Please choose another time.");
                }

                // Cập nhật các thuộc tính của cuộc hẹn
                existingAppointment.PatientID = appointment.PatientID;
                existingAppointment.DentistID = appointment.DentistID;
                existingAppointment.ClinicID = appointment.ClinicID;
                existingAppointment.Date = appointment.Date;
                existingAppointment.TimeSlot = appointment.TimeSlot;
                existingAppointment.UpdatedAt = DateTime.Now;

                _context.Appointments.Update(existingAppointment);
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
            if (!IsValidAppointmentDate(request.Date))
            {
                throw new Exception("Invalid appointment date. Please choose a future date.");
            }

            if (!IsTimeSlotAvailable(request.DentistID, request.ClinicID, request.Date, request.TimeSlot))
            {
                throw new Exception("The selected time slot is not available. Please choose another time.");
            }
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

        public List<Appointment> GetAppointmentsForUser(Guid userId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Clinic)
                .Where(a => a.PatientID == userId || a.DentistID == userId)
                .ToList();
        }

        //search appointment by name
        public List<Appointment> SearchAppointmentByName(string name)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Dentist)
                .Include(a => a.Clinic)
                .Where(a => a.Patient.FullName.Contains(name) || a.Dentist.FullName.Contains(name))
                .ToList();
        }

    }
}

