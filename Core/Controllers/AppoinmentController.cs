using BusinessObject.Models;
using Core.Auth.Services;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Appoinmets;
using Services.Clinics;
using System;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AppoinmentController : ControllerBase
    {
        private readonly IAppoinmentService _appoinmentService;
        private readonly ILogger<AppoinmentController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private UserManager<AppUser> _userManager;

        public AppoinmentController( ILogger<AppoinmentController> logger, ICurrentUserService currentUserService, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _appoinmentService = new AppoinmentService();
            _logger = logger;
            this._currentUserService = currentUserService;
            _roleManager = roleManager;
            _userManager = userManager;

        }

        // GET: api/<AppoinmentController>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                if (!Guid.TryParse(currentUserId, out Guid userId))
                {
                    _logger.LogWarning("ID người dùng không hợp lệ hoặc không tìm thấy.");
                    return Unauthorized("Vui lòng đăng nhập để xem các cuộc hẹn.");
                }

                _logger.LogInformation($"Đang lấy danh sách cuộc hẹn cho người dùng có ID: {userId}");
                var appointments = _appoinmentService.GetAppointmentsForUser(userId);

                if (appointments == null || !appointments.Any())
                {
                    _logger.LogWarning("Không tìm thấy cuộc hẹn nào.");
                    return NotFound("Không tìm thấy cuộc hẹn nào cho tài khoản của bạn.");
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách cuộc hẹn.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi khi xử lý yêu cầu của bạn.");
            }
        }

        // GET api/<AppoinmentController>/5
        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            try
            {
                _logger.LogInformation($"Fetching appointment with ID: {id}");
                var appoinment = _appoinmentService.GetAppointmentByID(id);
                if (appoinment == null)
                {
                    _logger.LogWarning($"Appointment with ID: {id} not found.");
                    return NotFound();
                }
                return Ok(appoinment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching appointment with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<AppoinmentController>
        [HttpPost("Create")]
        public IActionResult Post([FromBody] AppointmentRequest appointment)
        {
            if (appointment == null)
            {
                _logger.LogWarning("Invalid appointment request.");
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Creating a new appointment.");
                _appoinmentService.CreateAppointment(appointment);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating appointment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT api/<AppoinmentController>/5
        [HttpPut("Update")]
        public IActionResult Put(Guid id, [FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                _logger.LogWarning("Invalid appointment request.");
                return BadRequest();
            }

            try
            {
                _logger.LogInformation($"Updating appointment with ID: {id}");
                var updateAppointment = _appoinmentService.GetAppointmentByID(id);
                if (updateAppointment == null)
                {
                    _logger.LogWarning($"Appointment with ID: {id} not found.");
                    return NotFound();
                }

                _appoinmentService.UpdateAppointment(appointment);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating appointment with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<AppoinmentController>/5
        [HttpDelete("Delete")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _logger.LogInformation($"Deleting appointment with ID: {id}");
                var appoinment = _appoinmentService.GetAppointmentByID(id);
                if (appoinment == null)
                {
                    _logger.LogWarning($"Appointment with ID: {id} not found.");
                    return NotFound();
                }

                _appoinmentService.DeleteAppointment(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting appointment with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<AppoinmentController>/CreateAppointmentForPeriodic
        [HttpPost("CreateAppointmentForPeriodic")]
        public IActionResult CreateAppointmentForPeriodic([FromBody] AppointmentRequest appointment)
        {
            if (appointment == null)
            {
                _logger.LogWarning("Invalid appointment request.");
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Creating a new periodic appointment.");
                _appoinmentService.CreateAppointmentForPeriodic(appointment);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating periodic appointment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // getbyDentistID
        [HttpGet("GetByDentistID")]
        public IActionResult GetByDentistID(Guid dentistID)
        {
            try
            {
                _logger.LogInformation($"Fetching appointments with Dentist ID: {dentistID}");
                var appoinments = _appoinmentService.GetByDentistID(dentistID);
                if (appoinments == null)
                {
                    _logger.LogWarning($"No appointments found with Dentist ID: {dentistID}");
                    return NotFound("No appointments found.");
                }
                return Ok(appoinments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching appointments with Dentist ID: {dentistID}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


    }
}
