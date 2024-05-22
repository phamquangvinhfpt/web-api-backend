using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models.UserModels
{
    public class UserResponseManager 
    {
        public bool IsSuccess { get; set; }
        public dynamic?  Message { get; set; }
        public IEnumerable<dynamic>? Errors { get; set; }
    }
}