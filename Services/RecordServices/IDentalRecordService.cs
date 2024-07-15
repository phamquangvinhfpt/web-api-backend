using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Data;
using DAO.Requests;

namespace Services.RecordServices
{
    public interface IDentalRecordService
    {
        public List<DentalRecordData> getAllRecord();
        public DentalRecord GetRecordByID(Guid id);
        public Appointment CreateDentalRecord(CreateDentalRecordRequest request, Guid userID);
        public DentalRecord GetByAppointment(Guid appointmentId);
        public List<DentalRecordData> GetRecordByCustomer(Guid customerID);
        public List<DentalRecordData> GetRecordByDentist(Guid dentisID);
        public List<DentalRecordData> GetRecordByClinicOwner(Guid ownerID);
    }
}