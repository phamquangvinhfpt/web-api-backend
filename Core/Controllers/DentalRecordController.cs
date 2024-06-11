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
                new Responses.Response { Status = StatusCodes.Status200OK, Message = "Success", Object = _recordService.getAllRecord() }
                );
        }
        [HttpPost("getRecord")]
        public async Task<IActionResult> GetAllRecordByID([FromBody] string id)
        {
            return StatusCode(StatusCodes.Status200OK,
                new Responses.Response { Status = StatusCodes.Status200OK, Message = "Success", Object = _recordService.GetRecordByID(id) }
                );
        }

        [HttpPost("createDentalRecord")]
        public async Task<IActionResult> CreateDentalRecord([FromBody] CreateDentalRecordRequest request)
        {
            _recordService.CreateDentalRecord(request);
            return StatusCode(StatusCodes.Status200OK,
                 new Responses.Response { Status = StatusCodes.Status200OK, Message = "Success", Object = null }
                 );
        }

    }
}
