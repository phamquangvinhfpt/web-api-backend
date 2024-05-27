using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessObject.Models;
using Core.Models;
using Core.Models.AuthModel;
using Core.Models.AuthModels;
using Core.Models.UserModels;

namespace Core.Services
{
    public interface IAuthService
    {
        Task<ResponseManager> RegisterUser(RegisterUser model);

        Task<ResponseManager> LoginUser(AuthUser model, string deviceId, bool isMobile);

        Task<ResponseManager> ConfirmEmail(Guid userId, string token);

        Task<ResponseManager> ForgetPassword(string email);

        Task<ResponseManager> ResetPassword(ResetPasswordModel model);
    }
}