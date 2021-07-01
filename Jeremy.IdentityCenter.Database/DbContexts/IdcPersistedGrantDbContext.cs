using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Jeremy.IdentityCenter.Database.DbContexts
{
    public class IdcPersistedGrantDbContext : PersistedGrantDbContext<IdcPersistedGrantDbContext>
    {
        public IdcPersistedGrantDbContext(DbContextOptions<IdcPersistedGrantDbContext> options,
            OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {
        }
    }
}
