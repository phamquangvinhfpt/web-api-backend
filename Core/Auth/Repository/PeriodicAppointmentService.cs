using BusinessObject.Models;
using Core.Auth.Repository;
using DAO.Requests;
using Repository.Appointments;
using Services.Appoinmets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using Core.Helpers;
using NuGet.Protocol.Core.Types;
using Repository.FollowUpAppointments;
using Repository.RecordRepositories;
using DAO.Data;
namespace Core.Auth.Repository

{
    public class PeriodicAppointmentService : IAppoinmentService
    {
        private IAppointmentRepository appointmentRepository;
        private IMailService _mailService;
        private readonly ILogger<PeriodicAppointmentService> _logger;

        public PeriodicAppointmentService(IMailService mailService, ILogger<PeriodicAppointmentService> logger)
        {
            appointmentRepository = new AppointmentRepository();
            _mailService = mailService;
            _logger = logger;
        }

        public Appointment CreateAppointment(AppointmentRequest request)
        {
            throw new NotImplementedException();
        }

        public Appointment CreateAppointmentForPeriodic(AppointmentRequest request)
        {
            throw new NotImplementedException();
        }

        public void DeleteAppointment(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Appointment> GetAllAppointments()
        {
            throw new NotImplementedException();
        }

        public Appointment GetAppointmentByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAppointment(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public void UpdateAppointmentDate(Guid Id, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Appointment> GetByDentistID(Guid dentistID)
        {
            throw new NotImplementedException();
        }

        async Task IAppoinmentService.PeriodicAppointment()
        {
            //var list lấy hàm Getallstatusandtype ra
           var list = appointmentRepository.GetAllByStatusAndType(BusinessObject.Enums.AppointmentStatus.Scheduled, BusinessObject.Enums.AppointmentType.Periodic);
            if (list != null)
            {
                DateTime today = DateTime.Now;
                foreach (var appointment in list)
                {
                    var i = appointment.Date;
                    var t = today.Date.AddDays(1);
                    if (i.Day == t.Day)
                    {
                        SendMailToRemind(appointment.Patient, appointment.Dentist, i);

                        var nextAppointmentDate = appointment.Date.AddDays(appointment.duration);
                        appointment.Date = nextAppointmentDate;
                        appointmentRepository.UpdateAppointment(appointment);

                    }
                }
            }
        }

        private void SendMailToRemind(AppUser patient, AppUser dentist, DateTime date)
        {
            // Kiểm tra nếu patient hoặc dentist là null

            var appointmentDate = date.Date.ToString("dd-MM-yyyy"); // Chỉnh lại để cho phù hợp với ngày tái khám mới
            var mailContent = new MailRequest
            {
                ToEmail = patient.Email,
                Subject = "Appointment Reminder",
                Body = $"Dear {patient.FullName}, <br/>" +
                       $"This is a reminder for your upcoming dental appointment with {dentist.FullName} on {appointmentDate}. <br/>" +
                       $"Please let us know if you're available or need to reschedule. <br/>" +
                       $"Looking forward to seeing you.<br/><br/>" +
                       $"Best regards,<br/>" +
                       $"{dentist.FullName}"
            };
            // Gửi email bằng cách sử dụng dịch vụ gửi mail
             _mailService.SendEmailAsync(mailContent);
        }

        public AppointmentData GetAppointmentForCreateDentalByID(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
