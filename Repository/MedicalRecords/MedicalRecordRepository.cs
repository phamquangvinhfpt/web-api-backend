using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.MedicalDAO;
using DAO.Requests;

namespace Repository.MedicalRecords
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        public void CreateMedicalRecord(MedicalRecordRequest request, Guid appoinmentid, Guid dentalID) => MedicalRecordDAO.Instance.CreateMedicalRecord(request, appoinmentid, dentalID);

        public MedicalRecord GetMedicalRecordByDentalID(Guid dentalID) => MedicalRecordDAO.Instance.GetMedicalRecordByDentalID(dentalID);
    }
}