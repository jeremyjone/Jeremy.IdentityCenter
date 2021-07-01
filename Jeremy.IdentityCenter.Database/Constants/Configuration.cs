using Jeremy.IdentityCenter.Database.Entities;
using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Database.Constants
{
    public static class Configuration
    {
        public static readonly string DefaultPassword = "Test123";

        public static readonly List<IdcIdentityUserRole> DefaultRole = new List<IdcIdentityUserRole>
        {
            new IdcIdentityUserRole
            {
                Role = new IdcIdentityRole
                {
                    Name = "user"
                }
            }
        };
    }
}
