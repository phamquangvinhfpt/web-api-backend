using BusinessObject.Data;
using BusinessObject.Models;
using DAO.Requests;

namespace DAO.FollowUpAppoinmentsDAO
{
    public class FollowUpAppointmentDAO
    {
        private readonly AppDbContext _context;

        public FollowUpAppointmentDAO(AppDbContext context)
        {
            _context = context;
        }

        public void UpdateStatus(Guid id, bool status)
        {
            try
            {
                var flu = _context.FollowUpAppointments.FirstOrDefault(p => p.Id == id);
                flu.IsSuccess = status;
                _context.FollowUpAppointments.Update(flu);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void CreateFollowAppointments(FollowUpAppointmentRequest requests, Guid dentalID, Guid userID)
        {
            try
            {
                _context.FollowUpAppointments.Add(new FollowUpAppointment
                {
                    DentalRecordId = dentalID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ScheduledDate = requests.ScheduledDate,
                    Reason = requests.Reason,
                    IsSuccess = false
                });
                _context.SaveChangesAsync(userID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            try
            {
                return _context.FollowUpAppointments.Where(p => p.DentalRecordId == dentalID).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<FollowUpAppointment> GetAllIsFalse()
        {
            try
            {
                return _context.FollowUpAppointments.Where(p => p.IsSuccess == false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
