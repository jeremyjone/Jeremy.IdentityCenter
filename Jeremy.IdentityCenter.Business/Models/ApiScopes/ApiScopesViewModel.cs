using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.ApiScopes
{
    public class ApiScopesViewModel
    {
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<ApiScopeViewModel> Scopes { get; set; } = new List<ApiScopeViewModel>();
    }
}