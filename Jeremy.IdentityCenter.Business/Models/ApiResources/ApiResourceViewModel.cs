using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.ApiResources
{
    public class ApiResourceViewModel
    {
        [Display(Name = "在发现文档中显示")]
        public bool ShowInDiscoveryDocument { get; set; } = true;

        public int Id { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "显示名称")]
        public string DisplayName { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "用户声明")]
        public List<string> UserClaims { get; set; } = new List<string>();

        public string UserClaimsItems { get; set; }

        [Display(Name = "启用")]
        public bool Enabled { get; set; } = true;

        [Display(Name = "允许的访问令牌的签入算法")]
        public List<string> AllowedAccessTokenSigningAlgorithms { get; set; } = new List<string>();

        public string AllowedAccessTokenSigningAlgorithmsItems { get; set; }

        [Display(Name = "作用域")]
        public List<string> Scopes { get; set; } = new List<string>();

        public string ScopesItems { get; set; }
    }
}