using BusinessObject.Models;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Appoinmets;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AppoinmentController : ControllerBase
    {
        private readonly IAppoinmentService _appoinmentService;
        private readonly ILogger<AppoinmentController> _logger;

        public AppoinmentController(ILogger<AppoinmentController> logger)
        {
            _appoinmentService = new AppoinmentService();
            _logger = logger;
        }

        // GET: api/<AppoinmentController>
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all appointments.");
                var appoinments = _appoinmentService.GetAllAppointments();
                if (appoinments == null)
                {
                    _logger.LogWarning("No appointments found.");
                    return NotFound("No appointments found.");
                }
                return Ok(appoinments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all appointments.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
