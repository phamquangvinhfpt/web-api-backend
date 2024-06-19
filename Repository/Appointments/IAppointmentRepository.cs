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
        public Appointment GetAppointmentByID(Guid id);
        public Appointment CreateAppointment(AppointmentRequest request);
        public void ChangeStatusAppointment(Guid appointmentID, AppointmentStatus status);
        public Appointment GetAppoitmentAndDental(Guid id);
    }
}