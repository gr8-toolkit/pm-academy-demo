using System.ComponentModel.DataAnnotations;

namespace AuthWebApps.StandardScheme.Cookies.Contracts
{
    public class AuthRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(64, MinimumLength = 6)]
        public string Login { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
