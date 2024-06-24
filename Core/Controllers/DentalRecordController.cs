using BusinessObject.Models;
using Core.Helpers;
using Core.Models;
using Core.Services;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Org.BouncyCastle.Math.EC;
using Services.RecordServices;
using System.Security.Claims;
using System.Security.Policy;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DentalRecordController : ODataController
    {
        private readonly IDentalRecordService _recordService;
        private UserManager<AppUser> _userManager;
        private IMailService _mailService;
        private readonly ILogger<DentalRecordController> _logger;
        public DentalRecordController(UserManager<AppUser> userManager, IMailService mailService, ILogger<DentalRecordController> logger)
        {
            _recordService = new DentalRecordService();
            _userManager = userManager;
            _mailService = mailService;
            _logger = logger;
        }

        [HttpGet("getRecords")]
        [EnableQuery]
        public async Task<IActionResult> GetAllRecords(ODataQueryOptions<DentalRecord> queryOptions, [FromQuery] PaginationParameters paginationParameters)
        {
            try
            {
                var dentals = _recordService.getAllRecord().AsQueryable();
                dentals = (IQueryable<DentalRecord>)queryOptions.ApplyTo(dentals, new ODataQuerySettings());

                var results = dentals
                    .Skip(paginationParameters.PageSize * (paginationParameters.PageNumber - 1))
                    .Take(paginationParameters.PageSize);
                return StatusCode(StatusCodes.Status200OK,
                new ResponseManager
                {
                    IsSuccess = true,
                    Message = new List<dynamic> { results },
                    Errors = null
                });
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseManager
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Errors = null
                });
            }
        }

        [HttpPost("getRecord")]
        public async Task<IActionResult> GetAllRecordByID([FromBody] Guid id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK,
    new ResponseManager
    {
        IsSuccess = true,
        Message = new List<dynamic> { _recordService.GetRecordByID(id) },
        Errors = null
    });
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseManager
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Errors = null
                });
            }
        }

        [HttpPost("createDentalRecord")]
        public async Task<IActionResult> CreateDentalRecord([FromBody] CreateDentalRecordRequest request)
        {

            try
            {
                var appointment = _recordService.CreateDentalRecord(request, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));

                var dentist = await _userManager.FindByIdAsync(appointment.DentistID.ToString());

                var patient = await _userManager.FindByIdAsync(appointment.PatientID.ToString());

                var dental = _recordService.GetByAppointment(appointment.Id);
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
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseManager
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                        Errors = null
                    });
            }
        }

    }
}
