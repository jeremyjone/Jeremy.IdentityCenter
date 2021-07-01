using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class RoleViewModel : IBaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "描述")]
        [Required]
        public string Description { get; set; }
    }
}
