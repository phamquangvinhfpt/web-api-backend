using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessObject.Models;
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

        Task<ResponseManager> AddRefreshToken(RefreshToken token);

        Task<RefreshToken> GetRefreshToken(string token);

        Task<ResponseManager> UpdateRefreshToken(RefreshToken token);

        public bool IsExist(Guid id);
    }
}