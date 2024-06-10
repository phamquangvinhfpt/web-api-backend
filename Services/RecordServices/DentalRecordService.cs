using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repository.RecordRepositories;

namespace Services.RecordServices
{
    public class DentalRecordService : IDentalRecordService
    {
        private readonly IDentalRecordRepository dentalRecordRepository;
        public DentalRecordService(){
            dentalRecordRepository = new DentalRecordRepository();
        }
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