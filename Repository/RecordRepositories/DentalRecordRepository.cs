using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.RecordDAO;
using DAO.Requests;

namespace Repository.RecordRepositories
{
    public class DentalRecordRepository : IDentalRecordRepository
    {
        public DentalRecord CreateDentalRecord(Guid appointmentid, Guid userID) => DentalRecordDAO.Instance.CreateDentalRecord(appointmentid, userID);

        public List<DentalRecord> getAllRecord() => DentalRecordDAO.Instance.getAllRecord();

        public DentalRecord GetByAppointment(Guid appointmentId) => DentalRecordDAO.Instance.GetByAppointment(appointmentId);

        public List<DentalRecord> GetRecordByClinicOwner(Guid ownerID) => DentalRecordDAO.Instance.GetRecordByClinicOwner(ownerID);

        public List<DentalRecord> GetRecordByCustomer(Guid customerID) => DentalRecordDAO.Instance.GetRecordByCustomer(customerID);

        public List<DentalRecord> GetRecordByDentist(Guid dentisID) => DentalRecordDAO.Instance.GetRecordByDentist(dentisID);

        public DentalRecord GetRecordByID(Guid id) => DentalRecordDAO.Instance.GetRecordByID(id);
    }
}