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
        public void CreateFollowAppointments(FollowUpAppointmentRequest request, Guid dentalID)
        {
            FollowUpAppointmentDAO.Instance.CreateFollowAppointments(request, dentalID);
        }
        public void UpdateStatus(Guid id, bool status) => FollowUpAppointmentDAO.Instance.UpdateStatus(id, status);
        public List<FollowUpAppointment> GetAllIsFalse() => FollowUpAppointmentDAO.Instance.GetAllIsFalse();

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            return FollowUpAppointmentDAO.Instance.GetFollowUpAppointmentsByDentalID(dentalID);
        }
    }
}