using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.IdentityResource
{
    public class IdentityResourcesViewModel
    {

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<IdentityResourceViewModel> IdentityResources { get; set; } = new List<IdentityResourceViewModel>();
    }
}