using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Services.RecordServices
{
    public interface IDentalRecordService
    {
        public List<DentalRecord> getAllRecord();
        public DentalRecord GetRecordByID(string id);
    }
}