using Solid.Aim.Contracts;

namespace Solid.Aim.Validation.User
{
    internal class MinAgeValidator : IValidator<UserDto>
    {
        private readonly int _minAge;

        public MinAgeValidator(int minAge)
        {
            _minAge = minAge;
        }

        public virtual IValidationResult Validate(UserDto entity)
        {
            if (entity == null) return new ValidationResult(false, "Missing dto");
            if (!entity.Age.HasValue) return new ValidationResult(false, "Missing age");
            if (entity.Age.Value < _minAge) return new ValidationResult(false, "Age too low");
            return ValidationResult.Valid;
        }
    }
}

