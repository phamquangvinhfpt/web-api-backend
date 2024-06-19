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
        private static FollowUpAppointmentDAO instance = null;
        private AppDbContext _context = null;

        public FollowUpAppointmentDAO()
        {
            _context = new AppDbContext();
        }
        public static FollowUpAppointmentDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FollowUpAppointmentDAO();
                }
                return instance;
            }
        }
        public void UpdateStatus(Guid id, bool status)
        {
            try
            {
                var flu = _context.FollowUpAppointments.FirstOrDefault(p => p.Id == id);
                flu.IsSuccess = status;
                _context.FollowUpAppointments.Update(flu);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void CreateFollowAppointments(FollowUpAppointmentRequest requests, Guid dentalID)
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
                _context.SaveChanges();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            return _context.FollowUpAppointments.Where(p => p.DentalRecordId  == dentalID).ToList();
        }
        public List<FollowUpAppointment> GetAllIsFalse()
        {
            var list = _context.FollowUpAppointments.Where(p => p.IsSuccess == false).ToList();
            return list;
        }
    }
}
