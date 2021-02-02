using System.Linq;
using Solid.Aim.Contracts;

namespace Solid.Aim.Validation.User
{
    internal class NameValidator : IValidator<UserDto>
    {
        public IValidationResult Validate(UserDto entity)
        {
            if (entity == null) 
                return new ValidationResult(false, "Missing dto");
            
            if (string.IsNullOrWhiteSpace(entity.Name)) 
                return new ValidationResult(false, "Missing name");
            
            if (entity.Name.Any(c => !char.IsLetter(c))) 
                return new ValidationResult(false, "Name is invalid");
            
            return ValidationResult.Valid;
        }
    }
}

