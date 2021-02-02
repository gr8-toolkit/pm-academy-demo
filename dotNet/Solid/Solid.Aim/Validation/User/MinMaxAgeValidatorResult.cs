using Solid.Aim.Exceptions;

namespace Solid.Aim.Validation.User
{
    public class MinMaxAgeValidatorResult : ValidationResult
    {
        public MinMaxAgeValidatorResult(bool isValid, string failReason) 
            : base(isValid, failReason)
        {
        }

        public override void ThrowIfFail()
        {
            throw new MinMaxAgeValidationException("Age out of range");

        }
    }
}
