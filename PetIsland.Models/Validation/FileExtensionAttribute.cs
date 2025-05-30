﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PetIsland.Models.Validation;

public class FileExtensionAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName); //123.jpg
            string[] extensions = { "jpg", "png", "jpeg" };

            bool result = extensions.Any(x => extension.EndsWith(x));
            if (!result)
            {
                return new ValidationResult(ErrorMessage = "Chỉ chấp nhận ảnh có đuôi jpg, jpeg, png");
            }

        }
        return ValidationResult.Success!;
    }
}
