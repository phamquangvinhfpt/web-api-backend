using BusinessObject.Models;
using Core.Auth.Services;
using Core.Helpers;
using Core.Models;
using Core.Models.Clinics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Clinics;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ClinicsController : ControllerBase
    {
        private IClinicsService _clinicsService;
        private readonly ILogger<ClinicsController> _logger;
        private readonly IUriService uriService;

        public ClinicsController(IClinicsService clinicsService, ILogger<ClinicsController> logger, IUriService uriService)
        {
            _clinicsService = clinicsService;
            _logger = logger;
            this.uriService = uriService;
        }

        [HttpGet("Clinics")]
        public IActionResult GetAllClinics([FromQuery] PaginationFilter filter)
        {
            try
            {
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortBy, filter.SortOrder, filter.SearchTerm, filter.FilterBy, filter.FilterValue);

                var clinicsQuery = _clinicsService.GetAllClinics().AsQueryable();

                // Apply Search
                if (!string.IsNullOrEmpty(validFilter.SearchTerm))
                {
                    clinicsQuery = clinicsQuery.Where(c =>
                        c.Name.Contains(validFilter.SearchTerm) ||
                        c.Address.Contains(validFilter.SearchTerm));
                }

                // Apply Filter
                if (!string.IsNullOrEmpty(validFilter.FilterBy) && !string.IsNullOrEmpty(validFilter.FilterValue))
                {
                    var parameter = Expression.Parameter(typeof(Clinic), "x");
                    var property = Expression.Property(parameter, validFilter.FilterBy);
                    var constant = Expression.Constant(validFilter.FilterValue);
                    var equality = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda<Func<Clinic, bool>>(equality, parameter);

                    clinicsQuery = clinicsQuery.Where(lambda);
                }

                // Apply Sorting
                if (!string.IsNullOrEmpty(validFilter.SortBy))
                {
                    var parameter = Expression.Parameter(typeof(Clinic), "x");
                    var property = Expression.Property(parameter, validFilter.SortBy);
                    var lambda = Expression.Lambda(property, parameter);
                    var methodName = validFilter.SortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(typeof(Queryable), methodName,
                        new Type[] { typeof(Clinic), property.Type },
                        clinicsQuery.Expression, Expression.Quote(lambda));
                    clinicsQuery = clinicsQuery.Provider.CreateQuery<Clinic>(resultExpression);
                }

                // Get total count
                var totalRecords = clinicsQuery.Count();

                // Apply Pagination
                var pagedData = clinicsQuery
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();

                var pagedResponse = PaginationHelper.CreatePagedResponse(pagedData, validFilter, totalRecords, uriService, route);
                return Ok(pagedResponse);
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
        public ActionResult AddClinics([FromBody] ClinicsModel clinicsmodel)
        {
            try
            {
                var entity = new Clinic
                {
                    Name = clinicsmodel.Name,
                    OwnerID = clinicsmodel.OwnerID,
                    Address = clinicsmodel.Address,
                    Verified = false
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
        public async Task<ActionResult> UpdateClinics(Guid id, [FromBody] UpdateClinics clinicsModel)

        {
            try
            {
                Clinic clinic = new Clinic
                {
                    Id = id,
                    Name = clinicsModel.Name,
                    Address = clinicsModel.Address,
                    OwnerID = clinicsModel.OwnerID,
                    Verified = clinicsModel.Verified,
                };

                _clinicsService.UpdateClinics(clinic, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteClinics/{id}")]
        public ActionResult DeleteClinics(Guid id)
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
