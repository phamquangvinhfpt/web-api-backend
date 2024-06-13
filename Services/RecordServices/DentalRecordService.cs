using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using DAO.Requests;
using Repository.Appointments;
using Repository.FollowUpAppointments;
using Repository.MedicalRecords;
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
        private readonly IMedicalRecordRepository medicalRecordRepository;
        public DentalRecordService(){
            dentalRecordRepository = new DentalRecordRepository();
            appointmentRepository = new AppointmentRepository();
            followUpAppointmentRepository = new FollowUpAppointmentRepository();
            prescriptionRepository = new PrescriptionRepository();
            medicalRecordRepository = new MedicalRecordRepository();
        }

        public void CreateDentalRecord(CreateDentalRecordRequest request)
        {
            var appointment = appointmentRepository.CreateAppointment(request.appointment);
            var dental = dentalRecordRepository.CreateDentalRecord(appointment.Id);
            medicalRecordRepository.CreateMedicalRecord(request.MedicalRecordRequest, appointment.Id, dental.Id);
            followUpAppointmentRepository.CreateFollowAppointments(request.followUpAppointmentRequests, dental.Id);
            prescriptionRepository.CreatePrescription(request.prescriptionRequests, dental.Id);

        }

        public List<DentalRecord> getAllRecord()
        {
            return dentalRecordRepository.getAllRecord();
        }

        public DentalRecord GetRecordByID(Guid id)
        {
            return dentalRecordRepository.GetRecordByID(id);
        }
    }
}