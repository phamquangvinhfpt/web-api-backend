using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;

namespace Repository.Appointments
{
    public interface IAppointmentRepository
    {
        public Appointment GetAppointmentByID(string id);
        public Appointment CreateAppointment(AppointmentRequest request);
    }
}