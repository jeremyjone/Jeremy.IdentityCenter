using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.IdentityResource
{
    public class IdentityResourceViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "显示名称")]

        public string DisplayName { get; set; }

        [Display(Name = "描述")]

        public string Description { get; set; }

        [Display(Name = "启用")]

        public bool Enabled { get; set; } = true;

        [Display(Name = "在发现文档中显示")]

        public bool ShowInDiscoveryDocument { get; set; } = true;

        [Display(Name = "必须")]

        public bool Required { get; set; }

        [Display(Name = "强调")]

        public bool Emphasize { get; set; }

        [Display(Name = "用户声明")]

        public List<string> UserClaims { get; set; } = new List<string>();

        public string UserClaimsItems { get; set; }
    }
}