using System.Collections.Generic;
using Solid.Aim.Contracts;
using Solid.Aim.Validation;

namespace Solid.Aim.Services
{
    public interface IUserValidator
    {
        IEnumerable<IValidationResult> Validate(UserDto dto);
    }
}
