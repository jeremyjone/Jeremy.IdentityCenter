using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Personal
{
    public class DeleteAccountViewModel
    {
        public bool RequirePassword { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "输入密码")]
        public string Password { get; set; }
    }
}
