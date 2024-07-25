using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Enums;
using BusinessObject.Models;
using DAO.Requests;

namespace Repository.Appointments
{
    public interface IAppointmentRepository
    {
        public Appointment GetAppointmentForCreateDentalByID(Guid id);
        public Appointment GetAppointmentByID(Guid id);
        public Appointment CreateAppointment(AppointmentRequest request);
        public void ChangeStatusAppointment(Guid appointmentID, AppointmentStatus status, Guid userID);
        public Appointment GetAppoitmentAndDental(Guid id);
        public void DeleteAppointment(Guid id);
        public List<Appointment> GetAllAppointments();
        public void UpdateAppointment(Appointment appointment);
        public List<Appointment> GetAllByStatusAndType(AppointmentStatus status, AppointmentType type);
        public void UpdateAppointmentDate(Guid Id, DateTime date);
        public Appointment CreateAppointmentForPeriodic(AppointmentRequest request);
        public List<Appointment> GetByDentistID(Guid dentistID);
        public List<Appointment> GetAppointmentsForUser(Guid userId);

    }
}