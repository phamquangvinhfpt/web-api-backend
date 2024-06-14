using Core.Models;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.RecordServices;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DentalRecordController : ControllerBase
    {
        private readonly IDentalRecordService _recordService;

        public DentalRecordController()
        {
            _recordService = new DentalRecordService();
        }

        [HttpGet("getRecords")]
        public async Task<IActionResult> GetAllRecords()
        {
            return StatusCode(StatusCodes.Status200OK,
                new ResponseManager { 
                    IsSuccess = true,
                    Message = new List<dynamic> { _recordService.getAllRecord() },
                    Errors = null
                });
        }

        [HttpPost("getRecord")]
        public async Task<IActionResult> GetAllRecordByID([FromBody] Guid id)
        {
            return StatusCode(StatusCodes.Status200OK,
                new ResponseManager { 
                    IsSuccess = true,
                    Message = new List<dynamic> { _recordService.GetRecordByID(id) },
                    Errors = null
                });
        }

        [HttpPost("createDentalRecord")]
        public async Task<IActionResult> CreateDentalRecord([FromBody] CreateDentalRecordRequest request)
        {
            _recordService.CreateDentalRecord(request);
            return StatusCode(StatusCodes.Status200OK,
                 new ResponseManager { 
                    IsSuccess = true,
                    Message = "Create record success",
                    Errors = null
                });
        }

    }
}
