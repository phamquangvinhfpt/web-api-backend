using BusinessObject.Models;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Appoinmets;
using Services.Clinics;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AppoinmentController : ControllerBase
    {
        private IAppoinmentService  _appoinmentService;
        public AppoinmentController()
        {
            _appoinmentService = new AppoinmentService();
        }
        // GET: api/<AppoinmentController>
        [HttpGet("Get All")]
        public IActionResult Get()
        {
            var appoinments = _appoinmentService.GetAllAppointments();
            return Ok(appoinments);
        }
        // GET api/<AppoinmentController>/5
        [HttpGet("Get by id")]
        public IActionResult Get(Guid id)
        {
            var appoinment = _appoinmentService.GetAppointmentByID(id);
            if (appoinment == null)
            {
                return NotFound();
            }
            return Ok(appoinment);
        }
        // POST api/<AppoinmentController> 
        [HttpPost("Create")]
        public IActionResult Post([FromBody] AppointmentRequest appointment)
        {
            if (appointment != null)
            {
                _appoinmentService.CreateAppointment(appointment);
                return Ok(appointment);
            }
            return BadRequest();
        }
        
        // PUT api/<AppoinmentController>/5
        [HttpPut("update")]
         public IActionResult Put(Guid id, [FromBody] Appointment appointment)
            {
                if (appointment != null)
                {
                    var updateAppointment = _appoinmentService.GetAppointmentByID(id);
                    if (updateAppointment != null)
                    {
                        _appoinmentService.UpdateAppointment(appointment);
                        return Ok(appointment);
                    }
                }
                return NotFound();
            }
        // DELETE api/<AppoinmentController>/5
        [HttpDelete("delete")]
        public IActionResult Delete(Guid id)
        {
            var appoinment = _appoinmentService.GetAppointmentByID(id);
            if (appoinment == null)
            {
                return NotFound();
            }
            _appoinmentService.DeleteAppointment(id);
            return NoContent();
        }
        // Post api/<AppoinmentController>/CreateAppointmentForPeriodic
        [HttpPost("CreateAppointmentForPeriodic")]
        public IActionResult CreateAppointmentForPeriodic([FromBody] AppointmentRequest appointment)
        {
            if (appointment != null)
            {
                _appoinmentService.CreateAppointmentForPeriodic(appointment);
                return Ok(appointment);
            }
            return BadRequest();
        }

    }
}
