using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Services.Clinics;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Models.Clinics;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ClinicsController : ControllerBase
    {
        private IClinicsService _clinicsService;
        public ClinicsController(IClinicsService clinicsService)
        {
            _clinicsService = clinicsService;
        }

        [HttpGet("Clinics")]
        public List<Clinic> GetAllClinics()
        {
            var clinics = _clinicsService.GetAllClinics();
            return clinics;
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
                _clinicsService.AddClinics(entity);
                return CreatedAtAction(nameof(GetClinicsById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
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
