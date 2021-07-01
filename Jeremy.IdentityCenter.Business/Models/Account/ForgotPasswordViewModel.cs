using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "�û���")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "ע������")]
        [EmailAddress]
        public string Email { get; set; }
    }
}