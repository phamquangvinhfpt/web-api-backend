using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.FileStorage;

namespace Core.Models.Personal
{
    public class UpdateAvatarRequest
    {
        public Guid? UserId { get; set; }
        [AllowedExtensions(FileType.Image)]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile? Image { get; set; }
        public bool DeleteCurrentImage { get; set; } = false;
    }
}