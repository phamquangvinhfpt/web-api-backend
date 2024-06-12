using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.FollowUpAppoinmentsDAO;
using DAO.Requests;

namespace Repository.FollowUpAppointments
{
    public class FollowUpAppointmentRepository : IFollowUpAppointmentRepository
    {
        public void CreateFollowAppointments(List<FollowUpAppointmentRequest> requests, Guid dentalID)
        {
            FollowUpAppointmentDAO.Instance.CreateFollowAppointments(requests, dentalID);
        }

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            return FollowUpAppointmentDAO.Instance.GetFollowUpAppointmentsByDentalID(dentalID);
        }
    }
}