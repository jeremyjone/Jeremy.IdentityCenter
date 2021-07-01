using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class UsersViewModel : UserViewModel
    {
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
}
