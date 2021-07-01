using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.Identity
{
    public class UserProvidersViewModel : UserProviderViewModel
    {
        public List<UserProviderViewModel> Providers { get; set; } = new List<UserProviderViewModel>();
    }
}
