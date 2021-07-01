using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }

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
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [Display(Name = "当前密码")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}
