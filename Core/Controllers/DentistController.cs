using System;
using System.Threading.Tasks;

using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dentist;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [AllowAnonymous]
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _dentistService;

        public DentistController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        // GET: api/Dentists
        [HttpGet]
        public async Task<IActionResult> GetDentists()
        {
            var dentists = await _dentistService.GetAllDentists();
            return Ok(dentists);
        }

        // GET: api/Dentists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDentistById(Guid id)
        {
            var dentist = await _dentistService.GetDentistById(id);
            if (dentist == null)
            {
                return NotFound("Dentist not found");
            }
            return Ok(dentist);
        }

        // POST: api/Dentists
        [HttpPost]
        public async Task<IActionResult> CreateDentist([FromBody] DentistDetailDTO dentist)
        {
            await _dentistService.CreateDentist(dentist);
            return CreatedAtAction(nameof(GetDentistById), new { id = dentist.DentistId }, new
            {
                Data = dentist,
                Message = "Created Successfully",
                Status = true
            });
        }

        // PUT: api/Dentists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDentist(Guid id, [FromBody] DentistDetailDTO dentist)
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

        // DELETE: api/Dentists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDentist(Guid id)
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
    }
}
