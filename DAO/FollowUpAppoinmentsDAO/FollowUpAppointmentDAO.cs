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
        public void CreateFollowAppointments(List<FollowUpAppointmentRequest> requests, Guid dentalID)
        {
            List<FollowUpAppointment> listFLUA = new List<FollowUpAppointment>();
            foreach (var item in requests)
            {
                listFLUA.Add(new FollowUpAppointment
                {
                    DentalRecordId = dentalID,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ScheduledDate = item.ScheduledDate,
                    Reason = item.Reason,
                });
            }

            try
            {
                _context.FollowUpAppointments.AddRange(listFLUA);
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
    }
}
