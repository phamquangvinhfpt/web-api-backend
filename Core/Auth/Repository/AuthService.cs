using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;
using Core.Helpers;
using Core.Models;
using Core.Models.AuthModel;
using Core.Models.AuthModels;
using Core.Models.UserModels;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;

namespace Core.Repository
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private IMailService _mailService;
        private readonly IUserService _useService;

        public AuthService(IConfiguration config,
                            UserManager<AppUser> userManager,
                            IMailService mailService,
                            IUserService userService,
                            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = mailService;
            _useService = userService;
        }

        //Register User
        public async Task<ResponseManager> RegisterUser(RegisterUser model)
        {
        
            var identityUser = new AppUser
            {
                Email = model.Email,
                UserName = model.Username,
                FullName = model.FullName,
            };

            //user creation
            var createdUser = await _useService.CreateUser(model);

            //Mail Sending
            if (createdUser.IsSuccess != false)
            {
                // tìm User trong cơ sở dữ liệu sau khi gọi CreateUser.
                identityUser = await _userManager.FindByEmailAsync(model.Email);
                string confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                var emailToken = Uri.EscapeDataString(confirmEmailToken);
                var encodedEmailToken = Encoding.UTF8.GetBytes(emailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                var confirmUser = await _userManager.FindByEmailAsync(identityUser.Email);

                string url = $"{_config["AppUrl"]}/api/auth/ConfirmEmail?userid={confirmUser.Id}&token={validEmailToken}";

                var mailContent = new MailRequest
                {
                    ToEmail = identityUser.Email,
                    Subject = "Confirm your email",
                    Body = $"<h1>Welcome to our website</h1>" + $"<p>Hi {identityUser.UserName} !, Please confirm your email by <a href='{url}'>Clicking here</a></p><br><strong>Email Confirmation token for ID '"+confirmUser.Id+"' : <code>"+validEmailToken + "</code></strong>",
                };

                await _mailService.SendEmailAsync(mailContent);

                return new ResponseManager
                {
                    IsSuccess = true,
                    Message = "User created successfully! Please confirm the your Email!",
                };
            } 

            return new ResponseManager
            {
                IsSuccess = false,
                Message = "User Email Already Registered, Try Login(/api/auth/Authenticate)",
            };

        }

        //Login User
        public async Task<ResponseManager> LoginUser(AuthUser model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user.Email != model.Email || user.UserName != model.UserName)
            {
                return new ResponseManager
                {
                    Message = "There is no user with that Email address / Username! ",
                    IsSuccess = false,
                };
            }
            else
            {
                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!result)
                {
                    return new ResponseManager
                    {
                        Message = "Invalid password",
                        IsSuccess = false,
                    };
                }

                var userRole = new List<string>(await _userManager.GetRolesAsync(user));
                //Generate Token JWT
                var Token = await GenerateToken(user);

                return new ResponseManager
                {   
                    Message = Token,
                    IsSuccess = true,
                };
            }

        }

        //ConfirmEmail
        public async Task<ResponseManager> ConfirmEmail(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ResponseManager
                {
                    IsSuccess = false,
                    Message = "User not found",
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            // Convert back to string
            var normalToken = Encoding.UTF8.GetString(decodedToken);
            // Unescape to get the original email token
            normalToken = Uri.UnescapeDataString(normalToken);

            // check if the token is valid
            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return new ResponseManager
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true,
                };
            }

            return new ResponseManager
            {
                IsSuccess = false,
                Message = "Email did not confirm",
                //Errors = result.Errors.ToArray()
            };
        }

        //Forget Password
        public async Task<ResponseManager> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ResponseManager
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            string pass = "Tester@123";
            string url = $"{_config["AppUrl"]}/api/Auth/ResetPassword?Email={email}&Token={validToken}&NewPassword={pass}&ConfirmPassword={pass}";
            
            var mailContent = new MailRequest
            {
                ToEmail = email,
                Subject = "Reset Password",
                Body = "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password, <br><br> 1. Copy the Link :  <a href='{url}'>{url}</a><br><br> 2. Navigate to API Testing Tools(Postman)<br><br> 3. Set the Method to 'POST' <br><br> 4. Make a Request <br><br> or Use SWAGGER <br><br> <strong>Reset Token : {validToken}</strong></p>",
            };

            await _mailService.SendEmailAsync(mailContent);

            return new ResponseManager
            {
                IsSuccess = true,
                Message = "Reset password URL has been sent to the email successfully!",
            };
        }

        //Reset Password
        public async Task<ResponseManager> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new ResponseManager
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new ResponseManager
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new ResponseManager
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new ResponseManager
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        //Token Genereator
        public async Task<TokenModel> GenerateToken(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();

            var userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

            var roleClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FullName),
            }.Union(userClaims).Union(roleClaims).Union(userRoles);
            var tokenClaims = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenClaims);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                JwtId = tokenClaims.Id,
                User = user,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(1),
            };
            await _useService.AddRefreshToken(refreshTokenEntity);
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // GetRefreshToken
        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            return await _useService.GetRefreshToken(token);
        }

        // UpdateRefreshToken
        public async Task<ResponseManager> UpdateRefreshToken(RefreshToken token)
        {
            return await _useService.UpdateRefreshToken(token);
        }
    }
}