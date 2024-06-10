using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Repository.RecordRepositories
{
    public interface IDentalRecordRepository
    {
        public DentalRecord GetRecordByID(string id);
        public List<DentalRecord> getAllRecord();
    }
}