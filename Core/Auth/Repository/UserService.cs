using System.Data;
using System.Globalization;
using System.Text;
using BusinessObject.Data;
using BusinessObject.Models;
using Core.Models;
using Core.Models.UserModels;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMailService _mailService;

        public UserService(AppDbContext context,
                           UserManager<AppUser> userManager,
                           RoleManager<IdentityRole<Guid>> roleManager,
                           IMailService mailService,
                           IConfiguration config)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = mailService;
        }

        //All Users
        public async Task<ResponseManager> GetUsers()
        {
            var userAll = await _userManager.Users.ToListAsync();
            return new ResponseManager
            {
                IsSuccess = true,
                Message = userAll,
            };
        }

        //GetUserByID
        public async Task<ResponseManager> GetUserbyId(Guid id)
        {
            var userById = await _userManager.FindByIdAsync(id.ToString());
            return new ResponseManager
            {
                IsSuccess = true,
                Message = userById,
            };
        }

        //Create User
        public async Task<ResponseManager> CreateUser(RegisterUser model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Data provided is NULL");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new ResponseManager
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };
            }

            //Is User Exist
            var userFound = await _userManager.FindByEmailAsync(model.Email);

            //-Not Exists
            if (userFound == null)
            {
                var identityUser = new AppUser
                {
                    Email = model.Email,
                    UserName = RemoveUnicode(model.FullName.Replace(" ", "")),
                    FullName = model.FullName,
                    NormalizedUserName = RemoveUnicode(model.FullName.Replace(" ", "")).Normalize(),
                    NormalizedEmail = model.Email.Normalize(),
                    PhoneNumber = model.PhoneNumber,
                };

                try
                {
                    var result = await _userManager.CreateAsync(identityUser, model.Password);

                    //Setting Roles
                    if (model.Role != null)
                    {
                        var roleCheck = await _roleManager.RoleExistsAsync(model.Role);
                        if (roleCheck != true)
                        {
                            await _userManager.AddToRoleAsync(identityUser, Convert.ToString("Guest"));
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(identityUser, Convert.ToString(model.Role));
                        }

                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(identityUser, Convert.ToString("Guest"));
                    }

                    return new ResponseManager
                    {
                        IsSuccess = true,
                        Message = result,
                    };
                }

                catch (DBConcurrencyException ex)
                {
                    return new ResponseManager()
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                        Errors = new List<string> { ex.Message },
                    };
                }
            }
            //- User Exist
            return new ResponseManager
            {
                IsSuccess = false,
                Message = "User already exist!",
            };

        }

        //Update User
        public async Task<ResponseManager> UpdateUser(Guid id, UpdateUser user)
        {
            if (user != null)
            {
                var findUser = await _userManager.FindByIdAsync(id.ToString());
                if (findUser != null)
                {

                    try
                    {
                        /*context.Users.Add(findUser.)*/
                        var updateUser = new AppUser
                        {
                            Id= id,
                            UserName = user.Username,
                            FullName = user.FullName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNo,
                        };
                        var up =  _userManager.UpdateAsync(updateUser);
                        var updatedUser = await _userManager.FindByIdAsync(updateUser.Id.ToString());
                        var updatedUserResponse = new AppUser
                        {
                            Id = id,
                            UserName = updatedUser.UserName,
                            FullName = updatedUser.FullName,
                            NormalizedUserName = updatedUser.Email,
                            Email = updatedUser.Email,
                            NormalizedEmail = updatedUser.Email,
                            PhoneNumber = updatedUser.PhoneNumber,

                        };
                        return new ResponseManager
                        {
                            IsSuccess = true,
                            Message = updatedUserResponse,
                        };

                    }
                    catch (Exception ex)
                    {

                        return new ResponseManager
                        {
                            IsSuccess = false,
                            Message = ex.Message,
                        };
                    }
                }
                return new ResponseManager()
                {
                    IsSuccess = false,
                    Message = "User not found!",
                };

            }
            return new ResponseManager()
            {
                IsSuccess = false,
                Message = "updating property should not null!",
            };

        }

        //Delete User
        public async Task<ResponseManager> DeleteUser(Guid id)
        {
            AppUser user = await _userManager.FindByIdAsync(id.ToString());
            try
            {
                await _userManager.DeleteAsync(user);

                return new ResponseManager
                {
                    IsSuccess = true,
                    Message = " User " + id + " removed successfully!",
                };
            }
            catch
            {
                throw;
            }
            return null;

        }

        //User Exist
        public bool IsExist(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // Additional Creating Password Hash
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // RemoveUnicode
        public static string RemoveUnicode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }
    }
}