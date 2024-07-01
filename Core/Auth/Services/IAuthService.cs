using Core.Models;
using Core.Models.AuthModels;
using Core.Models.UserModels;

namespace Core.Services
{
    public interface IAuthService
    {
        Task<ResponseManager> RegisterUser(RegisterUser model, string origin);

        Task<ResponseManager> LoginUser(AuthUser model, string deviceId, bool isMobile, string ipAddress);

        Task<ResponseManager> LogoutUser(string accessToken);

        Task<ResponseManager> ConfirmEmail(Guid userId, string token);

        Task<ResponseManager> ForgetPassword(string email, string captchaToken, string origin);

        Task<ResponseManager> ResetPassword(ResetPasswordModel model);
    }
}