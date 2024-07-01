using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Infrastructure.SpeedSMS
{
    public interface ISpeedSMSService
    {
        public string sendSMS(string[] phones, String content, int type);
    }
}