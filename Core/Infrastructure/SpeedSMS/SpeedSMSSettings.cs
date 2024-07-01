using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.SpeedSMS
{
    public class SpeedSMSSettings
    {
        public string? RootUrl { get; set; }
        public string? AccessToken { get; set; }
        public string? Sender { get; set; }
    }
}