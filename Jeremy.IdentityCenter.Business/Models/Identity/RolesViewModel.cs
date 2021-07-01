using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class RolesViewModel : RoleViewModel
    {
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
    }
}
