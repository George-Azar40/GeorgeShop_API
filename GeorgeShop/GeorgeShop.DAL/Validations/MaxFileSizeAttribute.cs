using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Validations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSizeInMB;
        public MaxFileSizeAttribute(int maxSizeInMB)
        {
            _maxSizeInMB = maxSizeInMB;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var fileInMB = file.Length / (1024 * 1024);
                if (fileInMB > _maxSizeInMB)
                    return new ValidationResult( $"Maximum file is {_maxSizeInMB}MB" );
            }
            return ValidationResult.Success;
        }
    }
}
