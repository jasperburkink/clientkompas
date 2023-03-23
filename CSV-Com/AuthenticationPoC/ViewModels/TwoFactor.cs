using System.ComponentModel.DataAnnotations;

namespace AuthenticationPoC.ViewModels
{
    public class TwoFactor
    {
        [Required]
        public string TwoFactorCode { get; set; }

        public string ReturnUrl { get; set; }
    }
}
