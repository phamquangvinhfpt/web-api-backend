using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ResponseManager
    {
        public bool IsSuccess { get; set; }
        public dynamic? Message { get; set; }
        public IEnumerable<dynamic>? Errors { get; set; }
    }
}