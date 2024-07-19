using AutoMapper;
using Core.Auth.Permissions;
using Core.Auth.Services;
using Core.Helpers;
using Core.Models;
using Core.Models.Personal;
using Core.Models.UserModels;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Action = Core.Auth.Permissions.Action;
namespace Core.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _auth;
        private readonly ICurrentUserService _currentUser;
        private readonly IUriService _uriService;

        public UserController(IUserService userService, IMapper mapper, IAuthService authService, ICurrentUserService currentUserService, IUriService uriService)
        {
            _userService = userService;
            _mapper = mapper;
            _auth = authService;
            _currentUser = currentUserService;
            _uriService = uriService;
        }

        [HttpGet("resend-phone-number-code")]
        public async Task<string> ResendPhoneNumberCodeConfirmAsync()
        {
            var userId = _currentUser.GetCurrentUserId();
            return await _userService.ResendPhoneNumberCodeConfirm(userId);
        }

        [HttpGet("confirm-phone-number")]
        public Task<string> ConfirmPhoneNumberAsync([FromQuery] string code)
        {
            if (_currentUser.GetCurrentUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException();
            }

            return _userService.ConfirmPhoneNumberAsync(userId, code);
        }

        [HttpPost("get-all-users")]
        [MustHavePermission(Action.View, Resource.Users)]
        public async Task<PagedResponse<List<UserDetailsDto>>> GetAllUsers(GetListUsersRequest request)
        {
            try
            {
                var route = Request.Path.Value;
                var users = await _userService.GetUsers();
                var validFilter = new PaginationFilter(request.PageNumber, request.PageSize, request.SortBy, request.SortOrder, request.SearchTerm, request.FilterBy, request.FilterValue);

                if (!string.IsNullOrEmpty(validFilter.SearchTerm))
                {
                    users = users.Where(u =>
                        u.FullName.Contains(validFilter.SearchTerm) ||
                        u.Email.Contains(validFilter.SearchTerm) ||
                        u.PhoneNumber.Contains(validFilter.SearchTerm)).ToList();
                }

                if (!string.IsNullOrEmpty(validFilter.FilterBy) && !string.IsNullOrEmpty(validFilter.FilterValue))
                {
                    var parameter = Expression.Parameter(typeof(UserDetailsDto), "x");
                    var property = Expression.Property(parameter, validFilter.FilterBy);
                    var constant = Expression.Constant(validFilter.FilterValue);
                    var equality = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda<Func<UserDetailsDto, bool>>(equality, parameter);
                    users = users.Where(lambda.Compile()).ToList();
                }

                if (!string.IsNullOrEmpty(validFilter.SortBy))
                {
                    var parameter = Expression.Parameter(typeof(UserDetailsDto), "x");
                    var property = Expression.Property(parameter, validFilter.SortBy);
                    var lambda = Expression.Lambda(property, parameter);
                    var methodName = validFilter.SortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(typeof(Queryable), methodName,
                        new Type[] { typeof(UserDetailsDto), property.Type },
                        users.AsQueryable().Expression, Expression.Quote(lambda));
                    users = users.AsQueryable().Provider.CreateQuery<UserDetailsDto>(resultExpression).ToList();
                }

                var totalRecords = users.Count;
                var pagedData = users.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
                var pagedResponse = PaginationHelper.CreatePagedResponse(pagedData, request, totalRecords, _uriService, route);
                return pagedResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("delete-user-account")]
        [MustHavePermission(Action.Delete, Resource.Users)]
        public async Task<Response<UserDetailsDto>> DeleteUserAsync([FromQuery] Guid userId)
        {
            try
            {
                if (!_userService.IsExist(userId))
                {
                    return new Response<UserDetailsDto>
                    {
                        Message = "User not found!",
                        Succeeded = false
                    };
                }

                return await _userService.DeleteUser(userId);
            }
            catch (Exception e)
            {
                return new Response<UserDetailsDto>
                {
                    Message = e.Message,
                    Succeeded = false
                };
            }
        }
    }
}