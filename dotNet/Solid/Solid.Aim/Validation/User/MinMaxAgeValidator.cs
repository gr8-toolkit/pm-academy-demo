using Solid.Aim.Contracts;

namespace Solid.Aim.Validation.User
{
    internal class MinMaxAgeValidator : MinAgeValidator
    {
        private readonly int _maxAge;

        public MinMaxAgeValidator(int minAge, int maxAge) : base(minAge)
        {
            _maxAge = maxAge;
        }

        public override IValidationResult Validate(UserDto entity)
        {
            var result = base.Validate(entity);
            if (!result.IsValid) return result;
            if ((entity.Age ?? 0) >= _maxAge) return new MinMaxAgeValidatorResult(false, "Age too high");
            
            return ValidationResult.Valid;
        }
    }
}
