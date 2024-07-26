using BusinessObject.Enums;
using BusinessObject.Models;
using Core.Auth.Permissions;
using Core.Auth.Services;
using Core.Helpers;
using Core.Infrastructure.Notifications;
using Core.Models;
using Core.Responses;
using Core.Services;
using DAO.Data;
using DAO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Appoinmets;
using Services.RecordServices;
using System.Linq.Expressions;
using System.Security.Claims;
using Action = Core.Auth.Permissions.Action;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class DentalRecordController : ControllerBase
    {
        private readonly IDentalRecordService _recordService;
        private UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private IMailService _mailService;
        private readonly ILogger<DentalRecordController> _logger;
        private readonly IUriService uriService;
        private readonly IAppoinmentService appoinmentService;
        private readonly ICurrentUserService currentUserService;
        private readonly INotificationService _notificationService;
        public DentalRecordController(INotificationService notificationService, UserManager<AppUser> userManager, IMailService mailService, ILogger<DentalRecordController> logger, IUriService uriService, ICurrentUserService currentUserService, RoleManager<IdentityRole<Guid>> roleManager, IDentalRecordService recordService)
        {
            _notificationService = notificationService;
            _recordService = recordService;
            _userManager = userManager;
            _mailService = mailService;
            _logger = logger;
            this.uriService = uriService;
            appoinmentService = new AppoinmentService();
            this.currentUserService = currentUserService;
            _roleManager = roleManager;
        }

        [HttpGet("getRecords")]
        [MustHavePermission(Action.View, Resource.DentalRecords)]
        public async Task<IActionResult> GetAllRecords([FromQuery] PaginationFilter filter)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(currentUserService.GetCurrentUserId());
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortBy, filter.SortOrder, filter.SearchTerm, filter.FilterBy, filter.FilterValue);
                IQueryable<DentalRecordData> dentals = Enumerable.Empty<DentalRecordData>().AsQueryable();
                foreach (var role in roles)
                {
                    var roleEntity = await _roleManager.FindByNameAsync(role).ConfigureAwait(false);
                    if (roleEntity != null)
                    {
                        var rolename = await _roleManager.GetRoleNameAsync(roleEntity).ConfigureAwait(false);
                        if (rolename.Equals("SuperAdmin"))
                        {
                            dentals = _recordService.getAllRecord().AsQueryable();
                        }
                        else if (rolename.Equals("ClinicOwner"))
                        {
                            dentals = _recordService.GetRecordByClinicOwner(user.Id).AsQueryable();
                        }
                        else if (rolename.Equals("Dentist"))
                        {
                            dentals = _recordService.GetRecordByDentist(user.Id).AsQueryable();
                        }
                        else if (rolename.Equals("Customer"))
                        {
                            dentals = _recordService.GetRecordByCustomer(user.Id).AsQueryable();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(validFilter.SearchTerm))
                {
                    dentals = dentals.Where(c =>
                        c.dentist.ToString().ToLower().Contains(validFilter.SearchTerm.ToLower()) ||
                        c.patient.ToString().ToLower().Contains(validFilter.SearchTerm.ToLower())
                        );
                }
                if (!string.IsNullOrEmpty(validFilter.FilterBy) && !string.IsNullOrEmpty(validFilter.FilterValue))
                {
                    var parameter = Expression.Parameter(typeof(DentalRecordData), "x");
                    var property = Expression.Property(parameter, validFilter.FilterBy);
                    var constant = Expression.Constant(validFilter.FilterValue);
                    var equality = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda<Func<DentalRecordData, bool>>(equality, parameter);

                    dentals = dentals.Where(lambda);
                }
                if (!string.IsNullOrEmpty(validFilter.SortBy))
                {
                    var parameter = Expression.Parameter(typeof(DentalRecordData), "x");
                    var property = Expression.Property(parameter, validFilter.SortBy);
                    var lambda = Expression.Lambda(property, parameter);
                    var methodName = validFilter.SortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(typeof(Queryable), methodName,
                        new Type[] { typeof(DentalRecordData), property.Type },
                        dentals.Expression, Expression.Quote(lambda));
                    dentals = dentals.Provider.CreateQuery<DentalRecordData>(resultExpression);
                }
                var totalRecords = dentals.Count();
                var pagedData = dentals
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToList();

                var pagedResponse = PaginationHelper.CreatePagedResponse(pagedData, validFilter, totalRecords, uriService, route);
                return StatusCode(StatusCodes.Status200OK,
                new ResponseManager
                {
                    IsSuccess = true,
                    Message = new List<dynamic> { pagedResponse },
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseManager
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Errors = null
                });
            }
        }

        [HttpPost("getAppointmentForCreate")]
        [MustHavePermission(Action.View, Resource.Appointments)]
        public IActionResult GetAppointmentForCreateDentalByID([FromBody] GetByIDRequest request)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK,
                 new Response
                 {
                     Message = "Success",
                     Object = appoinmentService.GetAppointmentForCreateDentalByID(request.id),
                     Status = StatusCodes.Status200OK,
                 }
                 );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseManager
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Errors = null
                });
            }
        }

        [HttpPost("getRecord")]
        [MustHavePermission(Action.View, Resource.DentalRecords)]
        public async Task<IActionResult> GetAllRecordByID([FromBody] GetByIDRequest request)
        {
            try
            {
                var dental = _recordService.GetRecordByID(request.id);
                var dentist = await _userManager.FindByIdAsync(dental.Appointment.DentistID.ToString());

                var patient = await _userManager.FindByIdAsync(dental.Appointment.PatientID.ToString());
                dental.Appointment.Dentist = dentist;
                dental.Appointment.Patient = patient;
                return StatusCode(StatusCodes.Status200OK,
                 new Response
                 {
                     Message = "Success",
                     Object = dental,
                     Status = StatusCodes.Status200OK,
                 }
                 );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseManager
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Errors = null
                });
            }
        }

        [HttpPost("createDentalRecord")]
        [MustHavePermission(Action.Create, Resource.DentalRecords)]
        public async Task<IActionResult> CreateDentalRecord([FromBody] CreateDentalRecordRequest request)
        {
            try
            {
                var appointment = _recordService.CreateDentalRecord(request, Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier).Value));

                var dentist = await _userManager.FindByIdAsync(appointment.DentistID.ToString());

                var patient = await _userManager.FindByIdAsync(appointment.PatientID.ToString());

                var dental = _recordService.GetByAppointment(appointment.Id);
                var flu = dental.FollowUpAppointments[dental.FollowUpAppointments.Count - 1];
                var appointmentDate = flu.ScheduledDate.ToString("dd-MM-yyyy");
                var mailContent = new MailRequest
                {
                    ToEmail = patient.Email,
                    Subject = "Your Dental Record",
                    Body = $"<p>Hi {patient.FullName},</p>"
                        + $"<p>Thank you for your visit and for trusting our dental services.</p>"
                        + $"<p>We would like to see you at {appointmentDate} Because {flu.Reason}</p>"
                        + $"<p>Please let us know your availability, and we will be happy to arrange a convenient time for your next visit.</p>"
                        + $"<p>We appreciate your ongoing trust in our dental practice and look forward to seeing you again soon.</p>"
                        + $"<p>Best regards,</p>"
                        + $"<p>{dentist.FullName}</p>"
                };
                await _mailService.SendEmailAsync(mailContent);
                CancellationToken cancellationToken = new CancellationToken();
                var notification = new BasicNotification
                {
                    Message = "Your medical record has been created successfully",
                    Label = BasicNotification.LabelType.Success,
                    Title = "Dental Record Created Successfully",
                    Url = "/test"
                };
                await _notificationService.SendNotificationToUser(appointment.PatientID.ToString(), notification, null, cancellationToken);
                return StatusCode(StatusCodes.Status200OK,
                     new ResponseManager
                     {
                         IsSuccess = true,
                         Message = "Create record success",
                         Errors = null
                     });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseManager
                        {
                            IsSuccess = false,
                            Message = ex.Message,
                            Errors = null
                        });
            }
        }

    }
}
