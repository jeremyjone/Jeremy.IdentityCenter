using Microsoft.AspNetCore.Identity;

namespace Jeremy.IdentityCenter.Database.Entities
{
    public class IdcIdentityUserRole : IdentityUserRole<int>
    {
        public virtual IdcIdentityUser User { get; set; }
        public virtual IdcIdentityRole Role { get; set; }
    }
}