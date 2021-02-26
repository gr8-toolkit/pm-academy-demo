namespace Solid.Aim.Validation
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        string FailReason { get; }
        void ThrowIfFail();
    }
}
