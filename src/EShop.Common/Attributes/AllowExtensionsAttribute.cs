using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Attributes
{
    public class AllowExtensionsAttribute : BaseValidationAttribute, IClientModelValidator
    {
        private readonly string _displayName;
        private readonly string[] _allowExtensions;
        private readonly string[] _allowContentTypes;
        private readonly string _errorMessage;

        public AllowExtensionsAttribute(string displayName, string[] allowExtensions, string[] allowContentTypes)
        {
            _displayName = displayName;
            _errorMessage = $"فرمت های مجاز برای {displayName} ";
            _allowExtensions = allowExtensions;
            _allowContentTypes = allowContentTypes;
            foreach (var _allowExtension in _allowExtensions)
            {
                _errorMessage += $"{_allowExtension}, ";
            }
            _errorMessage = _errorMessage.Trim(' ');
            _errorMessage = _errorMessage.Trim(',');
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > 0)
            {
                if (!_allowContentTypes.Contains(file.ContentType))
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
            MergeAttribute(context.Attributes, "data-val-whitelistextensions", string.Join(",", _allowContentTypes));
        }
    }
}
