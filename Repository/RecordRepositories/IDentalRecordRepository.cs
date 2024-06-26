using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;

namespace Repository.RecordRepositories
{
    public interface IDentalRecordRepository
    {
        public DentalRecord GetRecordByID(Guid id);
        public List<DentalRecord> getAllRecord();
        public DentalRecord CreateDentalRecord(Guid appointmentid);
        public DentalRecord GetByAppointment(Guid appointmentId);
    }
}