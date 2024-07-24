using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Enums;
using BusinessObject.Models;
using DAO.AppointmentsDAO;
using DAO.Requests;

namespace Repository.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public void ChangeStatusAppointment(Guid appointmentID, AppointmentStatus status)=> AppointmentDAO.Instance.ChangeStatusAppointment(appointmentID, status);

        public Appointment CreateAppointment(AppointmentRequest request) => AppointmentDAO.Instance.CreateAppointment(request);

        public Appointment GetAppointmentByID(Guid id) => AppointmentDAO.Instance.GetAppointmentByID(id);

        public Appointment GetAppoitmentAndDental(Guid id) => AppointmentDAO.Instance.GetAppoitmentAndDental(id);
        public void DeleteAppointment(Guid id) => AppointmentDAO.Instance.DeleteAppointment(id);
        public List<Appointment> GetAllAppointments() => AppointmentDAO.Instance.GetAllAppointments();
        public void UpdateAppointment(Appointment appointment) => AppointmentDAO.Instance.UpdateAppointment(appointment);
        public List<Appointment> GetAllByStatusAndType(AppointmentStatus status, AppointmentType type) => AppointmentDAO.Instance.GetAllByStatusAndType(status, type);
        public void UpdateAppointmentDate(Guid Id, DateTime date) => AppointmentDAO.Instance.UpdateAppointmentDate(Id, date);
        public Appointment CreateAppointmentForPeriodic(AppointmentRequest request) => AppointmentDAO.Instance.CreateAppointmentForPeriodic(request);

        public Appointment GetAppointmentForCreateDentalByID(Guid id) => AppointmentDAO.Instance.GetAppointmentForCreateDentalByID(id);
        public List<Appointment> GetByDentistID(Guid dentistID) => AppointmentDAO.Instance.GetByDentistID(dentistID);
        public List<Appointment> GetAppointmentsForUser(Guid userId) => AppointmentDAO.Instance.GetAppointmentsForUser(userId);
    }
}