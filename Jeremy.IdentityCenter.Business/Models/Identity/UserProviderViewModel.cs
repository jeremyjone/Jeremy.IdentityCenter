using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class UserProviderViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "提供程序秘钥")]
        public string ProviderKey { get; set; }

        [Display(Name = "登录提供程序")]
        public string LoginProvider { get; set; }

        [Display(Name = "提供程序显示名称")]
        public string ProviderDisplayName { get; set; }
    }
}
