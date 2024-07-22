using Core.Models;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.FollowUpAppointments;
using System.Security.Claims;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FollowUpAppointmentController : ControllerBase
    {
        private readonly IFollowUpAppointmentService _followUpAppointmentService;
        private readonly ILogger<FollowUpAppointmentController> _logger;
        public FollowUpAppointmentController(ILogger<FollowUpAppointmentController> logger, IFollowUpAppointmentService followUpAppointmentService)
        {
            _followUpAppointmentService = followUpAppointmentService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddFollowUp([FromBody] CreateFollowUp createFollowUp)
        {
            try
            {
                _followUpAppointmentService.CreateFollowAppointments(createFollowUp.Flu, createFollowUp.DentalId, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));
                return StatusCode(StatusCodes.Status200OK,
                     new ResponseManager
                     {
                         IsSuccess = true,
                         Message = "Create success",
                         Errors = null
                     });
            }
            catch (Exception ex)
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
