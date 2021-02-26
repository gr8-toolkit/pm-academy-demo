using Solid.Aim.Exceptions;

namespace Solid.Aim.Validation
{
    public class ValidationResult : IValidationResult
    {
        public static ValidationResult Valid = new ValidationResult(true, null);

        public bool IsValid { get; }
        public string FailReason { get; }

        public ValidationResult(bool isValid, string failReason)
        {
            IsValid = isValid;
            FailReason = failReason;
        }

        public virtual void ThrowIfFail()
        {
            if (!IsValid) throw new ValidationException(FailReason);
        }
    }
}
