using Core.Models;
using Core.Models.Personal;
using Core.Models.UserModels;

namespace Core.Services
{
    public interface IUserService
    {
        Task<ResponseManager> GetUsers();

        Task<ResponseManager> GetUserbyId(Guid id);

        Task<ResponseManager> CreateUser(RegisterUser model);

        Task<Response<UserDetailsDto>> UpdateUser(Guid id, UpdateUserRequest user);

        Task<ResponseManager> DeleteUser(Guid id);

        public bool IsExist(Guid id);
        
        Task UpdateAvatarAsync(UpdateAvatarRequest request, CancellationToken cancellationToken);

        Task<UserDetailsDto> GetAsync(Guid userId, CancellationToken cancellationToken);

        Task ChangePasswordAsync(ChangePasswordRequest request, string userId);

        Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);

        Task<bool> VerifyCurrentPassword(string userId, string password);

        Task<string> UpdateEmailAsync(UpdateEmailRequest request);

        Task<string> ResendEmailCodeConfirm(string userId, string origin);

        Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

        Task UpdatePhoneNumberAsync(UpdatePhoneNumberRequest request);

        Task<string> ResendPhoneNumberCodeConfirm(string userId);

        Task<string> ConfirmPhoneNumberAsync(string userId, string code);
    }
}