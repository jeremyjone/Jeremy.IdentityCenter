using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密码至少{2}位")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次密码不匹配")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
