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
        public void CreateDentalRecord(CreateDentalRecordRequest request) => DentalRecordDAO.Instance.CreateDentalRecord(request);

        public List<DentalRecord> getAllRecord() => DentalRecordDAO.Instance.getAllRecord();

        public DentalRecord GetRecordByID(string id) => DentalRecordDAO.Instance.GetRecordByID(id);
    }
}