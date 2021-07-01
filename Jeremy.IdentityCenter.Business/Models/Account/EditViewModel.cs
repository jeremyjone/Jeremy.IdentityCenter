using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Account
{
    public class EditViewModel
    {
        [Required]
        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
