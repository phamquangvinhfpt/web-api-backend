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
        private readonly DentalRecordDAO _dentalRecordDAO;
        public DentalRecordRepository(DentalRecordDAO dentalRecordDAO)
        {
            _dentalRecordDAO = dentalRecordDAO;
        }
        public DentalRecord CreateDentalRecord(Guid appointmentid, Guid userID) => _dentalRecordDAO.CreateDentalRecord(appointmentid, userID);

        public List<DentalRecord> getAllRecord() => _dentalRecordDAO.getAllRecord();

        public DentalRecord GetByAppointment(Guid appointmentId) => _dentalRecordDAO.GetByAppointment(appointmentId);

        public List<DentalRecord> GetRecordByClinicOwner(Guid ownerID) => _dentalRecordDAO.GetRecordByClinicOwner(ownerID);

        public List<DentalRecord> GetRecordByCustomer(Guid customerID) => _dentalRecordDAO.GetRecordByCustomer(customerID);

        public List<DentalRecord> GetRecordByDentist(Guid dentisID) => _dentalRecordDAO.GetRecordByDentist(dentisID);

        public DentalRecord GetRecordByID(Guid id) => _dentalRecordDAO.GetRecordByID(id);
    }
}