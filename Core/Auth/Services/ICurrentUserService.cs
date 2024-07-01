using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Auth.Services
{
    public interface ICurrentUserService
    {
        string GetCurrentUserId();
    }
}