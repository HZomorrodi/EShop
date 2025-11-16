using EShop.Common.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common
{
    public class IsImageAttribute : BaseValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage;
        private readonly string[] _allowExtensions =
        [
            "image/png",
            "image/jpeg",
            "image/bmp",
            "image/gif"
        ];
        public IsImageAttribute(string displayName)
        {
            _errorMessage = $"{displayName} حتما باید عکس باشد";
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > 0)
            {
                if (!_allowExtensions.Contains(file.ContentType))
                {
                    return new ValidationResult(_errorMessage);
                }
                try
                {
                    var img = Image.FromStream(file.OpenReadStream());
                }
                catch
                {
                    return new ValidationResult(_errorMessage);
                }
            }
            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-allowExtensions", _errorMessage);
            MergeAttribute(context.Attributes, "data-val-whitelistextensions", string.Join(",", _allowExtensions));
        }
    }
}
