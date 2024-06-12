using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;

namespace Repository.MedicalRecords
{
    public interface IMedicalRecordRepository
    {
        public void CreateMedicalRecord(MedicalRecordRequest request, Guid appoinmentid, Guid dentalID);
        public MedicalRecord GetMedicalRecordByDentalID(Guid dentalID);
    }
}