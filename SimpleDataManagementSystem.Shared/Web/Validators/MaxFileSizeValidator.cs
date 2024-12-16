using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Web.Validators
{
    /// <summary>
    /// Validates uploaded files' size in bytes.
    /// </summary>
    public class MaxFileSizeValidator : ValidationAttribute
    {
        private readonly int _maxFileSizeBytes;
        private readonly decimal _maxFileSizeMegaBytes;


        public MaxFileSizeValidator(int maxFileSizeBytes)
        {
            _maxFileSizeBytes = maxFileSizeBytes;
            _maxFileSizeMegaBytes = _maxFileSizeBytes / (1024 * 1024);
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > _maxFileSizeBytes)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {_maxFileSizeBytes} bytes ({_maxFileSizeMegaBytes} MB).";
        }
    }
}
