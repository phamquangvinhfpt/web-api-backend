using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Models;
using Core.Auth.Services.Clinics;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public Clinic GetClinicsById(Guid Id)
        {
            return _clinicsService.GetClinicsById(Id);
        }

        [HttpPost]
        public async Task<ActionResult> AddClinics([FromBody] Clinic clinic)
        {
            try
            {
                 _clinicsService.AddClinics(clinic);
                return CreatedAtAction(nameof(GetClinicsById), new { id = clinic.Id }, clinic);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
