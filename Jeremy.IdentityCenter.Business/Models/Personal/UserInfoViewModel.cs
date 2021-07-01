using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Personal
{
    public class UserInfoViewModel
    {
        [Display(Name = "用户名")]
        public string Username { get; set; }

        public bool EmailConfirmed { get; set; }

        [Display(Name = "邮箱")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "电话")]
        [Phone]
        public string Phone { get; set; }

        [MaxLength(255)]
        [Display(Name = "昵称")]
        public string Nickname { get; set; }

        [MaxLength(255)]
        [Display(Name = "头像")]
        public string Avatar { get; set; }

        [MaxLength(255)]
        [Display(Name = "个人网址")]
        public string Website { get; set; }

        [MaxLength(255)]
        [Display(Name = "个人信息")]
        public string Profile { get; set; }

        public string StatusMessage { get; set; }
    }
}
