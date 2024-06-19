using Core.Models;
using DAO.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Prescriptions;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            _service.CreatePrescription(addPrescription.Prescriptions, addPrescription.dentalId);
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
