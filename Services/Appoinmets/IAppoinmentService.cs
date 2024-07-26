using BusinessObject.Enums;
using BusinessObject.Models;
using DAO.Data;
using DAO.Requests;

namespace Services.Appoinmets
{
    public interface IAppoinmentService
    {
        public Appointment GetAppointmentByID(Guid id);
        public Appointment CreateAppointment(AppointmentRequest request);
        public void DeleteAppointment(Guid id);
        public List<Appointment> GetAllAppointments();
        public void UpdateAppointment(Appointment appointment);

        Task PeriodicAppointment();
        public void UpdateAppointmentDate(Guid Id, DateTime date);
        public Appointment CreateAppointmentForPeriodic(AppointmentRequest request);
        public AppointmentData GetAppointmentForCreateDentalByID(Guid id);
        public List<Appointment> GetByDentistID(Guid dentistID);
        public List<Appointment> GetAppointmentsForUser(Guid userId);
        public void ChangeStatusAppointment(Guid appointmentID, AppointmentStatus status, Guid userID);
        public List<Appointment> SearchAppointmentByName(string name);
    }
}
