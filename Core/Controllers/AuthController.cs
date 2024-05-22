using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BussinessObject.Models;
using Core.Helpers;
using Core.Models;
using Core.Models.AuthModel;
using Core.Models.AuthModels;
using Core.Models.UserModels;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private readonly IUserService _user;

        public AuthController(IAuthService AuthService, IMapper mapper, IMailService mailService, IConfiguration configuration, IUserService userService)
        {
            _auth = AuthService;
            _mapper = mapper;
            _mailService = mailService;
            _config = configuration;
            _user = userService;
        }

        // api/auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.RegisterUser(model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code: 200 

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400
        }

        // api/auth/Authenticate
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.LoginUser(model);

                if (result.IsSuccess)
                {
                    return Ok(new ResponseManager
                    {
                        IsSuccess = true,
                        Message = new
                        {
                            content = "Login Successful",
                            token = result.Message,
                            expires = DateTime.Now.AddHours(24),
                        },
                    });
                }
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        // api/auth/RefreshToken
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenValidateParam = new TokenValidationParameters
            {
                SaveSigninToken = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = false, // we don't care about the token's expiration
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
            };

            try
            {

                // Check authen valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken,
                    tokenValidateParam, out var validatedToken);

                // Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return BadRequest(new ResponseManager
                        {
                            IsSuccess = false,
                            Message = "Invalid Token",
                        });
                    }

                    // Check accessToken is expired?
                    var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                    var expiryDate = ConvertUnixTimeStampToDateTime(utcExpiryDate);

                    if (expiryDate > DateTime.UtcNow)
                    {
                        return BadRequest(new ResponseManager
                        {
                            IsSuccess = false,
                            Message = "This token hasn't expired yet",
                        });
                    }

                    // Check refreshToken exists in DB
                    var refreshToken = await _auth.GetRefreshToken(model.RefreshToken);
                    if (refreshToken == null)
                    {
                        return BadRequest(new ResponseManager
                        {
                            IsSuccess = false,
                            Message = "This refresh token does not exist",
                        });
                    }

                    // Check refreshToken is used/revoked?
                    if (refreshToken.IsUsed)
                    {
                        return BadRequest(new ResponseManager
                        {
                            IsSuccess = false,
                            Message = "This refresh token has been used",
                        });
                    }

                    if (refreshToken.IsRevoked)
                    {
                        return BadRequest(new ResponseManager
                        {
                            IsSuccess = false,
                            Message = "This refresh token has been revoked",
                        });
                    }

                    // Check AccessToken Id == JwtId in RefreshToken
                    var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                    if (refreshToken.JwtId != jti)
                    {
                        return BadRequest(new ResponseManager
                        {
                            IsSuccess = false,
                            Message = "This refresh token does not match this JWT",
                        });
                    }

                    // Update token is used
                    refreshToken.IsUsed = true;
                    refreshToken.IsRevoked = true;

                    // Update token in DB
                    var user = await _user.GetUserbyId(refreshToken.UserId);
                    var token = await _auth.GenerateToken(user.Message as AppUser);

                    return Ok(new ResponseManager
                    {
                        IsSuccess = true,
                        Message = new
                        {
                            content = "Token Refreshed Successfully",
                            token = token,
                            expires = DateTime.Now.AddHours(24),
                        },
                    });
                }

                return BadRequest(new ResponseManager
                {
                    IsSuccess = false,
                    Message = "Invalid Token",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseManager
                {
                    IsSuccess = false,
                    Message = ex.Message,
                });
            }
        }

        private DateTime ConvertUnixTimeStampToDateTime(long utcExpiryDate)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(utcExpiryDate).ToUniversalTime();
        }

        // /api/auth/ConfirmEmail?userid&token
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId.ToString()) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _auth.ConfirmEmail(userId, token);

            if (result.IsSuccess)
            {
                return Content("Email Verified Successfully!");
            }

            return BadRequest(result);
        }

        // api/auth/ForgetPassword
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _auth.ForgetPassword(email);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        // api/auth/ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.ResetPassword(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
    }
}