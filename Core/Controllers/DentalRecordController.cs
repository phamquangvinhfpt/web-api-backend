using BusinessObject.Models;
using Core.Helpers;
using Core.Models;
using Core.Services;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC;
using Services.RecordServices;
using System.Security.Policy;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DentalRecordController : ControllerBase
    {
        private readonly IDentalRecordService _recordService;
        private UserManager<AppUser> _userManager;
        private readonly IUserService _useService;
        private readonly IDentalRecordService dentalRecordService;
        private IMailService _mailService;
        public DentalRecordController(UserManager<AppUser> userManager, IUserService userService, IMailService mailService)
        {
            _recordService = new DentalRecordService();
            _userManager = userManager;
            _useService = userService;
            dentalRecordService = new DentalRecordService();
            _mailService = mailService;
        }

        [HttpGet("getRecords")]
        public async Task<IActionResult> GetAllRecords()
        {
            return StatusCode(StatusCodes.Status200OK,
                new ResponseManager { 
                    IsSuccess = true,
                    Message = new List<dynamic> { _recordService.getAllRecord() },
                    Errors = null
                });
        }

        [HttpPost("getRecord")]
        public async Task<IActionResult> GetAllRecordByID([FromBody] Guid id)
        {
            return StatusCode(StatusCodes.Status200OK,
                new ResponseManager { 
                    IsSuccess = true,
                    Message = new List<dynamic> { _recordService.GetRecordByID(id) },
                    Errors = null
                });
        }

        [HttpPost("createDentalRecord")]
        public async Task<IActionResult> CreateDentalRecord([FromBody] CreateDentalRecordRequest request)
        {
            var appointment = _recordService.CreateDentalRecord(request);

            var dentist = await _userManager.FindByIdAsync(appointment.DentistID.ToString());

            var patient = await _userManager.FindByIdAsync(appointment.PatientID.ToString());

            var dental = dentalRecordService.GetByAppointment(appointment.Id);
            var flu = dental.FollowUpAppointments[dental.FollowUpAppointments.Count - 1];
            var appointmentDate = flu.ScheduledDate.ToString("dd-MM-yyyy");
            var mailContent = new MailRequest
            {
                ToEmail = patient.Email,
                Subject = "Your Dental Record",
                Body = $"<p>Hi {patient.FullName},</p>"
                    + $"<p>Thank you for your visit and for trusting our dental services.</p>"
                    + $"<p>We would like to see you at {appointmentDate} Because {flu.Reason}</p>"
                    + $"<p>Please let us know your availability, and we will be happy to arrange a convenient time for your next visit.</p>"
                    + $"<p>We appreciate your ongoing trust in our dental practice and look forward to seeing you again soon.</p>"
                    + $"<p>Best regards,</p>"
                    + $"<p>{dentist.FullName}</p>"
            };
            await _mailService.SendEmailAsync(mailContent);
            return StatusCode(StatusCodes.Status200OK,
                 new ResponseManager
                 {
                     IsSuccess = true,
                     Message = "Create record success",
                     Errors = null
                 });
        }

    }
}
