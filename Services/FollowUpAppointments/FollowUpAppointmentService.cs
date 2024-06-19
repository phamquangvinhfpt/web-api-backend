using AutoMapper.Internal;
using BusinessObject.Models;
using DAO.Requests;
using Repository.Appointments;
using Repository.FollowUpAppointments;
using Repository.RecordRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FollowUpAppointments
{
    public class FollowUpAppointmentService : IFollowUpAppointmentService
    {
        private IFollowUpAppointmentRepository repository;
        public FollowUpAppointmentService()
        {
            repository = new FollowUpAppointmentRepository();
        }
        public void CreateFollowAppointments(FollowUpAppointmentRequest request, Guid dentalID)
        {
            repository.CreateFollowAppointments(request, dentalID);
        }

        public List<FollowUpAppointment> GetAllIsFalse()
        {
            throw new NotImplementedException();
        }

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            throw new NotImplementedException();
        }

        public Task RemindFollowUpAppointment()
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(Guid id, bool status)
        {
            throw new NotImplementedException();
        }
    }
}
