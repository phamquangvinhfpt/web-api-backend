using Azure.Core;
using BusinessObject.Models;
using DAO.Requests;
using Repository.Appointments;
using Repository.Clinics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Appoinmets
{
    public class AppoinmentService : IAppoinmentService
    {
        private IAppointmentRepository _AppoinsRepository;
        public AppoinmentService()
        {
            this._AppoinsRepository = new AppointmentRepository();
        }
        public Appointment GetAppointmentByID(Guid id)
        {
            return _AppoinsRepository.GetAppointmentByID(id);
        }
        // create appointment
        public Appointment CreateAppointment(AppointmentRequest request)
        {
            return _AppoinsRepository.CreateAppointment(request);
        }
        public void DeleteAppointment(Guid id)
        {

           _AppoinsRepository.DeleteAppointment(id);
        }
        public List<Appointment> GetAllAppointments()
        {
               return _AppoinsRepository.GetAllAppointments();
        }
        public void UpdateAppointment(Appointment appointment)
        {
            _AppoinsRepository.UpdateAppointment(appointment);
        }
        public async Task PeriodicAppointment()
        {
            throw new NotImplementedException();
        }
         public void UpdateAppointmentDate(Guid Id, DateTime date)
        {
            _AppoinsRepository.UpdateAppointmentDate(Id, date);
        }
        public Appointment CreateAppointmentForPeriodic(AppointmentRequest request)
        {
            return _AppoinsRepository.CreateAppointmentForPeriodic(request);
        }
    }
}
