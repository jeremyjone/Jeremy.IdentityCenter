using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class RoleClaimsViewModel : RoleClaimViewModel
    {
        public string RoleName { get; set; }

        public List<RoleClaimViewModel> Claims { get; set; } = new List<RoleClaimViewModel>();

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}
