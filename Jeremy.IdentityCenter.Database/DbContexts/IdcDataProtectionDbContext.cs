using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jeremy.IdentityCenter.Database.DbContexts
{
    public class IdcDataProtectionDbContext : DbContext, IDataProtectionKeyContext
    {
        public IdcDataProtectionDbContext(DbContextOptions<IdcDataProtectionDbContext> options) : base(options)
        {

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}
