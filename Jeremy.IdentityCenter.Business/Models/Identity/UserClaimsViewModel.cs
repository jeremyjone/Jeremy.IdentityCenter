using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class UserClaimsViewModel : UserClaimViewModel
    {
        public string UserName { get; set; }

        public List<UserClaimViewModel> Claims { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}
