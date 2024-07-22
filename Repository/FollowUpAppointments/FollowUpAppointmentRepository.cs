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
        private readonly FollowUpAppointmentDAO _followUpAppointmentDAO;
        public FollowUpAppointmentRepository(FollowUpAppointmentDAO followUpAppointmentDAO)
        {
            _followUpAppointmentDAO = followUpAppointmentDAO;
        }
        public void CreateFollowAppointments(FollowUpAppointmentRequest request, Guid dentalID, Guid userID)
        {
            _followUpAppointmentDAO.CreateFollowAppointments(request, dentalID, userID);
        }
        public void UpdateStatus(Guid id, bool status) => _followUpAppointmentDAO.UpdateStatus(id, status);
        public List<FollowUpAppointment> GetAllIsFalse() => _followUpAppointmentDAO.GetAllIsFalse();

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            return _followUpAppointmentDAO.GetFollowUpAppointmentsByDentalID(dentalID);
        }
    }
}