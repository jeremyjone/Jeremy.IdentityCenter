using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "ÓÃ»§Ãû")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "×¢²áÓÊÏä")]
        [EmailAddress]
        public string Email { get; set; }
    }
}