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
        public PrescriptionController()
        {
            _service = new PrescriptionService();
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] AddPrescription addPrescription)
        {
            _service.CreatePrescription(addPrescription.Prescriptions, addPrescription.dentalId, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));
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
