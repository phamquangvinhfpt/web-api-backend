using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Enums;
using BusinessObject.Models;
using DAO.Data;
using DAO.Requests;
using Microsoft.Extensions.Logging;
using Repository.Appointments;
using Repository.Clinics;
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
        private readonly IClinicsRepository clinicsRepository;
        public DentalRecordService(){
            clinicsRepository = new ClinicsRepository();
            dentalRecordRepository = new DentalRecordRepository();
            appointmentRepository = new AppointmentRepository();
            followUpAppointmentRepository = new FollowUpAppointmentRepository();
            prescriptionRepository = new PrescriptionRepository();
            medicalRecordRepository = new MedicalRecordRepository();
        }

        public Appointment CreateDentalRecord(CreateDentalRecordRequest request, Guid userID)
        {
            var appointment = appointmentRepository.GetAppointmentByID(request.appointmentID);
            if(appointment == null)
            {
                throw new Exception("Appointment is not found");
            }
            var dental = dentalRecordRepository.CreateDentalRecord(appointment.Id, userID);
            medicalRecordRepository.CreateMedicalRecord(request.MedicalRecordRequest, appointment.Id, dental.Id, userID);
            followUpAppointmentRepository.CreateFollowAppointments(request.followUpAppointmentRequest, dental.Id, userID);
            prescriptionRepository.CreatePrescription(request.prescriptionRequests, dental.Id, userID);
/*            appointmentRepository.ChangeStatusAppointment(appointment.Id, BusinessObject.Enums.AppointmentStatus.Completed);*/
            return appointment;
        }

        public List<DentalRecordData> getAllRecord()
        {
            var list = dentalRecordRepository.getAllRecord();
            List<DentalRecordData> result = new List<DentalRecordData>();
            foreach( var record in list )
            {
                result.Add(new DentalRecordData()
                {
                    patient = record.Appointment.Patient.FullName,
                    dentist = record.Appointment.Dentist.FullName,
                    appointmentID = record.AppointmentID.ToString(),
                    date = record.Appointment.Date,
                    duration = record.Appointment.duration,
                    status = record.Appointment.Status,
                    timeSlot = record.Appointment.TimeSlot.ToString(),
                    type = record.Appointment.Type,
                    createdAt = record.CreatedAt,
                    updatedAt= record.UpdatedAt,
                    id = record.Id.ToString(),
                    
                    
                });
            }
            return result;
        }

        public DentalRecord GetByAppointment(Guid appointmentId) => dentalRecordRepository.GetByAppointment(appointmentId);

        public DentalRecord GetRecordByID(Guid id)
        {
            var dental = dentalRecordRepository.GetRecordByID(id);
            var clinic = clinicsRepository.GetClinicsById(dental.Appointment.ClinicID);
            dental.Appointment.Clinic = clinic;
            return dental;
        }
    }
}