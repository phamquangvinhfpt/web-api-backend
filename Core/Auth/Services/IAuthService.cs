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

        Task<ResponseManager> LoginUser(AuthUser model);

        Task<ResponseManager> ConfirmEmail(Guid userId, string token);

        Task<ResponseManager> ForgetPassword(string email);

        Task<ResponseManager> ResetPassword(ResetPasswordModel model);

        Task<TokenModel> GenerateToken(AppUser user);

        Task<ResponseManager> UpdateRefreshToken(RefreshToken token);

        Task<RefreshToken> GetRefreshToken(string token);
    }
}