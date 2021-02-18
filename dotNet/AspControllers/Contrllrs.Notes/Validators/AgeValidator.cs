using System.ComponentModel.DataAnnotations;

namespace Contrllrs.Notes.Validators
{
    public class AgeValidator : ValidationAttribute
    {
        public const int MaxAge = 200;
        public int LegalAge { get; set; }
        
        public override bool IsValid(object value)
        {
            if (value is int age)
            {
                if (age >= LegalAge && age <= MaxAge)
                {
                    return true;
                }
                else
                {
                    ErrorMessage = $"The age should be between {LegalAge} and {MaxAge}";
                }
            }
            return false;
        }
    }
}
