using System.ComponentModel.DataAnnotations;
using Contrllrs.Notes.Validators;

namespace Contrllrs.Notes.Models
{
    public class User
    {
        [Required]
        [StringLength(maximumLength: 64, MinimumLength = 3)]
        public string Name { get; set; }
        
        [AgeValidator(LegalAge = 18)]
        public int Age  { get; set; }
    }
}
