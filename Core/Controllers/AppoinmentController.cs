using System.Security.Claims;
using BusinessObject.Enums;
using BusinessObject.Models;
using Core.Auth.Permissions;
using Core.Auth.Services;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Appoinmets;
using Action = Core.Auth.Permissions.Action;

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
        private readonly IUriService uriService;

        public AppoinmentController( ILogger<AppoinmentController> logger, ICurrentUserService currentUserService, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUriService uriService)
        {
            _appoinmentService = new AppoinmentService();
            _logger = logger;
            this._currentUserService = currentUserService;
            _roleManager = roleManager;
            _userManager = userManager;
            this.uriService = uriService;
        }

        // GET: api/<AppoinmentController>
        [HttpGet("GetAll")]
        //permission [MustHavePermission]
        [MustHavePermission( Action.View, Resource.Appointments)]
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
        //permission [MustHavePermission]
        [MustHavePermission(Action.View, Resource.Appointments)]
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
        //permission [MustHavePermission]
        [MustHavePermission(Action.Create, Resource.Appointments)]
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
        //permission [MustHavePermission]
        [MustHavePermission(Action.Update, Resource.Appointments)]
        public IActionResult UpdateAppointment(Guid id, [FromBody] AppointmentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request.");
            }

            try
            {
                // Lấy cuộc hẹn hiện tại
                var existingAppointment = _appoinmentService.GetAppointmentByID(id);
                if (existingAppointment == null)
                {
                    return NotFound("Appointment not found.");
                }

                // Tạo đối tượng cuộc hẹn với các thuộc tính cần cập nhật
                var appointmentToUpdate = new Appointment
                {
                    Id = id,
                    PatientID = request.PatientID,
                    DentistID = request.DentistID,
                    ClinicID = request.ClinicID,
                    Date = request.Date,
                    TimeSlot = request.TimeSlot,
                    duration = existingAppointment.duration, // Giữ nguyên giá trị hiện tại
                    Type = existingAppointment.Type, // Giữ nguyên giá trị hiện tại
                    Status = existingAppointment.Status, // Giữ nguyên giá trị hiện tại
                    CreatedAt = existingAppointment.CreatedAt, // Giữ nguyên giá trị hiện tại
                    UpdatedAt = DateTime.Now // Cập nhật thời gian sửa đổi
                };

                _appoinmentService.UpdateAppointment(appointmentToUpdate);
                return Ok("Appointment updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<AppoinmentController>/5
        [HttpDelete("Delete")]
        //permission [MustHavePermission]
        [MustHavePermission(Action.Delete, Resource.Appointments)]
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
        //permission [MustHavePermission]
        [MustHavePermission(Action.Create, Resource.Appointments)]
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
        //permission [MustHavePermission]
        [MustHavePermission(Action.View, Resource.Appointments)]
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

        [HttpGet("ChangeStatus")]
        public IActionResult ChangeStatus(Guid appointmentID, AppointmentStatus status)
        {
            try
            {
                _appoinmentService.ChangeStatusAppointment(appointmentID, status, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));
                return Ok("Change status successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        //api /<AppoinmentController>/SearchAppointmentByName
        [HttpGet("SearchAppointmentByName")]
        //permission [MustHavePermission]
        [MustHavePermission(Action.View, Resource.Appointments)]
        public IActionResult SearchAppointmentByName(string name)
        {
            try
            {
                _logger.LogInformation($"Searching appointments with name: {name}");
                var appoinments = _appoinmentService.SearchAppointmentByName(name);
                if (appoinments == null)
                {
                    _logger.LogWarning($"No appointments found with name: {name}");
                    return NotFound("No appointments found.");
                }
                return Ok(appoinments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching appointments with name: {name}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


    }
}
