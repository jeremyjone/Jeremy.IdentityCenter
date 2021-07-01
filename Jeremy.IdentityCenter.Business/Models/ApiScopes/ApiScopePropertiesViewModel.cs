using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jeremy.IdentityCenter.Business.Models.ApiScopes
{
    public class ApiScopePropertiesViewModel
    {
        public int ApiScopePropertyId { get; set; }

        public int ApiScopeId { get; set; }

        public string ApiScopeName { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public List<ApiScopePropertyViewModel> ApiScopeProperties { get; set; } = new List<ApiScopePropertyViewModel>();

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}
