using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;
using Repository.Appointments;
using Repository.FollowUpAppointments;
using Repository.Prescriptions;
using Repository.RecordRepositories;

namespace Services.RecordServices
{
    public class DentalRecordService : IDentalRecordService
    {
        private readonly IDentalRecordRepository dentalRecordRepository;
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IFollowUpAppointmentRepository followUpAppointmentRepository;
        private readonly IPrescriptionRepository prescriptionRepository;
        public DentalRecordService(){
            dentalRecordRepository = new DentalRecordRepository();
            appointmentRepository = new AppointmentRepository();
            followUpAppointmentRepository = new FollowUpAppointmentRepository();
            prescriptionRepository = new PrescriptionRepository();
        }

        public void CreateDentalRecord(CreateDentalRecordRequest request)
        {
            var appointment = appointmentRepository.CreateAppointment(request.appointment);
            var dental = dentalRecordRepository.CreateDentalRecord(request);
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