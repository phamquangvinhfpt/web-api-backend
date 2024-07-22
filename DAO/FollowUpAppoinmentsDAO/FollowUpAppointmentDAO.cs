using Azure.Core;
using BusinessObject.Data;
using BusinessObject.Models;
using DAO.RecordDAO;
using DAO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.FollowUpAppoinmentsDAO
{
    public class FollowUpAppointmentDAO
    {
        private readonly AppDbContext _context;

        public FollowUpAppointmentDAO(AppDbContext context)
        {
            _context = context;
        }

        public async void UpdateStatus(Guid id, bool status)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var flu = _context.FollowUpAppointments.FirstOrDefault(p => p.Id == id);
                flu.IsSuccess = status;
                _context.FollowUpAppointments.Update(flu);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async void CreateFollowAppointments(FollowUpAppointmentRequest requests, Guid dentalID, Guid userID)
        {
            var transaction = _context.Database.BeginTransaction();
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
                await _context.SaveChangesAsync(userID);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                return _context.FollowUpAppointments.Where(p => p.DentalRecordId == dentalID).ToList();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public List<FollowUpAppointment> GetAllIsFalse()
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                return _context.FollowUpAppointments.Where(p => p.IsSuccess == false).ToList();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
