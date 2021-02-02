namespace Solid.Aim.Validation
{
    public interface IValidator<in T>
    {
        IValidationResult Validate(T entity);
    }
}
