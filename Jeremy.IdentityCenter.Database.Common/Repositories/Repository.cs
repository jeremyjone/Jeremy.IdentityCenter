using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jeremy.IdentityCenter.Database.Common.Repositories
{
    public class Repository<TContext, TRepository> : IRepository
        where TContext : DbContext
        where TRepository : IRepository
    {
        public TContext Db { get; }
        public ILogger<TRepository> Logger { get; }

        public Repository(TContext db, ILogger<TRepository> logger)
        {
            Db = db;
            Logger = logger;
        }
    }
}
