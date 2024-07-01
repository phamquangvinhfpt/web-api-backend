using AutoMapper;
using Core.Auth;
using Core.Auth.Services;
using Core.Models.Personal;
using Core.Models.UserModels;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public UserController(IUserService userService, IMapper mapper, IAuthService authService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _mapper = mapper;
            _auth = authService;
            _currentUser = currentUserService;
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
    }
}