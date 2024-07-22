using BusinessObject.Models;
using Core.Helpers;
using Core.Services;
using DAO.Requests;
using Repository.Appointments;
using Repository.FollowUpAppointments;
using Repository.RecordRepositories;
using Services.FollowUpAppointments;

namespace Core.Auth.Repository
{
    public class RemindFollowAppointmentService : IFollowUpAppointmentService
    {
        private IFollowUpAppointmentRepository repository;
        private IDentalRecordRepository recordRepository;
        private IAppointmentRepository appointmentRepository;
        private IMailService _mailService;
        private readonly ILogger<RemindFollowAppointmentService> logger;
        public RemindFollowAppointmentService(IMailService mailService, ILogger<RemindFollowAppointmentService> logger, IFollowUpAppointmentRepository repository, IDentalRecordRepository recordRepository, IAppointmentRepository appointmentRepository)
        {
            this.repository = repository;
            this.recordRepository = recordRepository;
            this.appointmentRepository = appointmentRepository;
            _mailService = mailService;
            this.logger = logger;
        }

        public void CreateFollowAppointments(FollowUpAppointmentRequest request, Guid dentalID, Guid userID)
        {
            throw new NotImplementedException();
        }

        public List<FollowUpAppointment> GetAllIsFalse()
        {
            throw new NotImplementedException();
        }

        public List<FollowUpAppointment> GetFollowUpAppointmentsByDentalID(Guid dentalID)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(Guid id, bool status)
        {
            throw new NotImplementedException();
        }

        private void SendMailToRemind(AppUser patient, AppUser dentist, FollowUpAppointment appointment)
        {
            try
            {
                var appointmentDate = appointment.ScheduledDate.ToString("dd-MM-yyyy");
                var mailContent = new MailRequest
                {
                    ToEmail = patient.Email,
                    Subject = "Remind For Re-Examination",
                    Body = $"<p>Hi {patient.FullName},</p>"
                        + $"<p>We would like to see you at {appointmentDate} Because {appointment.Reason}</p>"
                        + $"<p>Please let us know your availability, and we will be happy to arrange a convenient time for your next visit.</p>"
                        + $"<p>We appreciate your ongoing trust in our dental practice and look forward to seeing you again soon.</p>"
                        + $"<p>Best regards,</p>"
                        + $"<p>{dentist.FullName}</p>"
                };
                _mailService.SendEmailAsync(mailContent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public Task RemindFollowUpAppointment()
        {
            try
            {
                var list = repository.GetAllIsFalse();

                if (list != null)
                {
                    List<FollowUpAppointment> needs = new List<FollowUpAppointment>();
                    DateTime dateTime = DateTime.Now;
                    foreach (var item in list)
                    {
                        if (item.ScheduledDate.Date == dateTime.Date.AddDays(1))
                        {
                            needs.Add(item);
                        }
                    }
                    if (needs.Count > 0)
                    {
                        foreach (var item in needs)
                        {
                            var dental = recordRepository.GetRecordByID(item.DentalRecordId);
                            var appoint = appointmentRepository.GetAppointmentByID(dental.AppointmentID);
                            appointmentRepository.CreateAppointment(
                                new DAO.Requests.AppointmentRequest
                                {
                                    ClinicID = appoint.ClinicID,
                                    Date = item.ScheduledDate,
                                    DentistID = appoint.DentistID,
                                    PatientID = appoint.PatientID,
                                    TimeSlot = appoint.TimeSlot,
                                    Type = appoint.Type
                                });
                            repository.UpdateStatus(item.Id, true);
                            SendMailToRemind(appoint.Patient, appoint.Dentist, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
