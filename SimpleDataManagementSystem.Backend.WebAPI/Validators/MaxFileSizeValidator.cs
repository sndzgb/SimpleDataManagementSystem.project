using System.ComponentModel.DataAnnotations;

namespace SimpleDataManagementSystem.Backend.WebAPI.Validators
{
    // TODO move to shared project
    public class MaxFileSizeValidator : ValidationAttribute
    {
        private readonly int _maxFileSize;


        public MaxFileSizeValidator(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {_maxFileSize} bytes.";
        }
    }
}
