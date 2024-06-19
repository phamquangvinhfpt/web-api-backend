using BusinessObject.Models;
using Core.Helpers;
using Core.Services;
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
        public RemindFollowAppointmentService(IMailService mailService)
        {
            repository = new FollowUpAppointmentRepository();
            recordRepository = new DentalRecordRepository();
            appointmentRepository = new AppointmentRepository();
            _mailService = mailService;
        }

        async Task IFollowUpAppointmentService.RemindFollowUpAppointment()
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
        private void SendMailToRemind(AppUser patient, AppUser dentist, FollowUpAppointment appointment)
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
    }
}
