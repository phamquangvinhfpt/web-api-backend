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

        public Appointment CreateDentalRecord(CreateDentalRecordRequest request)
        {
            var appointment = appointmentRepository.GetAppointmentByID(request.appointmentID);
            if(appointment == null)
            {
                throw new Exception("Appointment is not found");
            }
            var dental = dentalRecordRepository.CreateDentalRecord(appointment.Id);
            medicalRecordRepository.CreateMedicalRecord(request.MedicalRecordRequest, appointment.Id, dental.Id);
            followUpAppointmentRepository.CreateFollowAppointments(request.followUpAppointmentRequest, dental.Id);
            prescriptionRepository.CreatePrescription(request.prescriptionRequests, dental.Id);
            appointmentRepository.ChangeStatusAppointment(appointment.Id, BusinessObject.Enums.AppointmentStatus.Completed);
            return appointment;
        }

        public List<DentalRecord> getAllRecord()
        {
            return dentalRecordRepository.getAllRecord();
        }

        public DentalRecord GetByAppointment(Guid appointmentId) => dentalRecordRepository.GetByAppointment(appointmentId);

        public DentalRecord GetRecordByID(Guid id)
        {
            return dentalRecordRepository.GetRecordByID(id);
        }
    }
}