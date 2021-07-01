using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class RoleClaimViewModel : IBaseViewModel
    {
        public int ClaimId { get; set; }

        public int RoleId { get; set; }

        [Required]
        [Display(Name = "声明类型")]
        public string ClaimType { get; set; }


        [Required]
        [Display(Name = "声明值")]
        public string ClaimValue { get; set; }
    }
}
