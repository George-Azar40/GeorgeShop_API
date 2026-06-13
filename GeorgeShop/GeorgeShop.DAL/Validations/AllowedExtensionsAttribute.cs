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
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        string[] _extensions = { ".jpg", ".webp" , ".png" , ".jfif" };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                //Test.png
                //Test.PNG
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult($"Allowed extensions is : {string.Join(", "  , _extensions)}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
