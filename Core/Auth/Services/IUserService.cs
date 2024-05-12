using Core.Models;
using Core.Models.UserModels;

namespace Core.Services
{
    public interface IUserService
    {
        Task<ResponseManager> GetUsers();

        Task<ResponseManager> GetUserbyId(Guid id);

        Task<ResponseManager> CreateUser(RegisterUser model);

        Task<ResponseManager> UpdateUser(Guid id, UpdateUser user);

        Task<ResponseManager> DeleteUser(Guid id);

        public bool IsExist(Guid id);
    }
}