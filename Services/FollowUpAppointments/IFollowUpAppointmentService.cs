using BusinessObject.Models;
using DAO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FollowUpAppointments
{
    public interface IFollowUpAppointmentService
    {
        Task RemindFollowUpAppointment();
        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID);
        public List<FollowUpAppointment> GetAllIsFalse();
        public void UpdateStatus(Guid id, bool status);
        public void CreateFollowAppointments(FollowUpAppointmentRequest request, Guid dentalID, Guid userID);
    }
}
