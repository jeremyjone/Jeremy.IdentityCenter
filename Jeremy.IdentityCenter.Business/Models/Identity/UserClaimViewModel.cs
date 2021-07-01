using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class UserClaimViewModel : IBaseViewModel
    {
        public int ClaimId { get; set; }

        public int UserId { get; set; }

        [Required]
        [Display(Name = "声明类型")]
        public string ClaimType { get; set; }

        [Required]
        [Display(Name = "声明值")]
        public string ClaimValue { get; set; }
    }
}
