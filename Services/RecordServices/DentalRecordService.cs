using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;
using Repository.RecordRepositories;

namespace Services.RecordServices
{
    public class DentalRecordService : IDentalRecordService
    {
        private readonly IDentalRecordRepository dentalRecordRepository;
        public DentalRecordService(){
            dentalRecordRepository = new DentalRecordRepository();
        }

        public void CreateDentalRecord(CreateDentalRecordRequest request) => dentalRecordRepository.CreateDentalRecord(request);

        public List<DentalRecord> getAllRecord()
        {
            return dentalRecordRepository.getAllRecord();
        }

        public DentalRecord GetRecordByID(string id)
        {
            return dentalRecordRepository.GetRecordByID(id);
        }
    }
}