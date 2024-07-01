using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.FileStorage;

namespace Core.Infrastructure.Extensions
{
    public class MaxFileSize : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSize(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                return ValidateFile(file);
            }

            if (value is IFormFile[] files && files != null && files.Length > 0)
            {
                foreach (var f in files)
                {
                    var validationResult = ValidateFile(f);
                    if (validationResult != ValidationResult.Success)
                    {
                        return validationResult;
                    }
                }
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateFile(IFormFile file)
        {
            if (file != null && file.Length > _maxFileSize)
            {
                return new ValidationResult(GetErrorMessage(file.FileName));
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string fileName)
        {
            return $"{fileName} is too large. Maximum allowed file size is {ConvertToMb()}MB.";
        }

        private float ConvertToMb()
        {
            const float bytesInMegabyte = 1024 * 1024; // 1 MB = 1024 * 1024 bytes
            float megabytes = _maxFileSize / bytesInMegabyte;
            return (float)Math.Round(megabytes, 3);
        }
    }

    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensions(FileType extensions)
        {
            _extensions = extensions.GetExtentionList();
        }

        public AllowedExtensions(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                return ValidateFile(file);
            }

            if (value is IFormFile[] files && files != null && files.Length > 0)
            {
                foreach (var f in files)
                {
                    var validationResult = ValidateFile(f);
                    if (validationResult != ValidationResult.Success)
                    {
                        return validationResult;
                    }
                }
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateFile(IFormFile file)
        {
            if (file != null)
            {
                string extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(file.FileName));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string fileName)
        {
            return $"File {fileName} is invalid because {Path.GetExtension(fileName)} extension not allowed.";
        }
    }
}