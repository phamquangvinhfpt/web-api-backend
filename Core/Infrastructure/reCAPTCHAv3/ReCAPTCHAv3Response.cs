using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.reCAPTCHAv3
{
    public class ReCAPTCHAv3Response
    {
        public bool success { get; set; }
        public string challengeTs { get; set; } = string.Empty;
        public string hostname { get; set; } = string.Empty;
        public double score { get; set; }
        public string action { get; set; } = string.Empty;
    }
}