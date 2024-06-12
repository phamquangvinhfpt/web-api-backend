using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;

namespace Repository.FollowUpAppointments
{
    public interface IFollowUpAppointmentRepository
    {
        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID);
        public void CreateFollowAppointments(List<FollowUpAppointmentRequest> requests, Guid dentalID);
    }
}