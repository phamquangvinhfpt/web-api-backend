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
        public DentalRecord CreateDentalRecord(Guid appointmentid, Guid userID);
        public DentalRecord GetByAppointment(Guid appointmentId);
        public List<DentalRecord> GetRecordByCustomer(Guid customerID);
        public List<DentalRecord> GetRecordByDentist(Guid dentisID);
        public List<DentalRecord> GetRecordByClinicOwner(Guid ownerID);
    }
}