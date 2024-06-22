using Core.Models;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Prescriptions;
using System.Security.Claims;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _service;
        private readonly ILogger<PrescriptionController> _logger;
        public PrescriptionController(ILogger<PrescriptionController> logger)
        {
            _service = new PrescriptionService();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] AddPrescription addPrescription)
        {
            try
            {
                _service.CreatePrescription(addPrescription.Prescriptions, addPrescription.dentalId, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));
                return StatusCode(StatusCodes.Status200OK,
         new ResponseManager
         {
             IsSuccess = true,
             Message = "Create success",
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
    }
}
