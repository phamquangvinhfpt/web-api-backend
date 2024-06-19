using BusinessObject.Models;
using Services.Clinics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Models.Clinics;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Core.Models;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ClinicsController : ODataController
    {
        private IClinicsService _clinicsService;
        private readonly ILogger<ClinicsController> _logger;

        public ClinicsController(IClinicsService clinicsService, ILogger<ClinicsController> logger)
        {
            _clinicsService = clinicsService;
            _logger = logger;
        }

        [HttpGet("Clinics")]
        [EnableQuery]
        public IActionResult GetAllClinics(ODataQueryOptions<Clinic> queryOptions, [FromQuery] PaginationParameters paginationParameters)
        {
            try
            {
                var clinics = _clinicsService.GetAllClinics().AsQueryable();

                // Apply OData query options to enable pagination, search, sort, and filter
                clinics = (IQueryable<Clinic>)queryOptions.ApplyTo(clinics, new ODataQuerySettings());

                var results = clinics
                    .Skip(paginationParameters.PageSize * (paginationParameters.PageNumber - 1))
                    .Take(paginationParameters.PageSize);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllClinics action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Clinic> GetClinicsById(Guid id)
        {
            try
            {
                var clinic = _clinicsService.GetClinicsById(id);
                if (clinic == null)
                {
                    return NotFound();
                }
                return Ok(clinic);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetClinicsById action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("AddClinics")]
        public async Task<ActionResult> AddClinics([FromBody] ClinicsModel clinicsmodel)
        {
            try
            {
                var entity = new Clinic
                {
                    Name = clinicsmodel.Name,
                    OwnerID = clinicsmodel.OwnerID,
                    Address = clinicsmodel.Address,
                    Verified = clinicsmodel.Verified
                };
                _clinicsService.AddClinics(entity, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));
                return CreatedAtAction(nameof(GetClinicsById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddClinics action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("UpdateClinics/{id}")]
        public async Task<ActionResult> UpdateClinics(Guid id, [FromBody] ClinicsModel clinicsModel)
        {
            try
            {
                var existingClinic = _clinicsService.GetClinicsById(id);
                if (existingClinic == null)
                {
                    return NotFound();
                }

                existingClinic.Name = clinicsModel.Name;
                existingClinic.OwnerID = clinicsModel.OwnerID;
                existingClinic.Address = clinicsModel.Address;
                existingClinic.Verified = clinicsModel.Verified;

                _clinicsService.UpdateClinics(existingClinic);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteClinics/{id}")]
        public async Task<ActionResult> DeleteClinics(Guid id)
        {
            try
            {
                var existingClinic = _clinicsService.GetClinicsById(id);
                if (existingClinic == null)
                {
                    return NotFound();
                }

                _clinicsService.DeleteClinics(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
