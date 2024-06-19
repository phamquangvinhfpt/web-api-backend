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
    }
}