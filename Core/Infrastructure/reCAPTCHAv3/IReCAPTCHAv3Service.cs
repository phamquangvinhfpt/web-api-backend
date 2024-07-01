using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.reCAPTCHAv3
{
    public interface IReCAPTCHAv3Service
    {
        Task<ReCAPTCHAv3Response> Verify(string token);
    }
}