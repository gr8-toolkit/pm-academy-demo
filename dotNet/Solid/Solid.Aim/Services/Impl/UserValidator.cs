using System.Collections.Generic;
using System.Linq;
using Solid.Aim.Contracts;
using Solid.Aim.Validation;
using Solid.Aim.Validation.User;

namespace Solid.Aim.Services.Impl
{
    internal class UserValidator : IUserValidator
    {
        public IEnumerable<IValidationResult> Validate(UserDto dto)
        {
            return GetValidators().Select(validator => validator.Validate(dto));
        }

        private static IEnumerable<IValidator<UserDto>> GetValidators()
        {
            yield return new NameValidator();
            yield return new MinMaxAgeValidator(18, 100);
        }
    }
}
