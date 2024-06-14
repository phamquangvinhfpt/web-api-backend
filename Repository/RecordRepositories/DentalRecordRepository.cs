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
        public DentalRecord CreateDentalRecord(Guid appointmentid) => DentalRecordDAO.Instance.CreateDentalRecord(appointmentid);

        public List<DentalRecord> getAllRecord() => DentalRecordDAO.Instance.getAllRecord();

        public DentalRecord GetRecordByID(Guid id) => DentalRecordDAO.Instance.GetRecordByID(id);
    }
}