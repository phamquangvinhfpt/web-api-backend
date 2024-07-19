using AutoMapper;
using BusinessObject.Models;
using Core.Auth.Services;
using Core.Infrastructure.Validator;
using Core.Models;
using Core.Models.Auditing;
using Core.Models.Personal;
using Core.Services;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace Core.Controllers
{
    [ApiController]
    [Route("api/personal")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PersonalController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizerFactory _localizerFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<PersonalController> _logger;
        private readonly IAuditService _auditService;
        private readonly UserManager<AppUser> _userManager;

        public PersonalController(IUserService userService, IMapper mapper, IStringLocalizerFactory localizerFactory,
                                  ICurrentUserService currentUserService, ILogger<PersonalController> logger,
                                  IAuditService auditService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _localizerFactory = localizerFactory;
            _currentUserService = currentUserService;
            _logger = logger;
            _auditService = auditService;
            _userManager = userManager;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDetailsDto>> GetProfileAsync(CancellationToken cancellationToken)
        {
            try
            {
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                var user = await _userService.GetAsync(Guid.Parse(userId), cancellationToken);
                if (user == null)
                {
                    return Unauthorized();
                }
                return Ok(_mapper.Map<UserDetailsDto>(user));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting user profile");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("profile")]
        public async Task<Response<UserDetailsDto>> UpdateProfileAsync(UpdateUserRequest request)
        {
            try
            {
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                if (userId is null)
                {
                    throw new UnauthorizedAccessException();
                }
                var baseLocalizer = _localizerFactory.Create(typeof(UpdateUserRequestValidator));
                var localizer = new StringLocalizerWrapper<UpdateUserRequestValidator>(baseLocalizer);
                var validator = new UpdateUserRequestValidator(localizer);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                    throw new ValidationException(string.Join(", ", errors));
                }

                return await _userService.UpdateUser(Guid.Parse(userId), request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating user profile");
                throw new Exception(e.Message);
            }
        }

        [HttpPut("update-password")]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            try
            {
                if (HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) is not { } userId || string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
                var baseLocalizer = _localizerFactory.Create(typeof(ChangePasswordRequestValidator));
                var localizer = new StringLocalizerWrapper<ChangePasswordRequestValidator>(baseLocalizer);
                var validator = new ChangePasswordRequestValidator(localizer);
                var validationResult = await validator.ValidateAsync(model);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                    throw new ValidationException(string.Join(", ", errors));
                }
                await _userService.ChangePasswordAsync(model, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update-email")]
        public async Task<string> UpdateEmailAsync(UpdateEmailRequest request)
        {
            try
            {
                request.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var baseLocalizer = _localizerFactory.Create(typeof(UpdateUserRequestValidator));
                var localizer = new StringLocalizerWrapper<UpdateUserRequestValidator>(baseLocalizer);
                var validator = new UpdateEmailRequestValidator(_userService, localizer, _currentUserService);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                    throw new ValidationException(string.Join(", ", errors));
                }

                if (HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) is not { } userId || string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException();
                }

                request.UserId = userId;
                return await _userService.UpdateEmailAsync(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut("update-phone")]
        public async Task<string> UpdatePhoneNumberAsync(UpdatePhoneNumberRequest request)
        {
            try
            {
                request.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var baseLocalizer = _localizerFactory.Create(typeof(UpdatePhoneNumberRequestValidator));
                var localizer = new StringLocalizerWrapper<UpdatePhoneNumberRequestValidator>(baseLocalizer);
                var validator = new UpdatePhoneNumberRequestValidator(_userService, localizer, _currentUserService);
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                    throw new ValidationException(string.Join(", ", errors));
                }
                if (HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) is not { } userId || string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException();
                }
                request.UserId = userId;
                await _userService.UpdatePhoneNumberAsync(request);
                return "Phone number updated successfully";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("resend-email-confirm")]
        public Task<string> ResendEmailConfirmAsync()
        {
            var request = new ResendEmailConfirmRequest { Origin = GetOriginFromRequest() };

            if (HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) is not { } userId || string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException();
            }

            return _userService.ResendEmailCodeConfirm(userId, request.Origin);
        }

        [HttpPut("update-avatar")]
        public async Task<IActionResult> UpdateAvatarAsync([FromForm] UpdateAvatarRequest request)
        {
            try
            {
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                request.UserId = Guid.Parse(userId);
                await _userService.UpdateAvatarAsync(request, HttpContext.RequestAborted);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("logs")]
        public Task<PaginationResponse<AuditDto>> GetLogsAsync(GetAuditLogsRequestDto requestDto)
        {
            try
            {
                var users = _userManager.Users.ToList();
                List<Guid> userIds = users.Select(u => u.Id).ToList();

                var request = new GetMyAuditLogsRequest
                {
                    UserId = userIds,
                    PageNumber = requestDto.PageNumber,
                    PageSize = requestDto.PageSize,
                    Action = requestDto.Action,
                    Resource = requestDto.Resource
                };
                return _auditService.GetUserTrailsAsync(request);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("logs/resource-type")]
        public async Task<List<string>> GetResourceNamesAsync()
        {
            return await _auditService.GetResourceName();
        }

        private string GetOriginFromRequest()
        {
            if (Request.Headers.TryGetValue("x-from-host", out var values))
            {
                return $"{Request.Scheme}://{values.First()}";
            }

            return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
        }
    }
}