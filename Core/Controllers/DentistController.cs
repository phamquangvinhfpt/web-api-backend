using System;
using System.Threading.Tasks;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Dentist;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _dentistService;
        private readonly ILogger<DentistController> _logger;

        public DentistController(IDentistService dentistService, ILogger<DentistController> logger)
        {
            _dentistService = dentistService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDentists()
        {
            try
            {
                var dentists = await _dentistService.GetAllDentists();
                return Ok(dentists);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetDentists action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDentistById(Guid id)
        {
            try
            {
                var dentist = await _dentistService.GetDentistById(id);
                if (dentist == null)
                {
                    return NotFound("Dentist not found");
                }
                return Ok(dentist);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetDentistById action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDentist([FromBody] DentistDetailDTO dentist)
        {
            try
            {
                await _dentistService.CreateDentist(dentist);
                return CreatedAtAction(nameof(GetDentistById), new { id = dentist.DentistId }, new
                {
                    Data = dentist,
                    Message = "Created Successfully",
                    Status = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateDentist action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDentist(Guid id, [FromBody] DentistDetailDTO dentist)
        {
            try
            {
                if (!await _dentistService.DentistExists(id))
                {
                    return NotFound("Dentist not found");
                }

                dentist.DentistId = id;
                await _dentistService.UpdateDentist(dentist);
                return Ok(new
                {
                    Data = dentist,
                    Message = "Updated Successfully",
                    Status = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateDentist action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDentist(Guid id)
        {
            try
            {
                if (!await _dentistService.DentistExists(id))
                {
                    return NotFound("Dentist not found");
                }

                await _dentistService.DeleteDentist(id);
                return Ok(new
                {
                    Message = "Deleted Successfully",
                    Status = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteDentist action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
