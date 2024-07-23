using BusinessObject.Models;
using DAO.Data;
using DAO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
