using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models.Personal
{
    public class RegisterUserEmailModel
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Url { get; set; } = default!;
    }
}