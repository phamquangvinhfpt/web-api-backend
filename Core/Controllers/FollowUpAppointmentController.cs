using Core.Models;
using DAO.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.FollowUpAppointments;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowUpAppointmentController : ControllerBase
    {
        private readonly IFollowUpAppointmentService _followUpAppointmentService;
        public FollowUpAppointmentController() {
            _followUpAppointmentService = new FollowUpAppointmentService();
        }

        [HttpPost]
        public async Task<IActionResult> AddFollowUp([FromBody] CreateFollowUp createFollowUp)
        {
            _followUpAppointmentService.CreateFollowAppointments(createFollowUp.Flu, createFollowUp.DentalId);
            return StatusCode(StatusCodes.Status200OK,
                 new ResponseManager
                 {
                     IsSuccess = true,
                     Message = "Create success",
                     Errors = null
                 });
        }
    }
}
