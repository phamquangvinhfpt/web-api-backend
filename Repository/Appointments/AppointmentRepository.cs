using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.AppointmentsDAO;
using DAO.Requests;

namespace Repository.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public Appointment CreateAppointment(AppointmentRequest request) => AppointmentDAO.Instance.CreateAppointment(request);

        public Appointment GetAppointmentByID(string id) => AppointmentDAO.Instance.GetAppointmentByID(id);
    }
}